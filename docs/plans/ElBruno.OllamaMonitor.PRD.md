# PRD: ElBruno.OllamaMonitor

## Product Name

**ElBruno.OllamaMonitor**

## Repository

```text
ElBruno.OllamaMonitor
```

## NuGet Package

```text
ElBruno.OllamaMonitor
```

## CLI Command

```text
ollamamon
```

## Tagline

A tiny Windows system tray tool to monitor your local Ollama runtime.

## Summary

ElBruno.OllamaMonitor is a Windows-first .NET tool that displays the current status of a local Ollama instance from the system tray.

The tool should show whether Ollama is running, whether a model is currently loaded or active, and basic resource usage such as CPU, memory, disk, and best-effort GPU usage.

The main user experience is a system tray icon with status indicators. The user can show or hide a floating details window with more information.

The CLI exists only to launch the app and manage basic configuration.

## Core Naming Decisions

```text
Repo:   ElBruno.OllamaMonitor
NuGet:  ElBruno.OllamaMonitor
CLI:    ollamamon
```

## Goals

- Provide quick visual feedback about local Ollama status.
- Show whether Ollama is reachable through the local API.
- Show whether Ollama has one or more models loaded.
- Show currently loaded/running model information when available.
- Show process-level resource usage for Ollama.
- Provide a lightweight floating window with details.
- Keep the tool simple, small, and useful for local AI developers.
- Package the tool as a .NET global tool.
- Support installation through NuGet using `dotnet tool install`.
- Generate developer documentation and promotional material as part of the repo.

## Non-Goals for Phase 1

- No full terminal dashboard.
- No `watch` command.
- No historical charts.
- No remote machine monitoring.
- No Windows Service.
- No perfect cross-vendor GPU metrics.
- No deep profiling of individual model inference requests.
- No cloud telemetry.
- No user account or login system.
- No Azure dependency.

## Target Users

- Developers running Ollama locally.
- .NET developers building local AI demos.
- Developers using GitHub Copilot CLI, local models, Foundry Local, Aspire, or agents.
- Demo presenters who need quick visibility into local model status.
- Users who want to know if Ollama is silently eating CPU, RAM, or GPU.

## Repository Rules

These rules are mandatory for the implementation.

### Folder Structure Rules

- All source code must live inside the `src` folder.
- All documentation must live inside the `docs` folder.
- The only documentation files allowed at the repository root are:
  - `README.md`
  - `LICENSE`
- No source code should be placed at the repository root.
- No planning documents should be placed at the repository root.
- Promotional material must live under `docs/promotional`.
- Image prompts must live under `docs/promotional/image-prompts`.
- Generated images, if any, must live under `docs/promotional/images`.

### Suggested Repository Structure

```text
ElBruno.OllamaMonitor/
  README.md
  LICENSE
  .gitignore
  ElBruno.OllamaMonitor.sln

  src/
    ElBruno.OllamaMonitor/
      Program.cs
      App.xaml
      App.xaml.cs
      MainWindow.xaml
      MainWindow.xaml.cs

      Commands/
        ConfigCommand.cs
        HelpCommand.cs

      Tray/
        TrayIconService.cs
        TrayStatusMapper.cs
        TrayMenuBuilder.cs

      Ollama/
        OllamaClient.cs
        OllamaStatusService.cs
        OllamaStatus.cs
        OllamaModelInfo.cs

      Metrics/
        ProcessMetricsService.cs
        GpuMetricsService.cs
        NvidiaSmiMetricsService.cs
        DiskMetricsService.cs
        ResourceSnapshot.cs

      Config/
        AppSettings.cs
        AppSettingsService.cs

      Diagnostics/
        StatusFormatter.cs
        ClipboardService.cs

  docs/
    architecture.md
    configuration.md
    development-guide.md
    release-notes.md
    troubleshooting.md

    promotional/
      blog-post.md
      linkedin-post.md
      twitter-post.md
      image-prompts.md
      images/
        .gitkeep
```

