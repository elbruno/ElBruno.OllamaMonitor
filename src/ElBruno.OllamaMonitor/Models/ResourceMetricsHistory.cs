namespace ElBruno.OllamaMonitor.Models;

public class ResourceMetricsHistory
{
    private const int MaxSamples = 30;

    public Queue<double> CpuSamples { get; } = new Queue<double>(MaxSamples);
    public Queue<double> MemorySamples { get; } = new Queue<double>(MaxSamples);
    public Queue<double> GpuSamples { get; } = new Queue<double>(MaxSamples);

    public void AddSample(double cpu, double memory, double gpu)
    {
        if (double.IsNaN(cpu) || double.IsInfinity(cpu)) cpu = 0;
        if (double.IsNaN(memory) || double.IsInfinity(memory)) memory = 0;
        if (double.IsNaN(gpu) || double.IsInfinity(gpu)) gpu = 0;

        CpuSamples.Enqueue(cpu);
        MemorySamples.Enqueue(memory);
        GpuSamples.Enqueue(gpu);

        if (CpuSamples.Count > MaxSamples) CpuSamples.Dequeue();
        if (MemorySamples.Count > MaxSamples) MemorySamples.Dequeue();
        if (GpuSamples.Count > MaxSamples) GpuSamples.Dequeue();
    }

    public void Clear()
    {
        CpuSamples.Clear();
        MemorySamples.Clear();
        GpuSamples.Clear();
    }
}
