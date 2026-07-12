using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using FiveMOptimizer.Core;
using FiveMOptimizer.Helpers;
using FiveMOptimizer.Models;
using FiveMOptimizer.Services;

namespace FiveMOptimizer.UI;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private OptimizationManager _optimizationManager;
    private FileService _fileService;
    private SystemInfoService _systemInfoService;
    private LoggerService _logger;

    private SystemInformation? _systemInfo;
    private OptimizationSettings? _settings;
    private List<OptimizationResult> _results = [];

    public SystemInformation? SystemInfo
    {
        get => _systemInfo;
        set { _systemInfo = value; OnPropertyChanged(); }
    }

    public OptimizationSettings? Settings
    {
        get => _settings;
        set { _settings = value; OnPropertyChanged(); }
    }

    public List<OptimizationResult> Results
    {
        get => _results;
        set { _results = value; OnPropertyChanged(); }
    }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        _logger = new LoggerService();
        _fileService = new FileService(_logger);
        _systemInfoService = new SystemInfoService(_logger);
        _optimizationManager = new OptimizationManager(_fileService, _systemInfoService, _logger);

        Settings = new OptimizationSettings();
        LoadSystemInfo();
    }

    private void LoadSystemInfo()
    {
        SystemInfo = _optimizationManager.GetSystemStatus();
    }

    private async void OptimizeButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var results = await _optimizationManager.ExecuteOptimization(Settings ?? new OptimizationSettings());
            Results = results;
            LoadSystemInfo();
            MessageBox.Show("Otimização concluída com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro durante otimização: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadSystemInfo();
        MessageBox.Show("Informações atualizadas!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
