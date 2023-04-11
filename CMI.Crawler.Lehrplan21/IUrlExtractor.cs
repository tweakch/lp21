namespace CMI.Crawler.Lehrplan21;

public interface INodeExtractor
{
    IAsyncEnumerable<TreeNode> ExtractNodes(Stream jsonStream);
}
