# Tank History

## Learnings

- Phase 1 uses a two-project packaging split: `src\ElBruno.OllamaMonitor` is the Windows-first .NET 10 WPF desktop app, and `src\ElBruno.OllamaMonitor.Tool` is the `net10.0` global tool shim that owns `ollamamon`.
- .NET SDK tool packaging does not support `UseWPF`, `UseWindowsForms`, or `-windows` TFMs directly, so the tool package is built first and then enriched with the published desktop payload via `build\Pack-Tool.ps1` and `build\Inject-DesktopPayload.ps1`.
- Shared CLI/config source files are linked into the tool project from the desktop project (`AppPaths`, `Cli`, `Configuration`, `Diagnostics`, `Interop`) to keep config behavior aligned across tray and CLI entry points.
- Core Phase 1 service boundaries live under `src\ElBruno.OllamaMonitor\Ollama`, `...\Services`, and `...\Models`; the tray/UI layer is intentionally isolated in `App.xaml.cs`, `TrayIconService`, `MainWindow`, and `ViewModels`.
- GPU metrics are best-effort only: `NvidiaSmiMetricsService` returns friendly unavailable states instead of failing when `nvidia-smi` is missing or unparsable.
- Bruno prefers Windows-first runtime behavior, safe degradation when Ollama/GPU data is unavailable, and explicit configuration commands that keep `%LOCALAPPDATA%\ElBruno\OllamaMonitor\settings.json` as the single source of truth.

### 2026-04-24 — Phase 1 Implementation Complete

- **Orchestration log:** Written to `.squad/orchestration-log/2026-04-24T18-11-14Z-tank.md`
- **Team integration:** Trinity wired WPF, Morpheus documented config paths and design, Switch validated all flows
- **Validation:** Build, packaging, CLI smoke tests all passed
- **Status:** Ready for private release

