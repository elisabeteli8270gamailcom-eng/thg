namespace TodoApp.Models;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#3498DB";
    public int TodoCount { get; set; }
}
