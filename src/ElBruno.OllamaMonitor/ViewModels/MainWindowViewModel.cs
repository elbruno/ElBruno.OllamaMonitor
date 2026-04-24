using System.Collections.ObjectModel;
using ElBruno.OllamaMonitor.Configuration;
using ElBruno.OllamaMonitor.Diagnostics;
using ElBruno.OllamaMonitor.Helpers;
using ElBruno.OllamaMonitor.Models;
using ElBruno.OllamaMonitor.Services;

namespace ElBruno.OllamaMonitor.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private readonly OllamaStatusService _statusService;
    private readonly AppSettingsService _settingsService;
    private readonly DiagnosticsLogService _diagnostics;
    private readonly Action _hideWindow;
    private readonly Action<string> _copyToClipboard;
    private readonly Action<string> _openUrl;
    private readonly SemaphoreSlim _refreshGate = new(1, 1);
    private OllamaMonitorSnapshot? _latestSnapshot;

    private string _stateText = "Starting";
    private string _endpoint = "http://localhost:11434";
    private string _versionText = "Version: Unavailable";
    private string _lastCheckedText = "Not checked yet";
    private string _apiReachableText = "API Reachable: Unknown";
    private string _processStatusText = "Process: Detecting";
    private string _cpuText = "CPU: Unavailable";
    private string _memoryText = "Memory: Unavailable";
    private string _privateMemoryText = "Private Memory: Unavailable";
    private string _diskReadText = "Disk Read: Unavailable";
    private string _diskWriteText = "Disk Write: Unavailable";
    private string _gpuText = "GPU: Unavailable";
    private string _compactGpuText = "GPU: Unavailable";
    private string _gpuMemoryText = "VRAM: Unavailable";
    private string _modelsSummaryText = "No loaded models.";
    private string _primaryModelText = "Model: No loaded models";
    private string? _errorText;

    public MainWindowViewModel(
        OllamaStatusService statusService,
        AppSettingsService settingsService,
        DiagnosticsLogService diagnostics,
        Action hideWindow,
        Action<string> copyToClipboard,
        Action<string> openUrl)
    {
        _statusService = statusService;
        _settingsService = settingsService;
        _diagnostics = diagnostics;
        _hideWindow = hideWindow;
        _copyToClipboard = copyToClipboard;
        _openUrl = openUrl;

        Models = new ObservableCollection<OllamaModelSnapshot>();
        RefreshCommand = new AsyncRelayCommand(() => RefreshAsync(CancellationToken.None));
        CopyStatusCommand = new RelayCommand(CopyStatus);
        OpenEndpointCommand = new RelayCommand(OpenEndpoint);
        HideWindowCommand = new RelayCommand(() => _hideWindow());
    }

    public event EventHandler<OllamaMonitorSnapshot>? SnapshotUpdated;

    public ObservableCollection<OllamaModelSnapshot> Models { get; }

    public AsyncRelayCommand RefreshCommand { get; }

    public RelayCommand CopyStatusCommand { get; }

    public RelayCommand OpenEndpointCommand { get; }

    public RelayCommand HideWindowCommand { get; }

    public string StateText
    {
        get => _stateText;
        private set => SetProperty(ref _stateText, value);
    }

    public string Endpoint
    {
        get => _endpoint;
        private set => SetProperty(ref _endpoint, value);
    }

    public string VersionText
    {
        get => _versionText;
        private set => SetProperty(ref _versionText, value);
    }

    public string LastCheckedText
    {
        get => _lastCheckedText;
        private set => SetProperty(ref _lastCheckedText, value);
    }

    public string ApiReachableText
    {
        get => _apiReachableText;
        private set => SetProperty(ref _apiReachableText, value);
    }

    public string ProcessStatusText
    {
        get => _processStatusText;
        private set => SetProperty(ref _processStatusText, value);
    }

    public string CpuText
    {
        get => _cpuText;
        private set => SetProperty(ref _cpuText, value);
    }

    public string MemoryText
    {
        get => _memoryText;
        private set => SetProperty(ref _memoryText, value);
    }

    public string PrivateMemoryText
    {
        get => _privateMemoryText;
        private set => SetProperty(ref _privateMemoryText, value);
    }

    public string DiskReadText
    {
        get => _diskReadText;
        private set => SetProperty(ref _diskReadText, value);
    }

    public string DiskWriteText
    {
        get => _diskWriteText;
        private set => SetProperty(ref _diskWriteText, value);
    }

    public string GpuText
    {
        get => _gpuText;
        private set => SetProperty(ref _gpuText, value);
    }

    public string CompactGpuText
    {
        get => _compactGpuText;
        private set => SetProperty(ref _compactGpuText, value);
    }

    public string GpuMemoryText
    {
        get => _gpuMemoryText;
        private set => SetProperty(ref _gpuMemoryText, value);
    }

    public string ModelsSummaryText
    {
        get => _modelsSummaryText;
        private set => SetProperty(ref _modelsSummaryText, value);
    }

    public string PrimaryModelText
    {
        get => _primaryModelText;
        private set => SetProperty(ref _primaryModelText, value);
    }

    public string ErrorText
    {
        get => _errorText ?? string.Empty;
        private set
        {
            if (SetProperty(ref _errorText, value))
            {
                OnPropertyChanged(nameof(HasError));
            }
        }
    }

    public bool HasError => !string.IsNullOrWhiteSpace(_errorText);

    public async Task RefreshAsync(CancellationToken cancellationToken)
    {
        await _refreshGate.WaitAsync(cancellationToken);
        try
        {
            var settings = await _settingsService.LoadAsync(cancellationToken);
            var snapshot = await _statusService.GetSnapshotAsync(settings, cancellationToken);
            ApplySnapshot(snapshot);
            SnapshotUpdated?.Invoke(this, snapshot);
        }
        catch (OperationCanceledException)
        {
            // Shutdown path.
        }
        catch (Exception exception)
        {
            _diagnostics.WriteError("Snapshot refresh failed.", exception);
            ErrorText = exception.Message;
        }
        finally
        {
            _refreshGate.Release();
        }
    }

    private void ApplySnapshot(OllamaMonitorSnapshot snapshot)
    {
        _latestSnapshot = snapshot;
        StateText = StatusTextHelper.GetStateLabel(snapshot.State);
        Endpoint = snapshot.Endpoint;
        VersionText = $"Version: {snapshot.Version ?? "Unavailable"}";
        LastCheckedText = snapshot.LastChecked.ToString("yyyy-MM-dd HH:mm:ss");
        ApiReachableText = $"API Reachable: {(snapshot.IsApiReachable ? "Yes" : "No")}";
        ProcessStatusText = $"Process: {snapshot.Resources?.ProcessName ?? "Not detected"}";
        CpuText = $"CPU: {StatusTextHelper.FormatPercent(snapshot.Resources?.CpuPercent)}";
        MemoryText = $"Memory: {StatusTextHelper.FormatBytes(snapshot.Resources?.MemoryBytes)}";
        PrivateMemoryText = $"Private Memory: {StatusTextHelper.FormatBytes(snapshot.Resources?.PrivateMemoryBytes)}";
        DiskReadText = $"Disk Read: {StatusTextHelper.FormatBytesPerSecond(snapshot.Resources?.DiskReadBytesPerSecond)}";
        DiskWriteText = $"Disk Write: {StatusTextHelper.FormatBytesPerSecond(snapshot.Resources?.DiskWriteBytesPerSecond)}";
        GpuText = $"GPU: {(snapshot.Resources is null ? "Unavailable" : StatusTextHelper.BuildGpuSummary(snapshot.Resources))}";
        CompactGpuText = $"GPU: {StatusTextHelper.BuildCompactGpuSummary(snapshot.Resources)}";
        GpuMemoryText = $"VRAM: {StatusTextHelper.FormatBytes(snapshot.Resources?.VramUsedBytes)} / {StatusTextHelper.FormatBytes(snapshot.Resources?.VramTotalBytes)}";
        ModelsSummaryText = snapshot.Models.Count == 0
            ? "No loaded models."
            : $"{snapshot.Models.Count} loaded model(s).";
        PrimaryModelText = snapshot.Models.Count == 0
            ? "Model: No loaded models"
            : $"Model: {snapshot.Models[0].Name}";
        ErrorText = snapshot.ErrorMessage ?? string.Empty;

        Models.Clear();
        foreach (var model in snapshot.Models)
        {
            Models.Add(model);
        }
    }

    private void CopyStatus()
    {
        if (_latestSnapshot is null)
        {
            return;
        }

        _copyToClipboard(SnapshotFormatter.ToMultilineStatus(_latestSnapshot));
    }

    private void OpenEndpoint()
    {
        if (_latestSnapshot is null)
        {
            return;
        }

        _openUrl(_latestSnapshot.Endpoint);
    }
}
