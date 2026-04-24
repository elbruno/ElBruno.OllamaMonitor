# Skill: .NET WPF Tray Tool

## Pattern

Split the implementation into a Windows desktop app plus a neutral .NET tool shim when you need both WPF tray UX and `dotnet tool` packaging.

## Recommended Structure

- `src\YourApp` → `net10.0-windows...`, `WinExe`, `UseWPF`, `UseWindowsForms`
- `src\YourApp.Tool` → `net10.0`, `PackAsTool`, `ToolCommandName`
- Share CLI/config files by source linking or a separate core library.

## Why

`PackAsTool` does not support WPF, WinForms, or `-windows` TFMs on .NET 5+, so a direct WPF global tool package will fail with `NETSDK1146`.

## Packaging Flow

1. Publish the desktop app for Windows (`win-x64` is a safe default).
2. Pack the `net10.0` tool project.
3. Inject the published desktop payload under `tools/<tfm>/any/desktop/` in the `.nupkg`.
4. Let the tool shim launch `desktop\YourApp.exe` when no CLI args are supplied.

## CLI + GUI Routing

- Tool shim: parse args, run config/help in console mode, otherwise start the packaged desktop executable.
- Desktop app: can still support the same CLI parser during local development, but the packaged tool should be the primary entry point.

## Console Output

If the desktop app also supports CLI commands, attach to the parent console before writing output:

```csharp
AttachConsole(-1);
```

This keeps `WinExe` tray launches windowless while preserving useful console output for config/help flows.

## Process CPU %

Use two-sample `Process.TotalProcessorTime` with a known delay, divided by `Environment.ProcessorCount`.
