using CMI.Crawler.Lehrplan21.Metrics;

namespace CMI.Crawler.Lehrplan21.Services;

public class NodePersisterMetricsLogger : INodePersisterWorkerMetrics
{
    private readonly IGaugeMetric gauge;
    private readonly ICounterMetric counter;

    public NodePersisterMetricsLogger(IGaugeMetric gauge, ICounterMetric counter)
    {
        this.gauge = gauge;
        this.counter = counter;
    }
    public void IncrementNodeError(double value = 1)
    {
        counter.Inc(value, new string[] { "node_error" });
    }

    public void IncrementPersistNode(double value = 1)
    {
        counter.Inc(value, new[] { "persist_node" });
    }

    public void GaugePersistNode(double value = 1.0)
    {
        gauge.Set(value, new[] { "persist_node" });
    }
}
