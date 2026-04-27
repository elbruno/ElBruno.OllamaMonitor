# Neo — History

## Learnings

### 2026-04-27 — Tray Double-Click Default Updated
- Trinity updated systray icon double-click to open MiniMonitorWindow by default (TrayIconService.cs line 50). Phase 2a quick-win, build verified. Aligns Mini Monitor as primary interface.

### 2026-04-24 — Phase 1 Architecture Contract

- **Environment:** .NET 10.0.203 stable SDK available. Use `net10.0-windows` TFM.
- **PRD location:** `docs/plans/ElBruno.OllamaMonitor.PRD.md` (~1000 lines, 4 phases, only Phase 1 in scope).
- **Repo rules:** All source in `src/`, all docs in `docs/`, root holds only README.md, LICENSE, .sln, .gitignore, .gitattributes.
- **Key packages:** H.NotifyIcon.Wpf for tray, System.Text.Json for Ollama API, no DI container Phase 1.
- **Ownership split:**
  - Tank: Program.cs, Commands/, Ollama/, Metrics/, Config/, Diagnostics/, .csproj
  - Trinity: App.xaml, MainWindow.xaml, Tray/
  - Morpheus: README.md, LICENSE, docs/**
  - Switch: build/pack verification, CLI smoke tests
- **High-risk:** WinExe+PackAsTool console output, H.NotifyIcon.Wpf net10 compat, CPU% two-sample, nvidia-smi spawn cost.
- **Simplifications:** No DI container, no MVVM framework, no unit tests, no logging framework, no settings dialog—all Phase 2+.
- **Config path:** `%LOCALAPPDATA%\ElBruno\OllamaMonitor\settings.json`
- **Decision file:** `.squad/decisions/inbox/neo-phase1-contract.md`

### 2026-04-24 — Phase 1 Complete & Approved

- **Status:** All orchestration logs written, decisions merged, validation complete.
- **Team outcomes:** Tank delivered platform, Trinity delivered WPF integration, Morpheus delivered all documentation, Switch approved for private release.
- **Next: Phase 2** — DI container, MVVM framework, unit tests, logging framework, remote monitoring.

### 2025-01-24 — Sparkline Persistence Fix

- **Problem:** Mini monitor window sparklines invisible because model metrics History was lost on each refresh cycle.
- **Root cause:** `MainWindowViewModel.ApplySnapshot()` cleared and rebuilt the Models collection with fresh `OllamaModelSnapshot` instances every 2 seconds, destroying the `History` objects that accumulate metrics over time.
- **Solution:** Implemented persistent model cache (`Dictionary<string, OllamaModelSnapshot>`) in MainWindowViewModel:
  - Added `_modelCache` field to store models by name across refresh cycles
  - Created `GetOrUpdateModel()` helper that reuses existing cached models (preserving History) or creates new ones
  - Refactored `ApplySnapshot()` to populate Models collection from cache instead of creating fresh instances
  - Added automatic cleanup of stale models no longer in API response
- **Key insight:** C# record `with` syntax preserves object references for properties not explicitly overridden, so updating cached models with `existingModel with { ExpiresAt = ... }` keeps the same History instance alive.
- **Verification:** Model loaded with `ollama run llama3.2:latest`, metrics accumulate across 2-second refresh cycles, sparklines render with visible data traces after 10-15 seconds.
- **Files modified:** `src/ElBruno.OllamaMonitor/ViewModels/MainWindowViewModel.cs`

