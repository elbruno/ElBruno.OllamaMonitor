using System.ComponentModel;

namespace ElBruno.OllamaMonitor;

public partial class MainWindow : System.Windows.Window
{
    private bool _allowClose;

    public MainWindow()
    {
        InitializeComponent();
    }

    public void PrepareForExit() => _allowClose = true;

    protected override void OnClosing(CancelEventArgs e)
    {
        if (!_allowClose)
        {
            e.Cancel = true;
            Hide();
            return;
        }

        base.OnClosing(e);
    }
}
