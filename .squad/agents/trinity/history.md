# Trinity — Project History

## Phase 2c: Settings UX Implementation (2026-04-28 planned)

### Upcoming Ownership: Trinity Window/UI/Menu Wiring

**Context:** Neo completed Settings UX architecture analysis. Recommendation approved: do both tray menu entry + dedicated Settings form, phased across 2a/2b.

**Trinity responsibilities (Phase 2a/2b):**
- Add ToolStripMenuItem("Settings…") to TrayIconService.cs (before "Open Config Folder")
- Create SettingsWindow.xaml (Phase 2a: read-only display; Phase 2b: editable Tier 1 fields)
- Implement ShowSettingsWindow() in App.xaml.cs with single-instance semantics
- Handle window lifecycle (show/hide, shutdown integration)
- Async Save handler coordination (Phase 2a: no-op spinner; Phase 2b: call Tank validation methods)
- Display layout: Tier 1 (Endpoint, RefreshIntervalSeconds) + Tier 2 (GPU/Disk flags, thresholds)

**Tank/Switch responsibilities:** Validation logic extension, build/smoke test verification.

**Decision file:** `.squad/decisions.md` (Settings UX Architecture section).

---

## Phase 2a: Quick Wins Implementation (2026-04-26)

### Completed Tasks

#### T2.1: Mini Monitor — Move "Always on Top" from header to status bar
- **Change:** Removed TextBlock with "Always on top" text from header (Row 0 StackPanel)
- **Result:** Freed ~25px of vertical space in mini window header
- **Implementation:** Added footer Grid (Row 3) with 2-column layout to display both "Always on top" and "Last checked" text
- **Status:** ✅ Complete, verified visually in XAML

#### T2.2: Mini Monitor — Increase model label font size (12px → 14px)
- **Change:** Added explicit `FontSize="14"` to model display TextBlock
- **Result:** Improved readability of model names in the mini monitor
- **Verification:** Window still fits 3 models at 310px height (no regression)
- **Status:** ✅ Complete

#### T2.3: Add app icon to windows (Mini Monitor + Details Window)
- **Asset:** Used existing `Assets/TrayIcons/tray-green.ico` (green Ollama logo, 16×16 px)
- **Implementation:** Added `Icon="Assets/TrayIcons/tray-green.ico"` to both:
  - MiniMonitorWindow.xaml
  - MainWindow.xaml (details window)
- **Result:** Icons now appear in both window title bars (visible in taskbar and window chrome)
- **Status:** ✅ Complete

#### T2.4: Implement in-use model status indicator (green bullet)
- **New Model Property:** Added `IsActive` computed property to OllamaModelSnapshot
  - Logic: `ExpiresAt == null || DateTime.UtcNow < ExpiresAt.Value.UtcDateTime`
  - Indicates model is currently loaded/active
- **Value Converter:** Created `BoolToColorConverter` in Helpers/
  - Maps `IsActive: true` → Green brush (RGB 34, 197, 94)
  - Maps `IsActive: false` → Gray brush (RGB 107, 114, 128)
  - Returns SolidColorBrush (not Color) to bind to TextBlock.Foreground
- **UI Implementation:** Replaced simple CompactModelsText display with ItemsControl
  - Each model renders as: `● [ModelName]` where bullet color reflects IsActive state
  - Font size: 14px (matches T2.2 increase)
  - Margin: 2px between items
- **Architecture Note:** Used Run elements in TextBlock.Inlines to apply converter to bullet while keeping model name plain text
- **Status:** ✅ Complete, build verified

### Technical Learnings

1. **XAML namespace gotchas:** ItemsControl with DataTemplate binding requires proper xmlns:local declaration for converters
2. **Ambiguous Color references:** System.Drawing.Color vs System.Windows.Media.Color causes compilation errors—must fully qualify or alias
3. **WPF binding best practices:**
   - Converters should return Brush types for Foreground binding (not Color)
   - SolidColorBrush instances can be static readonly for performance
4. **Model snapshots vs ViewModel:** Mini monitor receives MainWindowViewModel.Models collection directly—no need to duplicate formatting logic in ViewModel

### Architecture Decisions Made

1. **Status Indicator Design:** Used ItemsControl + DataTemplate instead of ViewModel string formatting
   - Advantage: Reusable, clean separation of concerns
   - Keeps CompactModelsText available for other uses (details window, clipboard)
   
2. **Converter Strategy:** Chose SolidColorBrush approach for performance and clarity
   - Static readonly instances avoid GC pressure on repeated binding evaluation

### Files Modified

- `src/ElBruno.OllamaMonitor/MiniMonitorWindow.xaml`
  - Restructured Row 0 header (removed nested StackPanel)
  - Updated Row 3 footer to Grid layout
  - Replaced CompactModelsText TextBlock with ItemsControl
  - Added local namespace for converter access
  
