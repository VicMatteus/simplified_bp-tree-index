namespace simplified_BP_tree_index;

//Represents a node in the BP-tree
public class Node
{
    private bool isLeaf {get; set;}
    private List<int> keys {get; set;}
    private List<int> children {get; set;}
    private string nextLeaf {get; set;}
    private List<List<int>> registersReferences {get; set;}

    public Node()
    {
        
    }
    
}