### Documentation Rules

Create and maintain these documentation files:

```text
docs/architecture.md
docs/configuration.md
docs/development-guide.md
docs/troubleshooting.md
docs/release-notes.md
```

The root `README.md` should be concise and practical. It should include:

- What the tool does.
- How to install it.
- How to run it.
- Basic configuration commands.
- Screenshots or images when available.
- Links to deeper docs in the `docs` folder.

### Promotional Material Rules

Create promotional content as part of the repo.

Promotional docs must include:

```text
docs/promotional/blog-post.md
docs/promotional/linkedin-post.md
docs/promotional/twitter-post.md
docs/promotional/image-prompts.md
```

The promotional material should follow Bruno / El Bruno style:

- Practical.
- Friendly.
- Slightly funny.
- Developer-focused.
- Focused on local AI, Ollama, .NET, and Windows productivity.
- Useful for LinkedIn, Twitter/X, and blog publishing.

### Image Prompt Rules

The file `docs/promotional/image-prompts.md` must include prompts for:

- A blog hero image.
- A LinkedIn image.
- A Twitter/X image.
- A square social media image.
- A simple app/logo-style image.

The prompts must be in English.

Prompts should avoid too much text inside the image.

Images should visually communicate:

- Local AI.
- Ollama.
- Windows system tray.
- Resource monitoring.
- CPU / GPU / RAM usage.
- .NET developer tooling.

### Text-to-Image Skill Rule

If the implementation environment has a text-to-image skill, image generation tool, or T2I capability installed, use it to generate the promotional images from the prompts.

Generated images should be saved under:

```text
docs/promotional/images/
```

Suggested image filenames:

```text
ollama-monitor-blog-hero.png
ollama-monitor-linkedin.png
ollama-monitor-twitter.png
ollama-monitor-square.png
ollama-monitor-logo-style.png
```

If no text-to-image skill is available, do not block the implementation. Instead:

- Create `docs/promotional/image-prompts.md`.
- Add clear instructions explaining that images can be generated later.
- Leave `docs/promotional/images/.gitkeep` in place.

## Core User Stories

### System Tray Status

As a developer, I want to see a system tray icon that tells me whether Ollama is running, so I do not need to open a terminal or Task Manager.

### Model Status

As a developer, I want to know whether Ollama currently has a model loaded, so I can understand what is consuming resources.

### Floating Details Window

As a developer, I want to open a small floating window from the tray icon, so I can see detailed status information when needed.

### Resource Usage

As a developer, I want to see CPU, memory, disk, and best-effort GPU usage for Ollama, so I can understand the impact of local AI workloads.

### Configuration

As a developer, I want to configure the Ollama endpoint and refresh interval from the CLI, so I can adapt the tool to my local setup.

## Phase 1 Scope

Phase 1 focuses on the Windows system tray experience and a simple floating details window.

### Included in Phase 1

- Windows system tray app.
- Status icon for Ollama state.
- Floating details window.
- Ollama API health check.
- Ollama version detection.
- Running/loaded model detection.
- Process CPU usage.
- Process memory usage.
- Basic disk read/write metrics if available.
- Best-effort NVIDIA GPU usage if `nvidia-smi` is available.
- Local configuration file.
- CLI launcher.
- CLI config commands.
- Copy status to clipboard.
- Manual refresh.
- Open Ollama API URL from tray menu.
- Initial docs folder.
- Initial promotional docs.
- Initial image prompts.
- Generate images if a text-to-image skill is available.

### Excluded from Phase 1

- Terminal live dashboard.
- Charts.
- Historical resource tracking.
- Remote Ollama monitoring.
- Multi-machine monitoring.
- Auto-update.
- Windows installer.
- Background Windows Service.
- Advanced GPU provider support for AMD/Intel.

## Suggested Commands

### Launch App

```bash
ollamamon
```

Launches the system tray app.

### Show Help

