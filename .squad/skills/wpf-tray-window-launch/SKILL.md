---
name: "wpf-tray-window-launch"
description: "Pattern for launching WPF windows from system tray icon interactions"
domain: "desktop-ui"
confidence: "high"
source: "observed"
---

## Context

When building WPF applications with system tray integration, you need a consistent pattern for launching/showing windows in response to tray icon events (double-click, context menu items).

**ElBruno.OllamaMonitor uses `H.NotifyIcon.Wpf` for tray integration** (via NuGet package, wraps System.Windows.Forms.NotifyIcon).

## Patterns

### 1. Window Lifecycle Pattern

TrayIconService holds **direct references** to window instances (not factories):

```csharp
private readonly MainWindow _mainWindow;
private readonly MiniMonitorWindow _miniMonitorWindow;
```

Windows are constructed once in `App.xaml.cs` and passed to TrayIconService constructor.

### 2. Show/Activate Pattern

Standard method for showing a window from tray (used by both windows):

```csharp
private void ShowMiniMonitorWindow()
{
    if (!_miniMonitorWindow.IsVisible)
    {
        _miniMonitorWindow.Show();
    }
    
    _miniMonitorWindow.Activate();
}
```

**Key steps:**
1. Check `IsVisible` first
2. Call `Show()` if hidden
3. Always call `Activate()` to bring to front (handles minimized case implicitly)

**MainWindow variation** includes explicit minimized state check:

```csharp
if (_mainWindow.WindowState == System.Windows.WindowState.Minimized)
{
    _mainWindow.WindowState = System.Windows.WindowState.Normal;
}
```

### 3. Prevent Window Close Pattern

Windows implement `PrepareForExit()` + `OnClosing` override to hide instead of close:

```csharp
private bool _allowClose;

public void PrepareForExit() => _allowClose = true;

protected override void OnClosing(CancelEventArgs e)
{
    if (!_allowClose)
    {
        e.Cancel = true;
        Hide();
        return;
    }
    base.OnClosing(e);
}
```

This allows reopening windows without re-instantiation.

### 4. Tray Event Wiring

Wire tray icon events in TrayIconService constructor:

```csharp
_notifyIcon.DoubleClick += (_, _) => ShowMiniMonitorWindow();
_notifyIcon.ContextMenuStrip.Opening += (_, _) => RefreshMenuText();
```

Context menu items use lambda handlers:

```csharp
new ToolStripMenuItem("Show Mini Monitor", null, (_, _) => ToggleMiniWindowVisibility())
```

### 5. Context Menu Toggle Pattern

Menu items toggle window visibility with dynamic text:

```csharp
private void ToggleMiniWindowVisibility()
{
    if (_miniMonitorWindow.IsVisible)
    {
        _miniMonitorWindow.Hide();
        return;
    }
    ShowMiniMonitorWindow();
}

private void RefreshMenuText()
{
    _toggleMiniWindowMenuItem.Text = _miniMonitorWindow.IsVisible 
        ? "Hide Mini Monitor" 
        : "Show Mini Monitor";
}
```

## Examples

**File:** `src/ElBruno.OllamaMonitor/Services/TrayIconService.cs`

- Lines 24, 14: Window instance fields
- Lines 109-122: `ShowWindow()` implementation (MainWindow)
- Lines 124-132: `ShowMiniMonitorWindow()` implementation
- Line 50: Double-click event wiring
- Lines 40-41: Context menu item creation
- Lines 88-96, 98-107: Toggle methods

**File:** `src/ElBruno.OllamaMonitor/MiniMonitorWindow.xaml.cs`

- Lines 28-40: `PrepareForExit()` + `OnClosing()` override

## Anti-Patterns

1. **Don't create new window instances on each show** — reuse existing instances for tray-launched windows
2. **Don't rely solely on `Show()`** — always call `Activate()` to handle already-visible-but-background windows
3. **Don't forget `PrepareForExit()`** — without it, closing a window during app exit triggers unwanted hide logic
4. **Don't skip context menu text refresh** — static menu items confuse users when window state changes
