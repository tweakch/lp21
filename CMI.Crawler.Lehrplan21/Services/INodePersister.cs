using CMI.Crawler.Lehrplan21.Models;

namespace CMI.Crawler.Lehrplan21.Services;

public interface INodePersister
{
    Task PersistNodeAsync(PersistContext context, CancellationToken cancellationToken);
}
