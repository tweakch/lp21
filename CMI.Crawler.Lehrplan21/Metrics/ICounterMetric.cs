namespace CMI.Crawler.Lehrplan21.Metrics;

public interface ICounterMetric
{
    void Inc(double value = 1, string[]? labelValues = null);
}
