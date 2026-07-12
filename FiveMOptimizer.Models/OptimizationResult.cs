namespace FiveMOptimizer.Models;

public class OptimizationResult
{
    public string ModuleName { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public long ItemsProcessed { get; set; }
    public long SpaceFreed { get; set; } // em bytes
    public DateTime ExecutedAt { get; set; } = DateTime.Now;
    public TimeSpan ExecutionTime { get; set; }
}
