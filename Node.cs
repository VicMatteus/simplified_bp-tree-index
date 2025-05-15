namespace simplified_BP_tree_index;

//Represents a node in the BP-tree
public class Node
{
    public string Id {get; set;}
    public bool IsLeaf {get; set;}
    public List<int> Keys {get; set;}
    public List<int> Children {get; set;}
    public List<List<Reference>> RegistersReferences {get; set;}
    public string NextLeaf {get; set;}


    public Node LoadNode(String id)
    {
        //Loads the node from the disk
        return new Node();
    }
}