```bash
ollamamon --help
```

Shows CLI help.

### Show Config

```bash
ollamamon config
```

Prints the current configuration.

### Set Ollama Endpoint

```bash
ollamamon config set endpoint http://localhost:11434
```

Sets the Ollama API endpoint.

### Set Refresh Interval

```bash
ollamamon config set refresh-interval 2
```

Sets refresh interval in seconds.

### Reset Config

```bash
ollamamon config reset
```

Resets configuration to defaults.

## Default Configuration

```json
{
  "endpoint": "http://localhost:11434",
  "refreshIntervalSeconds": 2,
  "startMinimizedToTray": true,
  "showFloatingWindowOnStart": false,
  "enableGpuMetrics": true,
  "enableDiskMetrics": true
}
```

## System Tray UX

### Tray States

The tray icon should represent the current Ollama status.

```text
Off / Not reachable       -> Gray
Ollama running            -> Green
Model loaded              -> Blue
Model active / high usage -> Orange
Error                     -> Red
```

### Suggested Status Rules

```text
Gray:
- Ollama API endpoint is not reachable.

Green:
- Ollama API endpoint is reachable.
- No loaded/running model detected.

Blue:
- Ollama API endpoint is reachable.
- One or more models are loaded/running.

Orange:
- Ollama API endpoint is reachable.
- One or more models are loaded/running.
- CPU, memory, or GPU usage is above configurable threshold.

Red:
- Unexpected error while reading Ollama status or metrics.
```

### Tray Tooltip Examples

```text
Ollama: Not reachable
```

```text
Ollama: Running - no model loaded
```

```text
Ollama: llama3.2:latest | CPU 18% | RAM 5.4 GB
```

```text
Ollama: Error reading status
```

## Tray Context Menu

Right-clicking the tray icon should show:

```text
ElBruno.OllamaMonitor
Status: Running
Model: llama3.2:latest

Show / Hide Details
Refresh
Copy Status
Open Ollama API
Settings
Exit
```

### Menu Behavior

#### Show / Hide Details

Toggles the floating details window.

#### Refresh

Immediately refreshes Ollama status and metrics.

#### Copy Status

Copies a plain text status summary to clipboard.

Example:

```text
Ollama Status: Running
Endpoint: http://localhost:11434
Version: 0.x.x
Model: llama3.2:latest
CPU: 18%
Memory: 5.4 GB
GPU: NVIDIA RTX 4070 - 62%
```

#### Open Ollama API

Opens the configured endpoint, for example:

```text
http://localhost:11434
```

#### Settings

For Phase 1, this can open the configuration file location or show a basic settings dialog.

#### Exit

Closes the app.

## Floating Window UX

The floating window should be simple, compact, and demo-friendly.

### Window Behavior

- Can be shown or hidden from tray menu.
- Should not steal focus aggressively.
- Should remember last position if possible.
- Should be resizable or have a compact fixed size.
- Should refresh automatically based on the configured refresh interval.
- Should include a manual refresh button.
- Should include a copy status button.

### Suggested Sections

```text
Ollama Status
- Status
- Endpoint
- Version
- Last checked
- API reachable

Models
- Loaded/running models
- Model name
- Size, if available
- Processor/details, if available
- Expires/unload time, if available

Resources
- CPU %
- Memory used
- Disk read/write
- GPU usage, if available
- VRAM usage, if available

Actions
- Refresh
- Copy status
- Open Ollama API
```

## Data Sources

### Ollama API

The tool should call the configured Ollama endpoint.

Default:

```text
http://localhost:11434
```

Useful endpoints:

```text
GET /api/version
GET /api/tags
GET /api/ps
```

Expected usage:

```text
/api/version -> Detect Ollama version.
/api/tags    -> List locally available models.
/api/ps      -> List currently loaded/running models.
```

### Process Metrics

The tool should find the local Ollama process.

Likely process names:

```text
ollama
ollama.exe
```

Metrics to collect:

