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

## Phase 2 Sprint Planning

### Decision: Sprint Backlog Triage & Prioritization
**Author:** Neo (Lead Architect)
**Date:** 2026-04-25
**Status:** Approved

Comprehensive 5-tier sprint breakdown completed:
- **TIER 1 (Critical):** Squad state merge (T1.1), docs cleanup (T1.2)
- **TIER 2 (UI/UX):** Mini monitor improvements (T2.1-T2.5), details window, themes, tray menu (T2.6-T2.8)
- **TIER 3 (CI):** GitHub Actions optimization for version tags (T3.1-T3.3)
- **TIER 4 (QA):** Parity benchmark design & execution (T4.1-T4.2)
- **TIER 5 (Optional):** Team expansion as needed

Estimated scope: ~40-50 story points across 5 weeks.

Open questions resolved:
- **CI Strategy:** Approved Option A (NuGet/build caching, 15-20% minute savings, low risk)
- **Mini Monitor Height:** Approved 310px increase to accommodate sparklines
- **Theme Support:** Approved as Phase 2b addition (light/dark/system)

---

### Decision: CI Optimization Recommendation
**Author:** Tank (Platform Developer)
**Date:** 2026-04-25
**Status:** Approved

GitHub Actions workflow optimization for version tags:
- Current cost: ~11-12 billing minutes per release (windows-latest)
- **Approved approach:** Option A — Aggressive NuGet & build output caching
- Expected savings: 1-2 minutes per release (15-20% reduction)
- Risk assessment: Low (standard practice, minimal cache invalidation)
- Implementation effort: 15 minutes
- Next: Implement after TIER 1 completion

---

### Decision: Squad State Consolidation
**Author:** Tank (Platform Developer)
**Date:** 2026-04-25
**Status:** Approved

All squad state successfully consolidated to `feature/sprint-improvements`:
- `.squad/decisions/` merged
- `.squad/agents/*/history.md` updated with learnings
- `.gitattributes` updated with `merge=union` drivers for append-only files
- PII/credentials verification: **CLEAN**
- Multi-machine development ready for main branch sync

---

### Decision: QA Parity Benchmark Framework
**Author:** Switch (Quality Engineer)
**Date:** 2026-04-25
**Status:** Ready for Execution

Comprehensive benchmark plan designed covering 13 realistic test scenarios:
- Normal workflows: monitor status, pull metrics, view resources
- Edge cases: no Ollama, GPU missing, network lag, rapid state changes
- Configuration changes: settings update, theme switching

Measurement thresholds:
- CPU: ±2%
- Model count: ±1%
- Response times: <5s

Recommendation: Execute after Trinity completes Phase 2b improvements (3-5 day delay) for before/after delta capture. Fallback: run baseline immediately if schedule slips.

---

### Decision: Documentation Audit & Corrections
**Author:** Morpheus (Documentation Lead)
**Date:** 2026-04-25
**Status:** Completed

Audit findings:
- No MCP tool overstatement detected (false positive)
- No "wake-up" feature references (false positive)
- **Real issue found & corrected:** `docs/configuration.md` listed Phase 2+ CLI commands (`high-cpu-threshold`, `high-memory-threshold`, `high-gpu-threshold`) as Phase 1 features
- Correction: Removed non-existent commands, clarified Phase 1 CLI supports only endpoint and refresh-interval

Impact: Low (internal docs, no customer-facing impact from correction)

---

### Decision: Mini Monitor UI Research & Design
**Author:** Trinity (Desktop Developer)
**Date:** 2026-04-25
**Status:** Design Complete, Implementation Ready

Comprehensive research on mini monitor improvements:
- **In-use model indicator:** Data available via `OllamaModelSnapshot.ExpiresAt` property (null/future = active). Implement green bullet styling, no backend changes needed.
- **Usage charts:** Recommend Canvas-based sparklines (no external dependencies, keeps Phase 1 simplicity intact). 30-sample rolling window, lightweight rendering.
- **App icon integration:** Reuse existing tray icon assets (16×16 px), simple XAML placement
- **Always-on-top relocation:** Remove header TextBlock, add to footer status bar (frees ~25px vertical space)
- **Font sizing:** Increase model labels 12px → 14px
- **Details window polish:** Match dark theme, improved spacing/padding, add app icon

Implementation sequence:
- **Phase 2a (6 hrs, quick wins):** Move always-on-top, increase fonts, add icon, implement in-use indicator
- **Phase 2b (10 hrs, advanced):** Build history buffer, Canvas sparklines, polish, test rendering performance

No blockers. No Tank dependencies. No Switch approval needed (no external dependencies).

---

### Decision: Tray Double-Click Opens Mini Monitor
**Author:** Trinity (Desktop Developer)
**Date:** 2026-04-26
**Status:** Implemented

Updated `TrayIconService.cs` line 50 to call `ShowMiniMonitorWindow()` instead of `ShowWindow()` on tray icon double-click events. Mini Monitor is now the default quick-access window from the tray; context menu remains unchanged with explicit access to both windows. Rationale: user preference + Phase 2 focus on Mini Monitor as primary interface. Build verified: ✅ Success.

---

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
