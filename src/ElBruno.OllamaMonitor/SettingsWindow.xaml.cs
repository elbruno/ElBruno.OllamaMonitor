using System.Windows;
using ElBruno.OllamaMonitor.ViewModels;

namespace ElBruno.OllamaMonitor;

public partial class SettingsWindow : Window
{
    private readonly SettingsWindowViewModel? _viewModel;

    public SettingsWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _viewModel?.Dispose();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsWindowViewModel viewModel)
        {
            await viewModel.SaveAsync(CancellationToken.None);
            Close();
        }
    }
}
