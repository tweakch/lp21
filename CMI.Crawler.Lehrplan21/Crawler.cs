namespace CMI.Crawler.Lehrplan21;

public class Crawler : ICrawler
{
    private readonly IJsonDataProvider _jsonDataProvider;
    private readonly INodeExtractor _nodeExtractor;

    public Crawler(IJsonDataProvider jsonDataProvider, INodeExtractor nodeExtractor)
    {
        _jsonDataProvider = jsonDataProvider;
        _nodeExtractor = nodeExtractor;
    }

    public async Task CrawlAsync(TreeNode node)
    {
        if(!node.Kind.Equals("Url")) return;
        using Stream jsonStream = await _jsonDataProvider.GetJsonStreamAsync(node);
        await foreach (TreeNode childNode in _nodeExtractor.ExtractNodes(jsonStream))
        {
            Console.WriteLine(childNode.ToString());
            await CrawlAsync(childNode);
        }
    }
}
