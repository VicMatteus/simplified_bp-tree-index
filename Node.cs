namespace simplified_BP_tree_index;

//Represents a node in the BP-tree
public class Node
{
    public string Id {get; set;}
    public bool IsLeaf {get; set;}
    public List<int> Keys { get; set; } = new List<int>();
    public List<string> Children { get; set; } = new List<string>(); //they're strings caus they reference other nodes
    public List<List<Reference>> RegistersReferences { get; set; } = new List<List<Reference>>(); //only used for leaves
    public string NextLeaf {get; set;}
}