namespace CMI.Crawler.Lehrplan21;

public interface INodeProcessor
{
    ValueTask<TreeNode?> ProcessNodeAsync(Stream node);
}
