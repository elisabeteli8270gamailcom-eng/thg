using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Core;

public class TodoManager : INotifyPropertyChanged
{
    private readonly StorageService _storageService;
    private ObservableCollection<TodoItem> _todos = [];
    private ObservableCollection<Category> _categories = [];
    private string _selectedCategory = "Todos";
    private string _searchText = "";

    public ObservableCollection<TodoItem> Todos
    {
        get => _todos;
        set { _todos = value; OnPropertyChanged(); }
    }

    public ObservableCollection<Category> Categories
    {
        get => _categories;
        set { _categories = value; OnPropertyChanged(); }
    }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set { _selectedCategory = value; OnPropertyChanged(); RefreshTodos(); }
    }

    public string SearchText
    {
        get => _searchText;
        set { _searchText = value; OnPropertyChanged(); RefreshTodos(); }
    }

    public TodoManager()
    {
        _storageService = new StorageService();
    }

    public async Task Initialize()
    {
        await LoadCategories();
        await LoadTodos();
    }

    public async Task LoadTodos()
    {
        var todos = await _storageService.LoadTodos();
        Todos = new ObservableCollection<TodoItem>(todos);
        RefreshTodos();
    }

    public async Task LoadCategories()
    {
        var categories = await _storageService.LoadCategories();
        Categories = new ObservableCollection<Category>(categories);
    }

    public async Task AddTodo(string title, string description, string category, string priority, DateTime? dueDate)
    {
        var todo = new TodoItem
        {
            Title = title,
            Description = description,
            Category = category,
            Priority = priority,
            DueDate = dueDate
        };

        await _storageService.AddTodo(todo);
        Todos.Add(todo);
        RefreshTodos();
    }

    public async Task UpdateTodo(TodoItem todo)
    {
        await _storageService.UpdateTodo(todo);
        var index = Todos.ToList().FindIndex(t => t.Id == todo.Id);
        if (index >= 0)
        {
            Todos[index] = todo;
        }
        RefreshTodos();
    }

    public async Task DeleteTodo(Guid id)
    {
        await _storageService.DeleteTodo(id);
        Todos.Remove(Todos.FirstOrDefault(t => t.Id == id)!);
        RefreshTodos();
    }

    public async Task ToggleTodo(Guid id)
    {
        var todo = Todos.FirstOrDefault(t => t.Id == id);
        if (todo != null)
        {
            todo.IsCompleted = !todo.IsCompleted;
            await UpdateTodo(todo);
        }
    }

    public async Task ClearCompleted()
    {
        await _storageService.ClearCompleted();
        await LoadTodos();
    }

    public async Task<Dictionary<string, int>> GetStatistics()
    {
        return await _storageService.GetStatistics();
    }

    public ObservableCollection<TodoItem> GetFilteredTodos()
    {
        var filtered = Todos.AsEnumerable();

        if (SelectedCategory != "Todos" && SelectedCategory != "Atrasados")
            filtered = filtered.Where(t => t.Category == SelectedCategory);

        if (SelectedCategory == "Atrasados")
            filtered = filtered.Where(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < DateTime.Now);

        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(t => 
                t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                t.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            );

        return new ObservableCollection<TodoItem>(filtered);
    }

    private void RefreshTodos()
    {
        OnPropertyChanged(nameof(Todos));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
