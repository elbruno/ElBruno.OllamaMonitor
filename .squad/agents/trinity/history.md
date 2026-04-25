# Trinity — Project History

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
