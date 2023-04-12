using CMI.Crawler.Lehrplan21.Models;

namespace CMI.Crawler.Lehrplan21.Services;
public interface INodeProcessor
{
    ValueTask<TreeNode?> ProcessNodeAsync(Stream node);
}
