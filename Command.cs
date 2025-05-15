namespace simplified_BP_tree_index;
//Represents a command received from the in.txt

public enum Operation
{
    INC,
    BUS
}
public class Command
{
    public Operation Op { get; set; }
    public string Key { get; set; }

    public Command(Operation op, string key)
    {
        this.Op = op;
        this.Key = key;
    }

    public static Command FromString(string line)
    {
        Operation opAux;
        
        if (line.Contains("BUS"))
        {
            opAux = Operation.BUS;
        }
        else if (line.Contains("INC"))
        {
            opAux = Operation.INC;
        }
        else
        {
            throw new ArgumentException($"Comando inválido: linha {line}");
        }
        string keyAux = line.Substring(line.IndexOf(":") + 1);

        return new Command(opAux, keyAux);
    }
}