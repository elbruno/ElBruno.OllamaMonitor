# Switch History

## Learnings

- Phase 1 ships as a two-project split: `src\ElBruno.OllamaMonitor` is the Windows desktop payload, and `src\ElBruno.OllamaMonitor.Tool` is the `ollamamon` global-tool shim.
- The supported packaging flow is `build\Pack-Tool.ps1`, which publishes the desktop app, packs the tool, then injects the desktop payload into the `.nupkg` with `build\Inject-DesktopPayload.ps1`.
- CLI/config behavior is shared between desktop and tool via linked source files under `src\ElBruno.OllamaMonitor\Cli`, `...\Configuration`, `...\Diagnostics`, and `AppPaths.cs`.
- Runtime settings and logs live under `%LOCALAPPDATA%\ElBruno\OllamaMonitor\settings.json` and `%LOCALAPPDATA%\ElBruno\OllamaMonitor\logs\`.
- The main validation paths for Phase 1 are: solution build (`ElBruno.OllamaMonitor.sln`), packaging scripts (`build\Pack-Tool.ps1`, `build\Inject-DesktopPayload.ps1`), tool entry point (`src\ElBruno.OllamaMonitor.Tool\Program.cs`), tray wiring (`src\ElBruno.OllamaMonitor\Services\TrayIconService.cs`), and acceptance docs in `docs\plans\ElBruno.OllamaMonitor.PRD.md`.
- Bruno prefers strict validation language: approve only when key flows are proven, and call out environment-driven limits explicitly instead of overstating UI verification.
- Final Phase 1 doc gate expects the root files (`README.md`, `LICENSE`) plus `docs\architecture.md`, `docs\configuration.md`, `docs\development-guide.md`, `docs\troubleshooting.md`, `docs\release-notes.md`, and the promotional set under `docs\promotional\`.
- A headless unreachable-Ollama smoke check is viable by installing the packaged tool, setting `endpoint` to `http://127.0.0.1:65535`, launching `ollamamon` with no args, and confirming `ElBruno.OllamaMonitor.exe` stays alive for a short window before cleanup.

### 2026-04-24 — Phase 1 Validation Complete & Release Approved

- **Orchestration log:** Written to `.squad/orchestration-log/2026-04-24T18-11-14Z-switch.md`
- **All validation gates passed:**
  - ✅ Solution builds
  - ✅ CLI commands functional (help, config show/set/reset)
  - ✅ Tool packages correctly
  - ✅ Documentation complete (5 technical + root + promotional)
  - ✅ No Phase 2+ inventions
  - ✅ Ownership boundaries clear
- **Acceptance criteria met:** All checks passed. Ready for private release.
- **Known limitations documented:** Tray UI (manual only), GPU best-effort, no remote Phase 1, no unit tests Phase 1
