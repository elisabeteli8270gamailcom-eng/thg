using System.Text.Json;
using TodoApp.Models;

namespace TodoApp.Services;

public class StorageService
{
    private readonly string _storagePath;
    private readonly string _todosFile;
    private readonly string _categoriesFile;

    public StorageService()
    {
        _storagePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TodoApp",
            "Data"
        );

        Directory.CreateDirectory(_storagePath);

        _todosFile = Path.Combine(_storagePath, "todos.json");
        _categoriesFile = Path.Combine(_storagePath, "categories.json");
    }

    public async Task<List<TodoItem>> LoadTodos()
    {
        try
        {
            if (!File.Exists(_todosFile))
                return [];

            var json = await File.ReadAllTextAsync(_todosFile);
            return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar todos: {ex.Message}");
            return [];
        }
    }

    public async Task SaveTodos(List<TodoItem> todos)
    {
        try
        {
            var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_todosFile, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar todos: {ex.Message}");
        }
    }

    public async Task<List<Category>> LoadCategories()
    {
        try
        {
            if (!File.Exists(_categoriesFile))
                return GetDefaultCategories();

            var json = await File.ReadAllTextAsync(_categoriesFile);
            return JsonSerializer.Deserialize<List<Category>>(json) ?? GetDefaultCategories();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar categorias: {ex.Message}");
            return GetDefaultCategories();
        }
    }

    public async Task SaveCategories(List<Category> categories)
    {
        try
        {
            var json = JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_categoriesFile, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar categorias: {ex.Message}");
        }
    }

    private static List<Category> GetDefaultCategories()
    {
        return new List<Category>
        {
            new() { Name = "Trabalho", Color = "#E74C3C" },
            new() { Name = "Pessoal", Color = "#3498DB" },
            new() { Name = "Compras", Color = "#2ECC71" },
            new() { Name = "Saúde", Color = "#F39C12" },
            new() { Name = "Outros", Color = "#95A5A6" }
        };
    }

    public async Task AddTodo(TodoItem todo)
    {
        var todos = await LoadTodos();
        todos.Add(todo);
        await SaveTodos(todos);
    }

    public async Task UpdateTodo(TodoItem todo)
    {
        var todos = await LoadTodos();
        var index = todos.FindIndex(t => t.Id == todo.Id);
        if (index >= 0)
        {
            todos[index] = todo;
            await SaveTodos(todos);
        }
    }

    public async Task DeleteTodo(Guid id)
    {
        var todos = await LoadTodos();
        todos.RemoveAll(t => t.Id == id);
        await SaveTodos(todos);
    }

    public async Task AddCategory(Category category)
    {
        var categories = await LoadCategories();
        categories.Add(category);
        await SaveCategories(categories);
    }

    public async Task DeleteCategory(Guid id)
    {
        var categories = await LoadCategories();
        categories.RemoveAll(c => c.Id == id);
        await SaveCategories(categories);
    }

    public async Task ClearCompleted()
    {
        var todos = await LoadTodos();
        todos.RemoveAll(t => t.IsCompleted);
        await SaveTodos(todos);
    }

    public async Task<List<TodoItem>> GetTodosByCategory(string category)
    {
        var todos = await LoadTodos();
        return todos.Where(t => t.Category == category).ToList();
    }

    public async Task<List<TodoItem>> GetOverdueTodos()
    {
        var todos = await LoadTodos();
        return todos.Where(t => 
            !t.IsCompleted && 
            t.DueDate.HasValue && 
            t.DueDate.Value < DateTime.Now
        ).ToList();
    }

    public async Task<Dictionary<string, int>> GetStatistics()
    {
        var todos = await LoadTodos();
        return new Dictionary<string, int>
        {
            { "Total", todos.Count },
            { "Completados", todos.Count(t => t.IsCompleted) },
            { "Pendentes", todos.Count(t => !t.IsCompleted) },
            { "Atrasados", todos.Count(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < DateTime.Now) }
        };
    }
}
