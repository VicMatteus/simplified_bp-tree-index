namespace simplified_BP_tree_index;

public class Reference
{
    public string WineId {get; set;}
    public int LineOffset {get; set;}

    public Reference(string wineId, int lineOffset)
    {
        WineId = wineId;
        LineOffset = lineOffset;
    }
    
    public string tostring()
    {
        return WineId + ";" + LineOffset;
    }
}