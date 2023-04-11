namespace CMI.Crawler.Lehrplan21;

public interface IJsonDataProvider
{
    string Language { get; set; }
    Task<Stream> GetJsonStreamAsync(TreeNode node);
}
