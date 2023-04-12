using CMI.Crawler.Lehrplan21.Models;

namespace CMI.Crawler.Lehrplan21.Services;

public class NodePersister : INodePersister
{
    public Task PersistNodeAsync(TreeNode node)
    {
        // Implement logic to persist the node to a database or any other storage.
        // For this example, let's just print the node details to the console.

        Console.WriteLine($"Persisting node: {node.Id} - {node.Kind}");
        return Task.CompletedTask;
    }
}