```text
CPU %
Working set memory
Private memory
Disk read bytes
Disk write bytes
Process start time, if available
```

### GPU Metrics

Phase 1 GPU metrics should be best-effort.

Preferred Phase 1 approach:

```text
If nvidia-smi is available:
- Run nvidia-smi query commands.
- Parse GPU usage.
- Parse VRAM usage.
- Optionally detect processes using GPU.
```

If GPU metrics cannot be read:

```text
GPU: Not available
```

or

```text
GPU: Not supported
```

Do not fail the application if GPU metrics are unavailable.

## Technical Requirements

## Platform

Phase 1 targets:

```text
Windows 10+
Windows 11
```

## Framework

Use modern .NET.

Preferred:

```text
.NET 9 or .NET 10
```

If .NET 10 is available and stable in the project environment, use .NET 10.

## UI

Recommended:

```text
WPF
```

Recommended tray library:

```text
H.NotifyIcon.Wpf
```

Alternative:

```text
WinForms NotifyIcon
```

Preference:

Use WPF for the floating window and tray integration.

## Architecture

### Main Components

#### App Startup

Responsibilities:

- Parse CLI args.
- If no args, launch tray app.
- If config args, run config command and exit.
- If help args, print help and exit.

#### OllamaClient

Responsibilities:

- Call Ollama API endpoints.
- Handle unavailable endpoint gracefully.
- Return typed models.
- Avoid throwing unhandled exceptions to UI.

#### OllamaStatusService

Responsibilities:

- Combine API status and model information.
- Determine high-level status:
  - Off
  - Running
  - ModelLoaded
  - HighUsage
  - Error

#### ProcessMetricsService

Responsibilities:

- Locate Ollama process.
- Calculate CPU usage over time.
- Read memory usage.
- Read disk metrics if available.

#### GpuMetricsService

Responsibilities:

- Detect whether GPU metrics are available.
- Use NVIDIA provider when possible.
- Return unavailable status when unsupported.

#### TrayIconService

Responsibilities:

- Own tray icon lifecycle.
- Update icon based on status.
- Build context menu.
- Show/hide floating window.
- Handle exit.

#### MainWindow

Responsibilities:

- Display current status.
- Display current model details.
- Display resource usage.
- Provide Refresh, Copy Status, and Open API actions.

#### AppSettingsService

Responsibilities:

- Load and save config.
- Create default config on first run.
- Support CLI config updates.

## Status Model

Suggested C# enum:

```csharp
public enum OllamaMonitorState
{
    NotReachable,
    Running,
    ModelLoaded,
    HighUsage,
    Error
}
```

Suggested status record:

```csharp
public sealed record OllamaMonitorSnapshot
{
    public OllamaMonitorState State { get; init; }
    public string Endpoint { get; init; } = "";
    public string? Version { get; init; }
    public bool IsApiReachable { get; init; }
    public IReadOnlyList<OllamaModelSnapshot> Models { get; init; } = [];
    public ResourceSnapshot? Resources { get; init; }
    public DateTimeOffset LastChecked { get; init; }
    public string? ErrorMessage { get; init; }
}
```

Suggested model record:

```csharp
public sealed record OllamaModelSnapshot
{
    public string Name { get; init; } = "";
    public string? Size { get; init; }
    public string? Processor { get; init; }
    public DateTimeOffset? ExpiresAt { get; init; }
}
```

Suggested resource record:

```csharp
public sealed record ResourceSnapshot
{
    public double? CpuPercent { get; init; }
    public long? MemoryBytes { get; init; }
    public double? MemoryGb { get; init; }
    public long? DiskReadBytesPerSecond { get; init; }
    public long? DiskWriteBytesPerSecond { get; init; }
    public double? GpuPercent { get; init; }
    public double? VramUsedGb { get; init; }
    public double? VramTotalGb { get; init; }
    public string? GpuName { get; init; }
}
```

## Error Handling