- `src/ElBruno.OllamaMonitor/MainWindow.xaml`
  - Added window icon attribute

- `src/ElBruno.OllamaMonitor/Models/OllamaModelSnapshot.cs`
  - Added IsActive property

- `src/ElBruno.OllamaMonitor/Helpers/BoolToColorConverter.cs` (new file)
  - Implements IValueConverter for bool → Brush conversion

### Testing Checklist (All Passed ✅)

- ✅ Build succeeds: `dotnet build` (0 errors)
- ✅ No console errors
- ✅ Window dimensions reasonable (~310px height for mini monitor)
- ✅ Text legible with font size increase
- ✅ Icons present in window title bars
- ✅ Footer layout preserves "Always on top" and "Last checked" labels

### Next Steps (Phase 2b)

- Build history buffer for sparklines
- Implement Canvas-based usage charts
- Polish details window styling
- Test rendering performance with 3+ models

### No Blockers Encountered

All tasks completed successfully with no architectural constraints or dependencies discovered. Ready for Phase 2b chart implementation.

---

## Screenshot Capture Session (2026-04-25 14:16)

### Environment

- **Ollama Version:** 0.21.2
- **Ollama Status:** Running on localhost:11434
- **Build Configuration:** Debug (net10.0-windows)
- **Runtime Status:** Application launched and captured successfully

### Available Models

| Model | Size (GB) |
|-------|-----------|
| deepseek-coder-v2:16b | 8.9 |
| gemma4:latest | 9.6 |
| qwen2.5-coder:latest | 4.7 |
| gpt-oss:20b | 13.8 |
| gemma4:e2b | 7.2 |
| llama3.2:latest | 2.0 |
| phi4-mini:latest | 2.5 |
| ministral-3:latest | 6.0 |
| deepseek-r1:latest | 5.2 |
| devstral-small-2:latest | 15.2 |
| gpt-oss:120b | 65.4 |

### Screenshots Captured

| Filename | Size | Description |
|----------|------|-------------|
| `mini-window.png` | 140.98 KB | Compact tray monitor view (captured 14:16:54) |
| `main-window.png` | 145.63 KB | Main expanded window showing model and metrics (captured 14:16:36) |
| `dashboard-metrics.png` | 145.35 KB | Dashboard view with additional metrics state (captured 14:16:38) |

**Location:** `docs/assets/`

### Notes

- All screenshots captured at full desktop resolution (1920×1080)
- Application running with ~130 MB memory footprint
- No errors or issues encountered during capture
- Models were populated and ready for monitoring
- Window icons successfully displaying in title bars (green Ollama icon)

---

## Tray Double-Click Behavior Update (2026-04-26)

### Task: Update default tray icon double-click action

**Requested by:** Bruno Capuano

**Objective:** Change the system tray icon double-click behavior to open the Mini Monitor window instead of the Details (Main) window.

### Implementation

**File Modified:** `src/ElBruno.OllamaMonitor/Services/TrayIconService.cs`

**Change:** Line 50
- **Before:** `_notifyIcon.DoubleClick += (_, _) => ShowWindow();`
- **After:** `_notifyIcon.DoubleClick += (_, _) => ShowMiniMonitorWindow();`

### Learnings

1. **Tray double-click handler location:** `TrayIconService.cs` line 50 wires the `NotifyIcon.DoubleClick` event
2. **Window launch pattern:** Both `ShowWindow()` (lines 109-122) and `ShowMiniMonitorWindow()` (lines 124-132) follow the same pattern:
   - Check if window is already visible; if not, call `Show()`
   - If minimized, restore to normal state (MainWindow only has this check)
   - Call `Activate()` to bring window to front
3. **No configurable setting:** AppSettings.cs does not contain a tray double-click target setting—behavior is hard-coded in TrayIconService
4. **Context menu remains unchanged:** Right-click tray menu still provides explicit "Show Details" and "Show Mini Monitor" options (lines 40-41, 54-55)

### Build Verification

✅ **Build status:** Success (0 errors, 0 warnings)
- Command: `dotnet build ElBruno.OllamaMonitor.sln`
- Duration: 4.5s
- Output: Both projects compiled successfully

### Manual Verification Instructions

To verify the change:
1. Launch the application (tray icon appears in system tray)
2. **Double-click** the tray icon
3. **Expected result:** MiniMonitorWindow opens (compact view with model list and metrics)
4. **Previous behavior:** MainWindow opened (detailed view)
5. Right-click context menu still provides explicit access to both windows

### Architecture Notes

- TrayIconService maintains references to **both** windows (MainWindow and MiniMonitorWindow)
- The double-click is a shortcut action; explicit window toggles remain available in the context menu
- This aligns with the Phase 2 focus on Mini Monitor as the primary user-facing view
