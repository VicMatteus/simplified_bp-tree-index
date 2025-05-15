using System.Text.Json;

namespace simplified_BP_tree_index;

public class JsonIndexStructure
{
    public string root {get; set;}
    public int order {get; set;}
    public Dictionary<string, Node> Nodes { get; set; } = new();


    public static JsonIndexStructure LoadIndexFromDisk(string indexPath)
    {
        if (!File.Exists(indexPath) == false)
        {
            return new JsonIndexStructure();
        }
        
        using (StreamReader reader = new StreamReader(indexPath))
        {
            string fullFile = reader.ReadToEnd();
            return JsonSerializer.Deserialize<JsonIndexStructure>(fullFile);
        };
    }
    
    public void SaveNode(Node node, string indexPath)
    {
        //Save the node on the index
        Nodes[node.Id] = node;
        
        //save the index on disk
        var options = new JsonSerializerOptions { WriteIndented = true };
        // using (StreamWriter writer = new StreamWriter(indexPath))
        // {
        //     writer.WriteLine(JsonSerializer.Serialize(this));
        //     writer.Flush();
        // }
        
        //Testar pra ver se funfa
        File.WriteAllText(indexPath, JsonSerializer.Serialize(this));
    }
}