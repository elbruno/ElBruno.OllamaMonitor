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
    private readonly MainWindowViewModel _viewModel;
    private readonly AppSettingsService _settingsService;
    private readonly DiagnosticsLogService _diagnostics;
    private readonly Action _exitAction;
    private readonly NotifyIcon _notifyIcon;
    private readonly ToolStripMenuItem _toggleWindowMenuItem;

    public TrayIconService(
        MainWindow mainWindow,
        MainWindowViewModel viewModel,
        AppSettingsService settingsService,
        DiagnosticsLogService diagnostics,
        Action exitAction)
    {
        _mainWindow = mainWindow;
        _viewModel = viewModel;
        _settingsService = settingsService;
        _diagnostics = diagnostics;
        _exitAction = exitAction;

        _toggleWindowMenuItem = new ToolStripMenuItem("Show Details", null, (_, _) => ToggleWindowVisibility());
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
            _toggleWindowMenuItem,
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

    private void ToggleWindowVisibility()
    {
        if (_mainWindow.IsVisible)
        {
            _mainWindow.Hide();
            return;
        }

        ShowWindow();
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

    private void RefreshMenuText()
    {
        _toggleWindowMenuItem.Text = _mainWindow.IsVisible ? "Hide Details" : "Show Details";
    }
}
