using System.Text;
using System.Text.Json;
using CMI.Crawler.Lehrplan21.Models;

namespace CMI.Crawler.Lehrplan21.Services;

public class NodeProcessor : INodeProcessor
{
    public async ValueTask<TreeNode?> ProcessNodeAsync(Stream node)
    {
        // copy stream and write to string
        using var streamReader = new StreamReader(node, Encoding.UTF8);
        
        var json = await streamReader.ReadToEndAsync();
        try
        {
            // deserialize json
            var response = JsonSerializer.Deserialize<ApiResponse>(json);

            if (response == null)
            {
                return null;
            }

            if (response.Lp21 == null)
            {
                return null;
            }

            var lp21 = response.Lp21.FirstOrDefault();
            if (lp21 == null)
            {
                return null;
            }

            // create new treeNode
            return new TreeNode(Id: lp21.Uid, Data: lp21, Urls: lp21.HierarchieUnten == null ? Array.Empty<Uri>() : lp21.HierarchieUnten);
        }
        catch (Exception ex)
        {
            Console.WriteLine(json);
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
