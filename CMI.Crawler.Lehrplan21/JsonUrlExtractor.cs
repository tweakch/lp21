using CMI.Crawler.Lehrplan21.Models;
using System.Text.Json;
using System.Web;

namespace CMI.Crawler.Lehrplan21;

public class LehrplanNodeExtractor : INodeExtractor
{
    public async IAsyncEnumerable<TreeNode> ExtractNodes(Stream jsonStream)
    {
        using JsonDocument jsonDocument = await JsonDocument.ParseAsync(jsonStream);

        JsonElement rootElement = jsonDocument.RootElement;
        if (rootElement.ValueKind == JsonValueKind.Null) yield break;
        if (rootElement.ValueKind != JsonValueKind.Object) yield break;
        
        var lp21 = rootElement.GetProperty("lp21");
        if (lp21.ValueKind == JsonValueKind.Null) yield break;
        if (lp21.ValueKind != JsonValueKind.Array) yield break;

        foreach (var item in lp21.EnumerateArray())
        {
            string id = string.Empty;
            if (!item.TryGetProperty("Uid", out JsonElement idElement))
            {
                // root node
            }
            else
            {
                id = idElement.GetString();
                // yield return new Node(idElement.GetString(), "Id", string.Empty);
            }

            if (item.TryGetProperty("hierarchie_unten", out JsonElement urlsElement) && urlsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement urlElement in urlsElement.EnumerateArray())
                {
                    var url = urlElement.GetString();
                    var uriBuilder = new UriBuilder(url);
                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                     // yield return new TreeNode(query["uid"], "", "Url", urlElement.GetString());
                }
            }
            // yield return new TreeNode(id, "Id", string.Empty);
        }


    }
}
