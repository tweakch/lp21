using CMI.Crawler.Lehrplan21.Models;

namespace CMI.Crawler.Lehrplan21;

public interface INodeExtractor
{
    IAsyncEnumerable<TreeNode> ExtractNodes(Stream jsonStream);
}
