using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ElBruno.OllamaMonitor.Helpers;
using ElBruno.OllamaMonitor.Models;

namespace ElBruno.OllamaMonitor;

public partial class MiniMonitorWindow : Window
{
    private bool _allowClose;
    private System.Windows.Threading.DispatcherTimer? _sparklineTimer;
    private readonly Dictionary<Canvas, (int lastCount, SolidColorBrush brush)> _canvasStates = new();
    
    private static readonly SolidColorBrush CpuBrush = new(System.Windows.Media.Color.FromRgb(96, 165, 250)); // #60A5FA
    private static readonly SolidColorBrush MemBrush = new(System.Windows.Media.Color.FromRgb(167, 139, 250)); // #A78BFA
    private static readonly SolidColorBrush GpuBrush = new(System.Windows.Media.Color.FromRgb(52, 211, 153)); // #34D399

    public MiniMonitorWindow()
    {
        InitializeComponent();
        this.Loaded += (_, _) => StartSparklineRefresh();
        this.Closing += (_, _) => StopSparklineRefresh();
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

    private void OnWindowDrag(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed)
        {
            return;
        }

        DragMove();
    }

    private void OnCloseClicked(object sender, RoutedEventArgs e)
    {
        Hide();
    }

    private void StartSparklineRefresh()
    {
        _sparklineTimer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(2)
        };
        _sparklineTimer.Tick += (_, _) => RefreshSparklines();
        _sparklineTimer.Start();
    }

    private void StopSparklineRefresh()
    {
        if (_sparklineTimer is not null)
        {
            _sparklineTimer.Stop();
            _sparklineTimer = null;
        }
    }

    private void RefreshSparklines()
    {
        var itemsControl = this.FindName("ItemsControlModels") as ItemsControl;
        if (itemsControl is null)
            return;

        var container = itemsControl.ItemContainerGenerator;
        for (int i = 0; i < itemsControl.Items.Count; i++)
        {
            var contentPresenter = container.ContainerFromIndex(i) as ContentPresenter;
            if (contentPresenter is null)
                continue;

            var template = itemsControl.ItemTemplate;
            if (template?.FindName("CpuChart", contentPresenter) is Canvas cpuChart &&
                template?.FindName("MemChart", contentPresenter) is Canvas memChart &&
                template?.FindName("GpuChart", contentPresenter) is Canvas gpuChart)
            {
                if (itemsControl.Items[i] is OllamaModelSnapshot model)
                {
                    RedrawIfChanged(cpuChart, model.History.CpuSamples, CpuBrush, 80, 20);
                    RedrawIfChanged(memChart, model.History.MemorySamples, MemBrush, 80, 20);
                    RedrawIfChanged(gpuChart, model.History.GpuSamples, GpuBrush, 80, 20);
                }
            }
        }
    }

    private void RedrawIfChanged(Canvas canvas, Queue<double> samples, SolidColorBrush brush, double width, double height)
    {
        int currentCount = samples.Count;
        
        if (_canvasStates.TryGetValue(canvas, out var state) && state.lastCount == currentCount)
        {
            return; // No change, skip redraw
        }

        _canvasStates[canvas] = (currentCount, brush);
        SparklineRenderer.DrawSparkline(canvas, samples, brush, width, height);
    }
}
