using System.Drawing;
using System.Windows.Forms;
using ElBruno.OllamaMonitor.Configuration;
using ElBruno.OllamaMonitor.Diagnostics;
using ElBruno.OllamaMonitor.Helpers;
using ElBruno.OllamaMonitor.Models;
using ElBruno.OllamaMonitor.ViewModels;

namespace ElBruno.OllamaMonitor.Services;

public sealed class TrayIconService : IDisposable
{
    private readonly MainWindow _mainWindow;
    private readonly MiniMonitorWindow _miniMonitorWindow;
    private readonly MainWindowViewModel _viewModel;
    private readonly AppSettingsService _settingsService;
    private readonly DiagnosticsLogService _diagnostics;
    private readonly Action _exitAction;
    private readonly NotifyIcon _notifyIcon;
    private readonly ToolStripMenuItem _toggleDetailsWindowMenuItem;
    private readonly ToolStripMenuItem _toggleMiniWindowMenuItem;

    public TrayIconService(
        MainWindow mainWindow,
        MiniMonitorWindow miniMonitorWindow,
        MainWindowViewModel viewModel,
        AppSettingsService settingsService,
        DiagnosticsLogService diagnostics,
        Action exitAction)
    {
        _mainWindow = mainWindow;
        _miniMonitorWindow = miniMonitorWindow;
        _viewModel = viewModel;
        _settingsService = settingsService;
        _diagnostics = diagnostics;
        _exitAction = exitAction;

        _toggleDetailsWindowMenuItem = new ToolStripMenuItem("Show Details", null, (_, _) => ToggleDetailsWindowVisibility());
        _toggleMiniWindowMenuItem = new ToolStripMenuItem("Show Mini Monitor", null, (_, _) => ToggleMiniWindowVisibility());
        _notifyIcon = new NotifyIcon
        {
            Visible = true,
            Text = "Ollama: Starting",
            Icon = SystemIcons.Application,
            ContextMenuStrip = new ContextMenuStrip()
        };

        _notifyIcon.DoubleClick += (_, _) => ShowWindow();
        _notifyIcon.ContextMenuStrip.Opening += (_, _) => RefreshMenuText();
        _notifyIcon.ContextMenuStrip.Items.AddRange(
        [
            _toggleDetailsWindowMenuItem,
            _toggleMiniWindowMenuItem,
            new ToolStripMenuItem("Refresh", null, async (_, _) => await _viewModel.RefreshAsync(CancellationToken.None)),
            new ToolStripMenuItem("Copy Status", null, (_, _) => _viewModel.CopyStatusCommand.Execute(null)),
            new ToolStripMenuItem("Open Ollama API", null, (_, _) => _viewModel.OpenEndpointCommand.Execute(null)),
            new ToolStripMenuItem("Open Config Folder", null, (_, _) => ProcessLauncher.Open(AppPaths.RootDirectory, _diagnostics)),
            new ToolStripSeparator(),
            new ToolStripMenuItem("Exit", null, (_, _) => _exitAction())
        ]);

        _viewModel.SnapshotUpdated += (_, snapshot) => ApplySnapshot(snapshot);
    }

    public void Dispose()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
    }

    private void ApplySnapshot(OllamaMonitorSnapshot snapshot)
    {
        _notifyIcon.Icon = snapshot.State switch
        {
            OllamaMonitorState.Running => SystemIcons.Information,
            OllamaMonitorState.ModelLoaded => SystemIcons.Shield,
            OllamaMonitorState.HighUsage => SystemIcons.Warning,
            OllamaMonitorState.Error => SystemIcons.Error,
            _ => SystemIcons.Error
        };

        _notifyIcon.Text = StatusTextHelper.BuildTooltip(snapshot);
    }

    private void ToggleDetailsWindowVisibility()
    {
        if (_mainWindow.IsVisible)
        {
            _mainWindow.Hide();
            return;
        }

        ShowWindow();
    }

    private void ToggleMiniWindowVisibility()
    {
        if (_miniMonitorWindow.IsVisible)
        {
            _miniMonitorWindow.Hide();
            return;
        }

        ShowMiniMonitorWindow();
    }

    private void ShowWindow()
    {
        if (!_mainWindow.IsVisible)
        {
            _mainWindow.Show();
        }

        if (_mainWindow.WindowState == System.Windows.WindowState.Minimized)
        {
            _mainWindow.WindowState = System.Windows.WindowState.Normal;
        }

        _mainWindow.Activate();
    }

    private void ShowMiniMonitorWindow()
    {
        if (!_miniMonitorWindow.IsVisible)
        {
            _miniMonitorWindow.Show();
        }

        _miniMonitorWindow.Activate();
    }

    private void RefreshMenuText()
    {
        _toggleDetailsWindowMenuItem.Text = _mainWindow.IsVisible ? "Hide Details" : "Show Details";
        _toggleMiniWindowMenuItem.Text = _miniMonitorWindow.IsVisible ? "Hide Mini Monitor" : "Show Mini Monitor";
    }
}
