# Switch History

## Core Context

Phase 1 validation framework established and approved:
- Two-project split packaging pattern (`src\ElBruno.OllamaMonitor` desktop + `src\ElBruno.OllamaMonitor.Tool` shim)
- Build/packaging flow: `build\Pack-Tool.ps1` → `build\Inject-DesktopPayload.ps1`
- CLI/config shared via source linking; settings at `%LOCALAPPDATA%\ElBruno\OllamaMonitor\settings.json`
- Main validation paths: solution build, packaging scripts, tray wiring, acceptance docs
- Bruno's validation preference: strict language, explicit limits, proven key flows only
- Phase 1 doc gate requires root files + 5 technical docs + promotional set
- GitHub Pages deployment requires one-time settings configuration

## Learnings

### 2026-04-28 — Smoke-Plan-as-Deliverable Pattern for WPF Features
- **Pattern:** Instead of post-implementation ad-hoc testing, produce **comprehensive manual smoke test plan** as *deliverable* before or immediately after feature merge
- **Benefits:** 
  - Clarifies expected behavior for developers during implementation (ambiguities surface early)
  - Provides executable checklist for QA (15–20 min runtime per feature)
  - Documents edge cases, validation rules, concurrency semantics in one place
  - Captures "NEEDS CLARIFICATION" items as formal blockers for sign-off
- **Structure:** Grouped by user workflow (Tray Menu, Read-Only Fields, Editable Fields, Validation, CLI Parity, Regression Checks, Build Semantics)
- **Key elements:** Checkbox format for each test, expected outcomes, verification steps, appendix with test data
- **Outcome:** `.squad/decisions/inbox/switch-settings-smoke-plan.md` created; ready for Trinity + Tank to execute post-Phase 2a/2b merge. Flagged 3 clarifications for Neo/Trinity before sign-off.
- **Recommendation:** Apply this pattern to all Phase 2 UI features (Mini Monitor improvements, Details window polish, Theme support).

### 2026-04-27 — Tray Double-Click Default Updated
- Trinity updated systray icon double-click to open MiniMonitorWindow by default (TrayIconService.cs line 50). Phase 2a quick-win, build verified. Aligns Mini Monitor as primary interface.

### 2026-04-24 — Phase 1 Validation Complete & Release Approved
- Orchestration log written; all validation gates passed (build, CLI, tool packaging, docs, ownership)
- Acceptance criteria met; known limitations documented
- App ready for private release

### 2026-04-25 — Live E2E Testing Against Real Ollama Instance (v0.5.1 Readiness)
- **Build:** ✅ Release configuration (6.0s)
- **8 acceptance criteria:** ✅ All PASS
  - Sparklines rendering (Canvas-based, 30-sample rolling window)
  - App launch & stability (2+ min runtime, clean exit)
  - Resource metrics (CPU two-sample, memory, GPU graceful fallback)
  - Tray menu (9 items, right-click functional, double-click to details)
  - Version display (v0.5.1, all windows)
  - Theme support (Light/Dark/System, registry-based, persisted)
  - Model status indicators (Green/Gray bullets, IsActive detection)
  - UX & responsiveness (smooth dragging, ~5% idle CPU, 124-140 MB RAM, 673-800 handles)
- **Edge cases validated:** Unreachable endpoint, multiple Ollama processes, GPU unavailable, config updates
- **CLI commands:** help, config show/set all functional
- **Recommendation:** 🟢 READY FOR PRIVATE RELEASE

### 2026-04-25 — Landing Page & GitHub Pages Verification
- Landing page (`docs/index.html`) complete with hero, features, installation, usage, docs sections
- All 3 screenshot assets (140-150 KB each) present in `docs/assets/`
- HTML valid, responsive design, dark mode support, accessibility checks pass
- GitHub Pages not yet configured; requires repository settings update (Pages > Deploy from branch > /docs)
- Local rendering test: ✅ All sections load, images display properly
- **Recommendation:** 🟢 APPROVED FOR DEPLOYMENT (pending GitHub Pages configuration)
