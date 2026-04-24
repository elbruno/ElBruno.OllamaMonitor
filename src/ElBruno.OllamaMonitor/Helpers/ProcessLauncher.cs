using System.Diagnostics;
using ElBruno.OllamaMonitor.Diagnostics;

namespace ElBruno.OllamaMonitor.Helpers;

public static class ProcessLauncher
{
    public static void Open(string urlOrPath, DiagnosticsLogService diagnostics)
    {
        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = urlOrPath,
                UseShellExecute = true
            });
        }
        catch (Exception exception)
        {
            diagnostics.WriteError($"Unable to open '{urlOrPath}'.", exception);
        }
    }
}
