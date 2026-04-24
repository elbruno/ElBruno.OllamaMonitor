using ElBruno.OllamaMonitor.Configuration;
using ElBruno.OllamaMonitor.Diagnostics;
using ElBruno.OllamaMonitor.Interop;

namespace ElBruno.OllamaMonitor.Cli;

public sealed class CliCommandRunner
{
    private readonly AppSettingsService _settingsService;
    private readonly DiagnosticsLogService _diagnostics;

    public CliCommandRunner(AppSettingsService settingsService, DiagnosticsLogService diagnostics)
    {
        _settingsService = settingsService;
        _diagnostics = diagnostics;
    }

    public async Task<int> RunAsync(CliCommand command, CancellationToken cancellationToken)
    {
        ConsoleManager.EnsureConsole();
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        switch (command.Kind)
        {
            case CliCommandKind.Help:
                Console.WriteLine(BuildHelpText());
                return 0;

            case CliCommandKind.ShowConfig:
                Console.WriteLine(await _settingsService.LoadAsJsonAsync(cancellationToken));
                return 0;

            case CliCommandKind.SetEndpoint:
                await _settingsService.UpdateEndpointAsync(command.StringValue!, cancellationToken);
                Console.WriteLine($"Updated endpoint to {command.StringValue}.");
                return 0;

            case CliCommandKind.SetRefreshInterval:
                await _settingsService.UpdateRefreshIntervalAsync(command.IntValue!.Value, cancellationToken);
                Console.WriteLine($"Updated refresh interval to {command.IntValue} second(s).");
                return 0;

            case CliCommandKind.ResetConfig:
                await _settingsService.ResetAsync(cancellationToken);
                Console.WriteLine("Configuration reset to defaults.");
                return 0;

            case CliCommandKind.Invalid:
                _diagnostics.WriteWarning(command.ErrorMessage ?? "Invalid CLI command.");
                Console.Error.WriteLine(command.ErrorMessage ?? "Invalid CLI command.");
                Console.WriteLine();
                Console.WriteLine(BuildHelpText());
                return 1;

            default:
                return 0;
        }
    }

    public static string BuildHelpText() => string.Join(
        Environment.NewLine,
        [
            "ElBruno.OllamaMonitor",
            string.Empty,
            "Usage:",
            "  ollamamon",
            "  ollamamon --help",
            "  ollamamon config",
            "  ollamamon config set endpoint <url>",
            "  ollamamon config set refresh-interval <seconds>",
            "  ollamamon config reset"
        ]);
}