The app should never crash because Ollama is not running.

Expected failure cases:

```text
Ollama API not reachable.
Ollama API returns unexpected JSON.
Ollama process not found.
Multiple Ollama processes found.
nvidia-smi not installed.
nvidia-smi output cannot be parsed.
Config file missing.
Config file corrupted.
```

Expected behavior:

```text
- Show safe status.
- Display friendly error.
- Continue refreshing.
- Log diagnostic details locally if logging is implemented.
```

## Configuration File

Suggested location:

```text
%LOCALAPPDATA%\ElBruno\OllamaMonitor\settings.json
```

Example:

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

## Logging

Phase 1 can use simple file logging.

Suggested location:

```text
%LOCALAPPDATA%\ElBruno\OllamaMonitor\logs
```

Log useful diagnostics:

```text
App startup
Config loaded
Ollama endpoint unreachable
Unexpected API response
GPU metrics unavailable
Unhandled exceptions
```

## Packaging

The tool should be packaged as a .NET global tool.

Install command:

```bash
dotnet tool install --global ElBruno.OllamaMonitor
```

Update command:

```bash
dotnet tool update --global ElBruno.OllamaMonitor
```

Run command:

```bash
ollamamon
```

## Phase 1 Acceptance Criteria

### App Launch

- Running `ollamamon` launches the tray app.
- The app adds an icon to the Windows system tray.
- The app does not show a console window during normal tray usage if possible.
- The app can be exited from the tray menu.

### Ollama Detection

- If Ollama is not running, the tray icon shows the Not Reachable state.
- If Ollama is running, the tray icon shows the Running state.
- If one or more models are loaded, the tray icon shows the Model Loaded state.
- The floating window displays endpoint, version, status, and last checked time.

### Model Detection

- The app uses `/api/ps` to detect loaded/running models.
- The floating window lists loaded/running model names.
- If no model is loaded, the UI clearly says so.

### Resource Metrics

- The app shows CPU usage for the Ollama process.
- The app shows memory usage for the Ollama process.
- The app shows disk metrics when available.
- The app shows GPU metrics when available.
- If GPU metrics are not available, the app shows a friendly unavailable state.

### Floating Window

- The tray menu can show the floating window.
- The tray menu can hide the floating window.
- The floating window refreshes automatically.
- The floating window has a Refresh action.
- The floating window has a Copy Status action.

### Configuration

- `ollamamon config` shows current config.
- `ollamamon config set endpoint <url>` updates the endpoint.
- `ollamamon config set refresh-interval <seconds>` updates refresh interval.
- `ollamamon config reset` resets to defaults.

### Documentation and Promotional Material

- Root `README.md` exists.
- Root `LICENSE` exists.
- `docs` folder exists.
- `docs/architecture.md` exists.
- `docs/configuration.md` exists.
- `docs/development-guide.md` exists.
- `docs/troubleshooting.md` exists.
- `docs/release-notes.md` exists.
- `docs/promotional/blog-post.md` exists.
- `docs/promotional/linkedin-post.md` exists.
- `docs/promotional/twitter-post.md` exists.
- `docs/promotional/image-prompts.md` exists.
- `docs/promotional/images` folder exists.
- If T2I capability is available, generated images exist in `docs/promotional/images`.

## Phase 2 Scope

Phase 2 adds a better developer experience and more diagnostics.

### Features

- Terminal status command:

```bash
ollamamon status
```

- Terminal live watch command:

```bash
ollamamon watch
```

- Better settings UI.
- Configurable thresholds.
- Configurable tray icon behavior.
- Better GPU provider abstraction.
- Improved NVIDIA process-level GPU mapping.
- Export diagnostics bundle.
- Open Ollama logs if detectable.
- Optional notification when Ollama starts or stops.
- Optional notification when a model is loaded.

## Phase 3 Scope

Phase 3 focuses on richer monitoring.

### Features

