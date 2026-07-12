using FiveMOptimizer.Models;
using FiveMOptimizer.Services;

namespace FiveMOptimizer.Core;

public class OptimizationManager
{
    private readonly FileService _fileService;
    private readonly SystemInfoService _systemInfoService;
    private readonly LoggerService _logger;
    private readonly List<OptimizationResult> _results = [];

    public OptimizationManager(FileService fileService, SystemInfoService systemInfoService, LoggerService logger)
    {
        _fileService = fileService;
        _systemInfoService = systemInfoService;
        _logger = logger;
    }

    public async Task<List<OptimizationResult>> ExecuteOptimization(OptimizationSettings settings)
    {
        _results.Clear();
        _logger.LogInfo("=== Iniciando otimização do sistema ===");

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            if (settings.EnableCleanTemp)
                await OptimizeCleanTemp();

            if (settings.EnableCleanCache)
                await OptimizeCleanCache();

            if (settings.EnableMemoryOptimization)
                await OptimizeMemory();

            stopwatch.Stop();
            _logger.LogSuccess($"=== Otimização concluída em {stopwatch.ElapsedMilliseconds}ms ===");
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro durante otimização", ex);
        }

        return _results;
    }

    private async Task OptimizeCleanTemp()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            long spaceFreed = _fileService.CleanTemporaryFiles();
            
            sw.Stop();
            _results.Add(new OptimizationResult
            {
                ModuleName = "Limpeza de Arquivos Temporários",
                Success = true,
                Message = "Arquivos temporários foram limpos com sucesso",
                SpaceFreed = spaceFreed,
                ExecutionTime = sw.Elapsed
            });
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError("Erro ao limpar temp", ex);
            _results.Add(new OptimizationResult
            {
                ModuleName = "Limpeza de Arquivos Temporários",
                Success = false,
                Message = $"Erro: {ex.Message}",
                ExecutionTime = sw.Elapsed
            });
        }

        await Task.CompletedTask;
    }

    private async Task OptimizeCleanCache()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            long spaceFreed = _fileService.CleanRecycleBin();
            
            sw.Stop();
            _results.Add(new OptimizationResult
            {
                ModuleName = "Limpeza de Cache",
                Success = true,
                Message = "Cache foi limpo com sucesso",
                SpaceFreed = spaceFreed,
                ExecutionTime = sw.Elapsed
            });
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError("Erro ao limpar cache", ex);
            _results.Add(new OptimizationResult
            {
                ModuleName = "Limpeza de Cache",
                Success = false,
                Message = $"Erro: {ex.Message}",
                ExecutionTime = sw.Elapsed
            });
        }

        await Task.CompletedTask;
    }

    private async Task OptimizeMemory()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            sw.Stop();
            _results.Add(new OptimizationResult
            {
                ModuleName = "Otimização de Memória",
                Success = true,
                Message = "Memória foi otimizada com sucesso",
                ExecutionTime = sw.Elapsed
            });
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError("Erro ao otimizar memória", ex);
            _results.Add(new OptimizationResult
            {
                ModuleName = "Otimização de Memória",
                Success = false,
                Message = $"Erro: {ex.Message}",
                ExecutionTime = sw.Elapsed
            });
        }

        await Task.CompletedTask;
    }

    public SystemInformation GetSystemStatus()
    {
        return _systemInfoService.GetSystemInformation();
    }

    public List<OptimizationResult> GetResults() => _results;
}
