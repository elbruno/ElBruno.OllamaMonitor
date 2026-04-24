using System.Runtime.InteropServices;

namespace ElBruno.OllamaMonitor.Interop;

internal static class NativeMethods
{
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetProcessIoCounters(IntPtr hProcess, out IoCounters ioCounters);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool AttachConsole(int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetConsoleWindow();

    [StructLayout(LayoutKind.Sequential)]
    internal struct IoCounters
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }
}