- Historical charts in floating window.
- CPU, RAM, GPU, and VRAM timeline.
- Per-model activity timeline.
- Request activity detection if possible.
- Model unload countdown based on `expires_at` when available.
- Support for remote Ollama endpoint monitoring.
- Support for multiple Ollama endpoints.
- Optional compact mode.
- Optional always-on-top mode.
- Optional transparency setting.

## Phase 4 Scope

Phase 4 focuses on ecosystem integration.

### Features

- Integration with .NET Aspire dashboard scenarios.
- Integration with local AI demos.
- Optional MCP server exposing Ollama monitor status.
- Optional JSON output for automation.
- Optional Prometheus-style metrics endpoint.
- Optional GitHub Copilot CLI helper commands.
- Optional Windows startup registration.
- Optional installer in addition to .NET tool packaging.

## Suggested Initial Implementation Tasks

### Task 1: Create Solution Structure

Create the .NET solution and initial projects.

```text
ElBruno.OllamaMonitor
```

Use WPF for the tray and floating window experience.

### Task 2: Implement Configuration

Create:

```text
AppSettings
AppSettingsService
```

Support default config, load, save, reset, and CLI updates.

### Task 3: Implement CLI Argument Parsing

Support:

```bash
ollamamon
ollamamon --help
ollamamon config
ollamamon config set endpoint http://localhost:11434
ollamamon config set refresh-interval 2
ollamamon config reset
```

### Task 4: Implement Ollama API Client

Create `OllamaClient`.

Support:

```text
GET /api/version
GET /api/tags
GET /api/ps
```

Handle unavailable endpoint gracefully.

### Task 5: Implement Status Service

Create `OllamaStatusService`.

Combine:

```text
API reachable
Version
Running models
Errors
```

Return a single `OllamaMonitorSnapshot`.

### Task 6: Implement Process Metrics

Create `ProcessMetricsService`.

Find Ollama process and collect:

```text
CPU %
Memory
Disk read/write if possible
```

### Task 7: Implement GPU Metrics

Create `NvidiaSmiMetricsService`.

Detect `nvidia-smi`.

If available, collect:

```text
GPU name
GPU usage %
VRAM used
VRAM total
```

If not available, return unavailable state.

### Task 8: Implement Tray Icon

Create `TrayIconService`.

Support:

```text
Icon state updates
Tooltip updates
Context menu
Show/hide details window
Refresh
Copy status
Open API
Exit
```

### Task 9: Implement Floating Window

Create the main details UI.

Display:

```text
Ollama status
Endpoint
Version
Loaded models
CPU
Memory
Disk
GPU
Last checked
```

### Task 10: Create Documentation

Create all required documentation files under `docs`.

Do not place documentation files at the root except `README.md` and `LICENSE`.

### Task 11: Create Promotional Material

Create:

```text
docs/promotional/blog-post.md
docs/promotional/linkedin-post.md
docs/promotional/twitter-post.md
docs/promotional/image-prompts.md
```

Include image prompts for blog, LinkedIn, Twitter/X, square social, and logo-style images.

### Task 12: Generate Promotional Images if Possible

If a text-to-image skill is available, generate images and save them under:

```text
docs/promotional/images/
```

If not available, create `.gitkeep` and continue.

### Task 13: Package as .NET Tool

Configure the project for NuGet packaging as a .NET global tool.

Expected install:

```bash
dotnet tool install --global ElBruno.OllamaMonitor
```

Expected run:

```bash
ollamamon
```

## Suggested README Summary

```markdown
# ElBruno.OllamaMonitor

A tiny Windows system tray tool to monitor your local Ollama runtime.

It shows whether Ollama is running, which model is currently loaded, and how much CPU, memory, disk, and GPU resources Ollama is using.

The tray icon gives you the quick status.  
The floating window gives you the details.

## Install

```bash
dotnet tool install --global ElBruno.OllamaMonitor
```

## Usage

```bash
ollamamon
```

Configure the local Ollama endpoint:

```bash
ollamamon config set endpoint http://localhost:11434
```

## Why?

Because local AI should not be a mysterious process silently eating your GPU.
```

