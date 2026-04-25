using System.ComponentModel;
using ElBruno.OllamaMonitor.Services;

namespace ElBruno.OllamaMonitor;

public partial class MainWindow : System.Windows.Window
{
    private bool _allowClose;

    public MainWindow()
    {
        InitializeComponent();
        this.Loaded += (_, _) => InitializeThemeSelector();
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

    private void InitializeThemeSelector()
    {
        var savedTheme = ThemeService.GetSavedThemePreference();
        ThemeSelector.SelectedItem = savedTheme.ToString();
    }

    private void ThemeSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (ThemeSelector.SelectedItem is string selectedItem &&
            System.Enum.TryParse<Services.ThemeMode>(selectedItem, out var theme))
        {
            ThemeService.ApplyTheme(theme);
            ThemeService.SaveThemePreference(theme);
        }
    }
}
