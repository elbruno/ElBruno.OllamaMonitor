# Windows Tool Smoke Validation

## When to use

Use this when a Windows desktop app is packaged behind a .NET global tool shim and you need a fast release-readiness check.

## Pattern

1. Run `dotnet restore` and `dotnet build` on the solution first.
2. Exercise the repository's intended packaging entry point rather than raw `dotnet pack` if payload injection is part of the design.
3. Inspect the generated `.nupkg` to confirm the desktop payload exists under the expected tool path.
4. Install the package to a repo-local `--tool-path` and smoke-test:
   - `--help`
   - config show
   - config mutations
   - config reset
5. For no-arg launch, set the configured endpoint to an unreachable localhost port, start the installed tool, and confirm the desktop process is spawned and remains alive briefly instead of crashing on startup.
6. Use a deliberately unreachable localhost endpoint to validate offline behavior without depending on an external service.

## Project example

- Packaging script: `build\Pack-Tool.ps1`
- Payload injector: `build\Inject-DesktopPayload.ps1`
- Tool install test: `dotnet tool install ElBruno.OllamaMonitor --tool-path <repo-local-path> --add-source .\artifacts\packages --version 0.1.0`
- Offline launch check: `ollamamon config set endpoint http://127.0.0.1:65535` then launch `ollamamon` and verify `ElBruno.OllamaMonitor.exe` is still running after ~10 seconds before stopping it.
