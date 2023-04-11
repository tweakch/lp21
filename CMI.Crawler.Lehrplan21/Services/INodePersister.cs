namespace CMI.Crawler.Lehrplan21;

public interface INodePersister
{
    Task PersistNodeAsync(TreeNode node);
}
