using System.Text.Json;

namespace simplified_BP_tree_index;

//Represents a register from the data.csv
public class Register
{
    //vinho_id;rotulo;ano_colheita;tipo
    public string Id { get; set; }
    public string Label { get; set; }
    public string HarvestYear { get; set; }
    public string Type { get; set; }
    
    public static Register FromCSV(string line)
    {
        string[] splittedLine = line.Split(";");
        Register register = new Register
        {
             Id = splittedLine[0],
             Label = splittedLine[1],
             HarvestYear = splittedLine[2],
             Type = splittedLine[3]
        };
        return register;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}