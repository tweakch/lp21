public class MetricBase {
    protected MetricBase(ILogger logger)
    {
        _logger = logger;
    }

    protected readonly ILogger _logger;
    protected void WriteMetric(double value, string[]? labelValues = null)
    {
        if(labelValues == null || !labelValues.Any()) _logger.LogInformation("{value}", value);
        else _logger.LogInformation("{value} ({labels})", value, string.Join(", ", labelValues));
    }
}
