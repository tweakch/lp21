namespace CMI.Crawler.Lehrplan21.Services;

public interface INodePersisterWorkerMetrics
{
    void IncrementPersistNode(double value = 1);
    void GaugePersistNode(double value = 1.0);
    void IncrementNodeError(double value = 1);
}
