# Release Notes

## Version History

### 0.5.0 — NuGet Packaging and Release Automation

**Release Date:** 2026-04-24

This release prepares ElBruno.OllamaMonitor for repeatable NuGet publishing through GitHub Releases.

#### What's New

- ✅ Added NuGet-ready package metadata, including repository information and package icon support
- ✅ Added a GitHub Actions `publish.yml` workflow based on the ElBruno.LocalLLMs release process
- ✅ Added publishing documentation for OIDC Trusted Publishing with NuGet.org
- ✅ Added NuGet version and download badges to the README
- ✅ Updated the README author section to match the ElBruno.LocalLLMs style
- ✅ Added package branding assets for the NuGet icon workflow

#### Notes

- The release workflow is triggered by a published GitHub Release such as `v0.5.0`
- Successful NuGet publishing still requires the repository to be configured in NuGet Trusted Publishing
- The application feature set remains the Phase 1 tray-monitor experience introduced earlier

---

### 0.1.0 (Phase 1) — Initial Release

**Release Date:** 2026-04-24

ElBruno.OllamaMonitor Phase 1 is the foundation: a lightweight Windows system tray app to monitor your local Ollama instance.

#### What's Included

**Core Features:**
- ✅ **System Tray Integration** — Minimalist monitoring from the notification area
- ✅ **Status Icon** — Color-coded states (Gray, Green, Blue, Orange, Red) reflect Ollama status at a glance
- ✅ **Floating Details Window** — Click the tray icon to see full diagnostics
- ✅ **Real-Time Metrics** — CPU, memory, disk I/O, and best-effort GPU tracking
- ✅ **Ollama API Integration** — Version detection, model list, running process info
- ✅ **Local Configuration** — JSON settings file with sensible defaults
- ✅ **CLI Commands** — Fully scriptable configuration and help
- ✅ **Global Tool** — Install once via `dotnet tool install --global ElBruno.OllamaMonitor`

**CLI Commands:**
```bash
ollamamon                                    # Launch tray app
ollamamon --help                            # Show help
ollamamon config                            # Display current settings
ollamamon config set endpoint <url>         # Change Ollama endpoint
ollamamon config set refresh-interval <sec> # Change polling interval
ollamamon config reset                      # Reset to defaults
```

**Tray Menu:**
- **Show** — Display the floating details window
- **Refresh** — Force immediate status check
- **Copy Diagnostics** — Copy current status to clipboard
- **Open Ollama URL** — Launch default browser to Ollama API
- **Exit** — Close the app

**Configuration Options:**
```json
{
  "endpoint": "http://localhost:11434",
  "refreshIntervalSeconds": 2,
  "startMinimizedToTray": true,
  "showFloatingWindowOnStart": false,
  "enableGpuMetrics": true,
  "enableDiskMetrics": true,
  "highCpuThresholdPercent": 80,
  "highMemoryThresholdGb": 16,
  "highGpuThresholdPercent": 85
}
```

**System Requirements:**
- Windows 10 / Windows 11
- .NET 10 runtime (automatically installed with `dotnet tool install`)
- Ollama running locally or accessible via network

**GPU Support:**
- Best-effort NVIDIA GPU metrics via `nvidia-smi`
- Falls back gracefully to "N/A" if GPU unavailable
- No support for AMD/Intel GPUs in Phase 1

**Documentation:**
- README — Quick start and overview
- Architecture Guide — Technical structure and design
- Configuration Guide — Detailed settings reference
- Development Guide — Building from source
- Troubleshooting — Common issues and fixes
- Release Notes — Version history (this file)

#### Known Limitations

- **Windows Only** — No macOS or Linux support in Phase 1
- **Single Ollama Instance** — Monitors one Ollama endpoint at a time
- **No Historical Data** — No charts or time-series tracking
- **No Settings UI** — Configuration via CLI or direct JSON edit
- **No Auto-Update** — Manual reinstall required for updates
- **No Windows Service** — App must be launched by user

#### What's Coming in Phase 2+

- MVVM Framework — Better code structure and testability
- Unit Tests — Comprehensive test coverage
- Logging Framework — Enterprise-grade logging
- Historical Charts — Track metrics over time
- Settings Dialog — GUI configuration (no CLI needed)
- Multi-Instance Support — Monitor multiple Ollama servers
- Remote Notifications — Alert on threshold breaches
- Dark Mode — Alternative UI theme
- macOS/Linux Support — Cross-platform availability

#### Breaking Changes

None — this is the initial release.

#### Migration Guide

N/A — fresh installation.

#### Installation & Upgrade

**First Time Installation:**

```bash
dotnet tool install --global ElBruno.OllamaMonitor
```

**Upgrade from Previous Version:**

```bash
dotnet tool update --global ElBruno.OllamaMonitor
```

**Downgrade (if needed):**

```bash
dotnet tool uninstall --global ElBruno.OllamaMonitor
dotnet tool install --global ElBruno.OllamaMonitor --version 0.1.0
```

#### Troubleshooting

See [Troubleshooting Guide](troubleshooting.md) for common issues and solutions.

#### Contributing & Feedback

- **GitHub Issues:** Report bugs or request features
- **GitHub Discussions:** Ask questions or share ideas
- **Pull Requests:** Contributions welcome

#### Credits

**Project Lead:** Bruno Capuano (El Bruno)

**Architecture & Design:** Neo (Lead Architect)

**Platform Development:** Tank  

**Desktop UI:** Trinity  

**Documentation:** Morpheus

---

## Versioning

This project follows [Semantic Versioning](https://semver.org/):

- **MAJOR** — Breaking changes to CLI or configuration
- **MINOR** — New features (backward compatible)
- **PATCH** — Bug fixes (backward compatible)

**Example:** 0.1.0 = Major 0 (pre-1.0), Minor 1 (Phase 1), Patch 0 (first release)

---

## Support Timeline

| Version | Release | Status | End of Support |
|---------|---------|--------|-----------------|
| 0.5.0   | 2026-04-24 | Active | TBD |
| 0.1.0   | 2026-04-24 | Shipped | TBD |
| 0.2.0   | TBD | Planned | — |
| 1.0.0   | TBD    | Planned | — |

---

## License & Attribution

See [LICENSE](../LICENSE) for terms.

Built by the El Bruno team.

---

**Next Steps:**
- **Installation:** [README](../README.md)
- **Configuration:** [Configuration Guide](configuration.md)
- **Development:** [Development Guide](development-guide.md)
- **Issues?** [Troubleshooting Guide](troubleshooting.md)
