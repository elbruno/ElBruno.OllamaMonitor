# Neo — History

## Learnings

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

