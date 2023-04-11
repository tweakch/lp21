using System.Text.Json;
using CMI.Crawler.Lehrplan21.Models;

namespace CMI.Crawler.Lehrplan21;

public class NodeProcessor : INodeProcessor
{
    public async ValueTask<TreeNode?> ProcessNodeAsync(Stream node)
    {
        // copy stream and write to string
        using var streamReader = new StreamReader(node);

        var json = await streamReader.ReadToEndAsync();
        try 
        {

        // deserialize json
        var response = JsonSerializer.Deserialize<ApiResponse>(json);
        
        if(response == null)
        {
            return null;
        }

        if(response.Lp21 == null)
        {
            return null;
        }

        var lp21 = response.Lp21.FirstOrDefault();
        if(lp21 == null)
        {
            return null;
        }

        // create new treeNode
        var treeNode = new TreeNode(lp21.Uid, lp21.Bezeichnung,json, lp21.HierarchieUnten.Select(uri => uri.ToString()).ToList());
        
        // return treeNode
        return treeNode;
        }
        catch (Exception ex){
            Console.WriteLine(json);
            Console.WriteLine(ex.Message);
            return null;
            
        }
    }
}
