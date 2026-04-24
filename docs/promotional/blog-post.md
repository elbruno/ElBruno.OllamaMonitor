# Blog Post: ElBruno.OllamaMonitor — Monitor Your Local Ollama Runtime from the Windows Tray

## Subtitle
*A tiny, practical tool that gives you instant visibility into your Ollama workloads without cluttering your desktop.*

---

## Introduction

If you're running Ollama locally on Windows—whether you're tinkering with LLMs, building local AI demos, or just curious about the overhead of large models—you've probably wondered: *Is it still running? How much CPU is it chewing? Did that model load?*

Welcome to **ElBruno.OllamaMonitor**.

It's a no-frills system tray app that sits in your Windows notification area and tells you, at a glance, exactly what your local Ollama instance is doing. No dashboards. No complexity. Just real-time status, resource metrics, and a floating details window when you need more info.

## Why Another Monitoring Tool?

You could open Task Manager. You could open a terminal and `curl` the Ollama API. You could build a web dashboard.

But you probably don't want to. You want something that Just Works™—that launches with your system, stays out of the way, and gives you instant feedback without thinking about it.

That's the philosophy behind this tool:

- **Minimal footprint** — Sits in the tray, takes up one icon
- **Zero configuration** — Works out of the box with your local Ollama
- **Visual feedback** — Color-coded status at a glance (Gray, Green, Blue, Orange, Red)
- **Copy diagnostics** — Right-click the icon, copy full status to clipboard
- **Scriptable** — Full CLI support for automation and CI/CD

## What It Does

When you run `ollamamon`, you get:

1. **A tray icon** that changes color based on Ollama state:
   - 🟤 **Gray** = Ollama unreachable
   - 🟢 **Green** = Running, no model loaded
   - 🔵 **Blue** = Model loaded, light usage
   - 🟠 **Orange** = Model running, high resource usage
   - 🔴 **Red** = Error

2. **A floating details window** (click the icon) showing:
   - Ollama version and status
   - Currently loaded/running models
   - CPU, memory, and disk I/O
   - GPU utilization (if NVIDIA GPU available)
   - Thresholds and diagnostics

3. **A context menu** with options to:
   - Show the details window
   - Manually refresh the status
   - Copy diagnostics to clipboard
   - Open the Ollama API URL in your browser
   - Exit cleanly

4. **CLI commands** for automation:
   ```bash
   ollamamon                                    # Launch app
   ollamamon --help                            # Show help
   ollamamon config                            # View settings
   ollamamon config set endpoint <url>         # Change endpoint
   ollamamon config set refresh-interval <sec> # Change polling rate
   ollamamon config reset                      # Reset defaults
   ```

## Use Cases

### Local AI Development

You're prototyping a .NET app that uses local embeddings or inference. You need to know:
- Is Ollama still running?
- Is the model loaded?
- How much VRAM is the embedding model using?

Answer: Glance at the tray. Done.

### Demo Presentations

You're live-demoing a local AI feature. Suddenly, the inference slows down. Your audience wonders: *Is it CPU-bound? GPU-saturated? Out of memory?*

Answer: Click the tray icon, show the floating window with real-time metrics, explain what's happening. Credibility earned.

### Resource Awareness

You're running Ollama in the background while you work. Occasionally, your system gets sluggish. Is it Ollama? Is the model loading? Is it GPU-bound?

Answer: Check the tray. If it's orange, you know Ollama is hogging resources. You can decide whether to wait or pause it.

### CI/CD & Automation

You're running tests that depend on a local Ollama instance. You want to verify Ollama is healthy before running tests.

Answer: Use the CLI:
```bash
ollamamon config  # Returns current config
curl http://localhost:11434/api/version  # Via the app, you know the endpoint
```

## Installation

### Via NuGet (Recommended)

```bash
dotnet tool install --global ElBruno.OllamaMonitor
```

That's it. The `ollamamon` command is now in your PATH.

### From Source

Clone the repository, build, and run:

```bash
git clone https://github.com/ElBruno/ElBruno.OllamaMonitor.git
cd ElBruno.OllamaMonitor
dotnet build
dotnet run --project src/ElBruno.OllamaMonitor/
```

