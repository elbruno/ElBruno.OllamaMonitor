namespace ElBruno.OllamaMonitor.Configuration;

public sealed record AppSettings
{
    public string Endpoint { get; init; } = "http://localhost:11434";
    public int RefreshIntervalSeconds { get; init; } = 2;
    public bool StartMinimizedToTray { get; init; } = true;
    public bool ShowFloatingWindowOnStart { get; init; } = false;
    public bool EnableGpuMetrics { get; init; } = true;
    public bool EnableDiskMetrics { get; init; } = true;
    public double HighCpuThresholdPercent { get; init; } = 80;
    public double HighMemoryThresholdGb { get; init; } = 16;
    public double HighGpuThresholdPercent { get; init; } = 85;
}
