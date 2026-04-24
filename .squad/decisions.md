# Squad Decisions

## Phase 1 Architecture

### Decision: Phase 1 Implementation Contract

**Author:** Neo (Lead Architect)  
**Date:** 2026-04-24  
**Status:** Approved

- **.NET SDK:** 10.0.203 stable (`net10.0-windows` TFM)
- **Project layout:** All source in `src/`, all docs in `docs/`, root holds README + LICENSE + .sln
- **Ownership:** Tank (CLI/services/config), Trinity (WPF/tray), Morpheus (docs), Switch (validation)
- **Target framework & output:** WinExe, PackAsTool, ToolCommandName = "ollamamon"
- **CLI routing:** Simple string matching in Program.cs (no third-party CLI framework)
- **Ollama client:** System.Net.Http (singleton), System.Text.Json deserialization
- **Process metrics:** System.Diagnostics.Process (CPU two-sample, memory, disk I/O)
- **GPU metrics:** Best-effort nvidia-smi (graceful degradation if missing/error)
- **Tray icon colors:** 5-color model (Gray/Green/Blue/Orange/Red)
- **Configuration:** `%LOCALAPPDATA%\ElBruno\OllamaMonitor\settings.json`, default refresh 2s
- **Simplifications Phase 1:** No DI container, no MVVM framework, no unit tests, no logging framework, no settings dialog, no icon resources (generated at runtime)

**High-risk mitigations:**
- H.NotifyIcon.Wpf net10 support: check NuGet, fallback to System.Windows.Forms interop
- WinExe + PackAsTool console output: use P/Invoke AttachConsole if needed
- CPU % two-sample: async measurement, first refresh may show 0%
- Disk I/O per-process: best-effort, mark "N/A" if unavailable
- nvidia-smi spawn: cache GPU data 5s minimum, background thread

---

### Decision: Tank Phase 1 Core Implementation

**Author:** Tank (Platform Developer)  
**Date:** 2026-04-24  
**Status:** Approved

- **Two-project split:** `src\ElBruno.OllamaMonitor` (WPF desktop, `net10.0-windows`) + `src\ElBruno.OllamaMonitor.Tool` (tool shim, `net10.0`)
- **Packaging strategy:** `build\Pack-Tool.ps1` publishes desktop, packs tool, then `build\Inject-DesktopPayload.ps1` enriches .nupkg with desktop payload
- **Shared source linking:** CLI, config, diagnostics, interop modules linked from desktop to tool project (ensures behavioral consistency)
- **Service implementations:**
  - OllamaClient (HTTP), OllamaStatusService (state aggregation)
  - ProcessMetricsService (CPU/memory), GpuMetricsService (nvidia-smi), DiskMetricsService (I/O)
  - AppSettingsService (config file), StatusFormatter (plain text), ClipboardService (copy)
- **CLI routing:** Program.cs owns --help, config (show/set/reset), default launch WPF
- **GPU metrics safety:** Returns "unavailable" state instead of failing when nvidia-smi missing
- **Configuration path:** `%LOCALAPPDATA%\ElBruno\OllamaMonitor\settings.json`

---

### Decision: Morpheus Phase 1 Documentation

**Author:** Morpheus (Documentation Lead)  
**Date:** 2026-04-24  
**Status:** Approved

- **Root documentation:** README.md (concise 5K chars) + LICENSE (placeholder, awaiting license choice)
- **Technical documentation:** architecture.md, configuration.md, development-guide.md, troubleshooting.md (15+ issues), release-notes.md
- **Promotional materials:** blog-post.md (~2500 words), 3 LinkedIn variants, 5 Twitter variants, image-prompts.md (9 detailed prompts)
- **Images:** .gitkeep placeholder (no T2I capability; images deferred)
- **Documentation philosophy:** Practical, developer-focused, Bruno/El Bruno style (friendly, slightly humorous), Phase 1 scoped only
- **Scope decisions:**
  - README concise with links (not exhaustive)
  - License placeholder pending team decision (MIT/Apache 2.0/etc.)
  - Troubleshooting prioritized by frequency/impact (not A-Z)
  - Architecture moderate depth (class names, flows, not line-by-line)
  - Configuration at `%LOCALAPPDATA%\ElBruno\OllamaMonitor\settings.json` (matches implementation)
- **Alignment:** All required docs present, Phase 2 roadmap noted, promotional variants ready for social media

---

### Decision: Switch Phase 1 Validation & Release

**Author:** Switch (Validation & Release Engineering)  
**Date:** 2026-04-24  
**Status:** Approved for Private Release

- **Validation gates all passed:**
  - Solution builds (`dotnet build`)
  - CLI commands functional (help, config show/set/reset)
  - Tool packages correctly (Pack-Tool.ps1 + Inject-DesktopPayload.ps1)
  - Documentation complete (5 technical + root + promotional)
  - No Phase 2+ inventions
  - Ownership boundaries clear
- **Smoke test coverage:**
  - Build verification
  - CLI routing verification
  - Headless tool launch (unreachable endpoint, gray icon state persists)
- **Limitations explicitly documented:**
  - Non-interactive tray verification (manual only)
  - GPU metrics best-effort (nvidia-smi dependency)
  - No remote monitoring Phase 1
  - No unit tests Phase 1
  - Ollama endpoint reachability via error handling (graceful degradation)
- **Approval criteria met:** All acceptance gates passed. Ready for internal release.

---

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
