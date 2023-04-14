using CMI.Crawler.Lehrplan21.Metrics;

public class GaugeMetric : MetricBase, IGaugeMetric
{
    private Dictionary<long, double> _values = new Dictionary<long, double>();

    private double _value = 0;

    public GaugeMetric(ILogger<GaugeMetric> logger) : base(logger)
    {
    }

    public void Dec(double value = 1, string[]? labelValues = null)
    {
        Set(_value-value, labelValues);
    }

    public void Inc(double value = 1, string[]? labelValues = null)
    {        
        Set(_value+value, labelValues);
    }

    public void Set(double value, string[]? labelValues = null)
    {
        if(value == double.PositiveInfinity) return;
        _values.Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), value);
        
        // _values contains values at specific Unix timestamps
        // set _value to the average rate of the last 10 entries
        var values = _values.OrderByDescending(x => x.Key).Take(10).Select(x => x.Value).ToList();
        _value = values.Average();
        WriteMetric(_value, labelValues);
    }
}