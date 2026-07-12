using System.Windows;
using System.Windows.Controls;
using TodoApp.Core;
using TodoApp.Models;

namespace TodoApp.UI;

public partial class MainWindow : Window
{
    private TodoManager _todoManager = null!;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _todoManager = new TodoManager();
        await _todoManager.Initialize();
        
        RefreshTodos();
        UpdateStatistics();
    }

    private async void AddTodo_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddTodoWindow();
        if (dialog.ShowDialog() == true)
        {
            await _todoManager.AddTodo(
                dialog.Title,
                dialog.Description,
                dialog.Category,
                dialog.Priority,
                dialog.DueDate
            );
            RefreshTodos();
            await UpdateStatistics();
        }
    }

    private async void EditTodo_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && Guid.TryParse(button.Tag.ToString(), out var id))
        {
            var todo = TodosListBox.SelectedItem as TodoItem;
            if (todo != null)
            {
                var dialog = new AddTodoWindow(todo);
                if (dialog.ShowDialog() == true)
                {
                    todo.Title = dialog.Title;
                    todo.Description = dialog.Description;
                    todo.Category = dialog.Category;
                    todo.Priority = dialog.Priority;
                    todo.DueDate = dialog.DueDate;
                    
                    await _todoManager.UpdateTodo(todo);
                    RefreshTodos();
                    await UpdateStatistics();
                }
            }
        }
    }

    private async void DeleteTodo_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && Guid.TryParse(button.Tag.ToString(), out var id))
        {
            if (MessageBox.Show("Tem certeza que deseja deletar esta tarefa?", "Confirmação", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _todoManager.DeleteTodo(id);
                RefreshTodos();
                await UpdateStatistics();
            }
        }
    }

    private async void TodoComplete_Click(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkbox && Guid.TryParse(checkbox.Tag.ToString(), out var id))
        {
            await _todoManager.ToggleTodo(id);
            RefreshTodos();
            await UpdateStatistics();
        }
    }

    private async void ClearCompleted_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Deletar todas as tarefas completas?", "Confirmação", 
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            await _todoManager.ClearCompleted();
            RefreshTodos();
            await UpdateStatistics();
        }
    }

    private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CategoriesListBox.SelectedItem is ListBoxItem item)
        {
            var content = item.Content.ToString() ?? "Todos";
            var cleanCategory = content
                .Replace("📌 ", "")
                .Replace("⏰ ", "")
                .Replace("🏢 ", "")
                .Replace("👤 ", "")
                .Replace("🛒 ", "")
                .Replace("❤️ ", "");
            
            _todoManager.SelectedCategory = cleanCategory;
            RefreshTodos();
        }
    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
        _todoManager.SearchText = SearchBox.Text;
        RefreshTodos();
    }

    private void RefreshTodos()
    {
        TodosListBox.ItemsSource = _todoManager.GetFilteredTodos();
    }

    private async Task UpdateStatistics()
    {
        var stats = await _todoManager.GetStatistics();
        TotalCount.Text = $"Total: {stats["Total"]}";
        CompletedCount.Text = $"Completos: {stats["Completados"]}";
        PendingCount.Text = $"Pendentes: {stats["Pendentes"]}";
        OverdueCount.Text = $"Atrasados: {stats["Atrasados"]}";
    }
}
