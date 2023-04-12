using CMI.Crawler.Lehrplan21.Models;

namespace CMI.Crawler.Lehrplan21;

public interface ICrawler
{
    Task CrawlAsync(TreeNode node);
}
