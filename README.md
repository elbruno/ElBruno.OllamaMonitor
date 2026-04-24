# ElBruno.OllamaMonitor

[![NuGet](https://img.shields.io/nuget/v/ElBruno.OllamaMonitor.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/ElBruno.OllamaMonitor)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ElBruno.OllamaMonitor.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/ElBruno.OllamaMonitor)
[![Publish to NuGet](https://github.com/elbruno/ElBruno.OllamaMonitor/actions/workflows/publish.yml/badge.svg)](https://github.com/elbruno/ElBruno.OllamaMonitor/actions/workflows/publish.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)

A tiny Windows system tray tool to monitor your local Ollama runtime.

> Quick visual feedback about your Ollama status, resource usage, and models—right from your Windows system tray.

## What It Does

ElBruno.OllamaMonitor sits in your Windows system tray and tells you:

- **Is Ollama running?** A glance at the tray icon shows you the status.
- **Is a model loaded?** See what's currently active.
- **How much CPU, RAM, and GPU is it using?** Real-time resource metrics from the Ollama process.
- **Any errors?** Get instant visual feedback if something's wrong.

Perfect for:
- Local AI developers who need quick visibility into Ollama
- Demo presenters who want to know resource impact in real-time
- Anyone running large models locally who's curious about the overhead

## Installation

### Via NuGet (Recommended)

```bash
dotnet tool install --global ElBruno.OllamaMonitor
```

Then launch anytime:

```bash
ollamamon
```

### From Source

```bash
git clone https://github.com/elbruno/ElBruno.OllamaMonitor.git
cd ElBruno.OllamaMonitor
dotnet build src/ElBruno.OllamaMonitor/
dotnet run --project src/ElBruno.OllamaMonitor/
```

## Quick Start

### Launch the App

```bash
ollamamon
```

The app starts minimized to the tray by default. Use the tray icon to open either the full details window or the always-on-top mini monitor.

### View Current Configuration

```bash
ollamamon config
```

### Change the Ollama Endpoint

If you're running Ollama on a different machine or port:

```bash
ollamamon config set endpoint http://192.168.1.100:11434
```

### Adjust Refresh Interval

Control how often the app polls Ollama (in seconds, default is 2):

```bash
ollamamon config set refresh-interval 5
```

### Reset to Defaults

```bash
ollamamon config reset
```

### Get Help

```bash
ollamamon --help
```

## System Tray Status

The tray icon color tells you the status at a glance:

| Color  | Meaning |
|--------|---------|
| 🟤 Gray  | Ollama is not reachable |
| 🟢 Green | Ollama is running, no model loaded |
| 🔵 Blue  | A model is currently loaded |
| 🟠 Orange | A model is running or high resource usage |
| 🔴 Red   | Error or Ollama unavailable |

Click the icon to open the full details window for diagnostics, or open the mini monitor from the tray menu to keep resource usage visible on top of other windows.

## Features

- ✅ **System Tray Integration** — Runs in the background, always visible
- ✅ **Visual Status Indicators** — Color-coded icons for quick status checks
- ✅ **Standard Details Window** — A normal Windows window with minimize/close behavior that keeps the app in the tray when closed
- ✅ **Mini Monitor Window** — A semi-transparent always-on-top compact view for CPU, RAM, GPU, and model status
- ✅ **Local Configuration** — Customize endpoint, refresh rate, thresholds
- ✅ **CLI Commands** — Fully scriptable configuration
- ✅ **GPU Metrics** — Best-effort NVIDIA GPU tracking (if nvidia-smi is available)
- ✅ **Copy to Clipboard** — Quickly share diagnostics
- ✅ **Manual Refresh** — Force an immediate check
- ✅ **Open Ollama URL** — Quick link to the Ollama API

## Requirements

- **Windows 10 / Windows 11** (requires .NET 10 runtime, which can be downloaded from [dotnet.microsoft.com](https://dotnet.microsoft.com))
- **Ollama** running locally (download from [ollama.ai](https://ollama.ai))
- **.NET 10 SDK** to build from source

Optional:
- **nvidia-smi** (NVIDIA GPU drivers) for GPU metrics

## Configuration

Configuration is stored at:

```
%LOCALAPPDATA%\ElBruno\OllamaMonitor\settings.json
```

Default values:

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

You can edit this file directly or use the CLI commands.

## Documentation

- **[Architecture Guide](docs/architecture.md)** — How the app is built and organized
- **[Configuration Guide](docs/configuration.md)** — Detailed configuration options and defaults
- **[Development Guide](docs/development-guide.md)** — Building from source, folder structure, debugging
- **[Publishing Guide](docs/publishing.md)** — NuGet publishing with GitHub Releases and OIDC
- **[Troubleshooting](docs/troubleshooting.md)** — Common issues and solutions
- **[Release Notes](docs/release-notes.md)** — Version history and changelog

## Promotional Content

- **[Blog Post](docs/promotional/blog-post.md)** — Full-length article about the tool
- **[LinkedIn Post](docs/promotional/linkedin-post.md)** — Social media ready
- **[Twitter Post](docs/promotional/twitter-post.md)** — X-ready snippet
- **[Image Prompts](docs/promotional/image-prompts.md)** — AI image generation prompts for graphics

## License

This project is licensed under the MIT License — see [LICENSE](LICENSE) for details.

## Support

Found a bug or have a feature request? Open an issue on GitHub.

Questions about Ollama? Check the [Ollama documentation](https://github.com/ollama/ollama).

## About the Author

**Made with ❤️ by [Bruno Capuano (ElBruno)](https://github.com/elbruno)**

- 📝 **Blog**: [elbruno.com](https://elbruno.com)
- 📺 **YouTube**: [youtube.com/elbruno](https://youtube.com/elbruno)
- 🔗 **LinkedIn**: [linkedin.com/in/elbruno](https://linkedin.com/in/elbruno)
- 𝕏 **Twitter**: [twitter.com/elbruno](https://twitter.com/elbruno)
- 🎙️ **Podcast**: [notienenombre.com](https://notienenombre.com)
