using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ElBruno.OllamaMonitor.Helpers;

public static class SparklineRenderer
{
    public static void DrawSparkline(
        Canvas canvas,
        Queue<double> samples,
        System.Windows.Media.Brush strokeColor,
        double width = 80,
        double height = 20)
    {
        canvas.Children.Clear();

        if (samples.Count < 2)
            return;

        var polyline = new Polyline
        {
            Stroke = strokeColor,
            StrokeThickness = 1.5,
            Points = new PointCollection()
        };

        double maxValue = samples.Max();
        if (maxValue <= 0) maxValue = 1; // Prevent division by zero

        double xStep = width / (samples.Count - 1);
        int i = 0;

        foreach (var sample in samples)
        {
            double y = height - (sample / maxValue * height);
            polyline.Points.Add(new System.Windows.Point(i * xStep, y));
            i++;
        }

        canvas.Children.Add(polyline);
    }
}
