namespace CMI.Crawler.Lehrplan21.Metrics;

public interface IGaugeMetric
{
    void Set(double value, string[]? labelValues = null);
    void Inc(double value = 1, string[]? labelValues = null);
    void Dec(double value = 1, string[]? labelValues = null);
}