## Suggested Promotional Content

### Blog Post Draft

```markdown
# Monitoring Ollama from the Windows System Tray with .NET

Local AI is amazing... until your laptop fan starts preparing for takeoff and you are not sure which model is eating your GPU.

That is why I started building **ElBruno.OllamaMonitor**, a tiny Windows system tray tool built with .NET.

The idea is simple:

- See if Ollama is running.
- See which model is currently loaded.
- Check CPU, memory, disk, and best-effort GPU usage.
- Show or hide a small floating window with details.
- Keep everything local and developer-friendly.

The first version focuses on the tray experience. The CLI is only used to launch the app and configure basic settings.

Because sometimes the best developer tools are the tiny ones that answer one question really fast:

**What is my local AI runtime doing right now?**
```

### LinkedIn Post Draft

```markdown
I started working on a tiny .NET tool for everyone running local AI with Ollama on Windows.

The idea is simple:

- A system tray icon that shows if Ollama is running.
- A quick way to know if a model is loaded.
- A floating window with CPU, RAM, disk, and best-effort GPU usage.
- A simple CLI only to launch the app and manage config.

Repo/package name:

ElBruno.OllamaMonitor

CLI:

ollamamon

Because local AI should not be a mysterious process silently eating your GPU. 😄

#dotnet #AI #Ollama #LocalAI #WindowsDev
```

### Twitter/X Post Draft

```markdown
Building a tiny .NET tool for local AI devs 👇

ElBruno.OllamaMonitor

A Windows tray app that shows:
- Is Ollama running?
- Which model is loaded?
- CPU / RAM / disk / GPU usage
- Floating details window when needed

Because local AI should not be a mystery process eating your GPU 😄
```

## Suggested Image Prompts

### Blog Hero Image Prompt

```text
Create a clean 16:9 hero image for a developer blog post about a Windows system tray app that monitors Ollama local AI runtime. Show a modern Windows desktop, a small system tray status icon, a floating monitoring panel with CPU, GPU, RAM indicators, and subtle local AI visual elements. Use a modern technical style, minimal text, no brand logos, no clutter.
```

### LinkedIn Image Prompt

```text
Create a professional LinkedIn image showing a developer workstation running local AI. Include a Windows system tray area, a small floating monitor window, and visual indicators for Ollama status, CPU, GPU, and memory. Make it clean, modern, and suitable for a .NET developer audience. Avoid too much text.
```

### Twitter/X Image Prompt

```text
Create a simple social media image for a tiny .NET tool called ElBruno.OllamaMonitor. Visual theme: local AI, Ollama monitoring, Windows system tray, CPU/GPU/RAM indicators, lightweight developer utility. Make it fun, minimal, and eye-catching. Avoid dense text.
```

### Square Social Image Prompt

```text
Create a square image for social media promoting a local AI monitoring tool. Show a cute but technical representation of a local AI runtime being monitored from a Windows tray icon, with a small floating dashboard showing CPU, GPU, and memory usage. Clean composition, modern developer aesthetic, minimal text.
```

### Logo-Style Image Prompt

```text
Create a simple logo-style image for ElBruno.OllamaMonitor. Concept: a small monitor pulse/status icon combined with local AI and system tray monitoring. Modern, minimal, friendly developer tool aesthetic. No complex text, no official brand logos.
```

## Implementation Notes for Copilot

Please implement Phase 1 first.

Prioritize:

1. Reliable tray app startup.
2. Safe Ollama API detection.
3. Clean floating window.
4. Basic process metrics.
5. Best-effort GPU metrics.
6. Simple CLI config support.
7. Required docs structure.
8. Promotional documents.
9. Image prompts and generated images when possible.

Avoid overengineering.

Use clean services and typed models so Phase 2 can add CLI status/watch commands without rewriting the core logic.