## Configuration

All settings are stored in a JSON file:

```
%LOCALAPPDATA%\ElBruno\OllamaMonitor\settings.json
```

Default values are sensible for most use cases:

```json
{
  "endpoint": "http://localhost:11434",
  "refreshIntervalSeconds": 2,
  "startMinimizedToTray": true,
  "enableGpuMetrics": true,
  "highCpuThresholdPercent": 80,
  "highMemoryThresholdGb": 16,
  "highGpuThresholdPercent": 85
}
```

Need to monitor a remote Ollama instance? Just set the endpoint:

```bash
ollamamon config set endpoint http://192.168.1.100:11434
```

## Technical Highlights

- **Built on .NET 10** — Latest runtime, Windows-native, fast
- **WPF UI** — Proper Windows integration, not a web wrapper
- **System tray** — Uses `H.NotifyIcon.Wpf` for clean tray integration
- **No external dependencies** — Minimal install footprint
- **Best-effort GPU metrics** — Detects and polls `nvidia-smi` if available, gracefully degrades otherwise
- **Structured diagnostics** — Event logging to help debug issues

## What's in Phase 1?

This is the foundational release. You get:

- ✅ System tray status icon (5 states)
- ✅ Floating details window
- ✅ CPU, RAM, disk metrics
- ✅ Best-effort GPU metrics
- ✅ CLI commands
- ✅ Local configuration
- ✅ Copy-to-clipboard diagnostics
- ✅ Comprehensive documentation

## What's Coming in Phase 2+?

Future roadmap includes:

- Historical metrics & charts
- MVVM architecture for better testability
- Unit test suite
- Settings UI dialog
- Multi-instance support (monitor multiple Ollama servers)
- Dark mode
- macOS/Linux support
- Advanced alerting & notifications

## Who Should Use This?

- **Local AI developers** — Anyone running Ollama on Windows
- **.NET developers** — If you're in the .NET ecosystem, this fits naturally
- **Demo presenters** — Anyone who needs to show Ollama status live
- **Productivity enthusiasts** — If you like lightweight, single-purpose tools

## Getting Started

1. **Install:**
   ```bash
   dotnet tool install --global ElBruno.OllamaMonitor
   ```

2. **Ensure Ollama is running:**
   ```bash
   ollama serve
   ```

3. **Launch the app:**
   ```bash
   ollamamon
   ```

4. **Look at the system tray** for the status icon. Click it to see details.

5. **Customize if needed:**
   ```bash
   ollamamon config set refresh-interval 5  # Slower polling
   ollamamon config set endpoint http://192.168.1.50:11434  # Remote Ollama
   ```

## Documentation

Comprehensive docs are in the repository:

- **README** — Quick start and overview
- **Architecture Guide** — How the app works
- **Configuration Guide** — All settings explained
- **Development Guide** — Build from source
- **Troubleshooting** — Common issues and solutions

## Feedback & Contributions

Found a bug? Have an idea? Open an issue or PR on GitHub.

The codebase is clean, documented, and ready for contributions. Phase 1 is intentionally minimal—Phase 2 is where we add the fancy stuff.

## Conclusion

ElBruno.OllamaMonitor is proof that sometimes the best tools are the simplest ones. No bloat. No complexity. Just a tray icon that tells you what's going on.

Whether you're a local AI enthusiast, a .NET developer, or just someone who runs Ollama and wonders about resource usage, give it a try. It's free, open-source, and takes 30 seconds to install.

Your productivity will thank you.

---

**Try it:** `dotnet tool install --global ElBruno.OllamaMonitor`

**Docs:** See the [GitHub repository](https://github.com/ElBruno/ElBruno.OllamaMonitor)

**Made by:** [El Bruno](https://elbruno.com) — A .NET developer obsessed with local AI and productivity.

---

## Meta

- **Reading time:** ~8 minutes
- **Target audience:** Local AI developers, .NET developers, Windows users
- **Tone:** Practical, slightly irreverent, developer-focused
- **Keywords:** Ollama, local AI, Windows, system tray, monitoring, .NET
