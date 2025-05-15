namespace simplified_BP_tree_index;

//Represents a node in the BP-tree
public class Node
{
    public bool IsLeaf {get; set;}
    public List<int> Keys {get; set;}
    public List<int> Children {get; set;}
    public List<List<int>> RegistersReferences {get; set;}
    public string NextLeaf {get; set;}

    //Save the node on disk
    public void SaveNode(String id)
    {
        
    }

    //Loads the node from the disk
    public Node LoadNode(String id)
    {
        return new Node();
    }
}