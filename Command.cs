namespace simplified_BP_tree_index;

//Represents a command received from the in.txt
public class Command
{
    public enum Operation
    {
        INC,
        BUS
    }

    private Operation op { get; set; }
    private string key { get; set; }

    public Command(Operation op, string key)
    {
        this.op = op;
        this.key = key;
    }

    public Command FromString(string line)
    {
        Operation opAux = line.Contains("BUS") ? Operation.BUS : Operation.INC;
        string keyAux = line.Substring(line.IndexOf(":") + 1);

        return new Command(op, key);
    }
}