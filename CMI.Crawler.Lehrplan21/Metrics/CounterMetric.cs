namespace CMI.Crawler.Lehrplan21.Metrics;

public class CounterMetric : MetricBase, ICounterMetric
{
    public CounterMetric(ILogger<CounterMetric> logger) : base(logger) { }

    private double _value;

    public void Inc(double value = 1, string[]? labelValues = null)
    {
        _value += value;
        WriteMetric(_value, labelValues);
    }
}
