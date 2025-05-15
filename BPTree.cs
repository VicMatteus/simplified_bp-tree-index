namespace simplified_BP_tree_index;

//Represents the BP-tree. Coordinate the search and insert operations.
public class BPTree
{
    public string Root {get; set;}
    public int Order {get; set;}
    public string IndexPath {get; set;}
    
    //Insert the key in the index file
    public void Insert(string key)
    {
        
    }
    
    //Look up for the key in the index file
    public void Search(string key)
    {
        
    }

    public Node LoadNode(string key)
    {
        return new Node().LoadNode(key);
    }

    public void SaveNode(string key)
    {
        new Node().SaveNode(key);
    }
}