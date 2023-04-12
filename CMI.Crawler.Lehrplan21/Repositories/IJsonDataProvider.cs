using CMI.Crawler.Lehrplan21.Models;

namespace CMI.Crawler.Lehrplan21.Repositories;

public interface IJsonDataProvider
{
    string Language { get; set; }
    Task<Stream> GetJsonStreamAsync(TreeNode node);
}
