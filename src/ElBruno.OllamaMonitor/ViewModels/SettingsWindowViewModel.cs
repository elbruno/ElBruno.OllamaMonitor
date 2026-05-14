using ElBruno.OllamaMonitor.Configuration;
using ElBruno.OllamaMonitor.Models;

namespace ElBruno.OllamaMonitor.ViewModels;

public sealed class SettingsWindowViewModel : ViewModelBase
{
    private readonly AppSettings _settings;
    private readonly AppSettingsService _settingsService;

    private bool _enableNotifications;
    private bool _notifyOllamaOffline;
    private bool _notifyOllamaOnline;
    private bool _notifyModelLoaded;
    private bool _notifyModelUnloaded;
    private bool _notifyHighCpu;
    private bool _notifyHighMemory;
    private bool _notifyHighGpu;
    private int _notificationDebounceSeconds;
    private bool _startMinimizedToTray;
    private bool _showFloatingWindowOnStart;

    public SettingsWindowViewModel(AppSettings settings, AppSettingsService settingsService)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

        LoadSettings();
    }

    public bool EnableNotifications
    {
        get => _enableNotifications;
        set => SetProperty(ref _enableNotifications, value);
    }

    public bool NotifyOllamaOffline
    {
        get => _notifyOllamaOffline;
        set => SetProperty(ref _notifyOllamaOffline, value);
    }

    public bool NotifyOllamaOnline
    {
        get => _notifyOllamaOnline;
        set => SetProperty(ref _notifyOllamaOnline, value);
    }

    public bool NotifyModelLoaded
    {
        get => _notifyModelLoaded;
        set => SetProperty(ref _notifyModelLoaded, value);
    }

    public bool NotifyModelUnloaded
    {
        get => _notifyModelUnloaded;
        set => SetProperty(ref _notifyModelUnloaded, value);
    }

    public bool NotifyHighCpu
    {
        get => _notifyHighCpu;
        set => SetProperty(ref _notifyHighCpu, value);
    }

    public bool NotifyHighMemory
    {
        get => _notifyHighMemory;
        set => SetProperty(ref _notifyHighMemory, value);
    }

    public bool NotifyHighGpu
    {
        get => _notifyHighGpu;
        set => SetProperty(ref _notifyHighGpu, value);
    }

    public int NotificationDebounceSeconds
    {
        get => _notificationDebounceSeconds;
        set => SetProperty(ref _notificationDebounceSeconds, Math.Max(5, value));
    }

    public bool StartMinimizedToTray
    {
        get => _startMinimizedToTray;
        set => SetProperty(ref _startMinimizedToTray, value);
    }

    public bool ShowFloatingWindowOnStart
    {
        get => _showFloatingWindowOnStart;
        set => SetProperty(ref _showFloatingWindowOnStart, value);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        var notificationEvents = BuildNotificationFlags();

        var updatedSettings = _settings with
        {
            EnableNotifications = EnableNotifications,
            NotificationEvents = notificationEvents,
            NotificationDebounceSeconds = NotificationDebounceSeconds,
            StartMinimizedToTray = StartMinimizedToTray,
            ShowFloatingWindowOnStart = ShowFloatingWindowOnStart
        };

        await _settingsService.SaveAsync(updatedSettings, cancellationToken);
    }

    private void LoadSettings()
    {
        EnableNotifications = _settings.EnableNotifications;
        NotificationDebounceSeconds = _settings.NotificationDebounceSeconds;
        StartMinimizedToTray = _settings.StartMinimizedToTray;
        ShowFloatingWindowOnStart = _settings.ShowFloatingWindowOnStart;

        NotifyOllamaOffline = (_settings.NotificationEvents & NotificationEventType.OllamaOffline) != 0;
        NotifyOllamaOnline = (_settings.NotificationEvents & NotificationEventType.OllamaOnline) != 0;
        NotifyModelLoaded = (_settings.NotificationEvents & NotificationEventType.ModelLoaded) != 0;
        NotifyModelUnloaded = (_settings.NotificationEvents & NotificationEventType.ModelUnloaded) != 0;
        NotifyHighCpu = (_settings.NotificationEvents & NotificationEventType.HighCpuUsage) != 0;
        NotifyHighMemory = (_settings.NotificationEvents & NotificationEventType.HighMemoryUsage) != 0;
        NotifyHighGpu = (_settings.NotificationEvents & NotificationEventType.HighGpuUsage) != 0;
    }

    private NotificationEventType BuildNotificationFlags()
    {
        var flags = NotificationEventType.None;

        if (NotifyOllamaOffline)
            flags |= NotificationEventType.OllamaOffline;
        if (NotifyOllamaOnline)
            flags |= NotificationEventType.OllamaOnline;
        if (NotifyModelLoaded)
            flags |= NotificationEventType.ModelLoaded;
        if (NotifyModelUnloaded)
            flags |= NotificationEventType.ModelUnloaded;
        if (NotifyHighCpu)
            flags |= NotificationEventType.HighCpuUsage;
        if (NotifyHighMemory)
            flags |= NotificationEventType.HighMemoryUsage;
        if (NotifyHighGpu)
            flags |= NotificationEventType.HighGpuUsage;

        return flags;
    }

    public void Dispose()
    {
        // Cleanup if needed in the future
    }
}

