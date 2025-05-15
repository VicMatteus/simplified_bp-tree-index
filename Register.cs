namespace simplified_BP_tree_index;

//Represents a register from the data.csv
public class Register
{
    //vinho_id;rotulo;ano_colheita;tipo
    private string id { get; set; }
    private string label { get; set; }
    private string harvest_year { get; set; }
    private string type { get; set; }

    public Register(string id, string label, string harvest_year, string type)
    {
        this.id = id;
        this.label = label;
        this.harvest_year = harvest_year;
        this.type = type;
    }

    public static Register FromCSV(string line)
    {
        string[] splittedLine = line.Split(";");
        Register register = new Register(splittedLine[0], splittedLine[1], splittedLine[2], splittedLine[3]);
        return register;
    }

    public override string ToString()
    {
        string register = "";
        return register;
    }
}