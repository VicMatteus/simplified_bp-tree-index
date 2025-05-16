namespace simplified_BP_tree_index;

public class CommandProcessor
{
    private string _inFile;
    private string _outFile;
    private BPTree _bpTree;
    private string _result;

    public CommandProcessor(string inFile, string outFile)
    {
        _inFile = inFile;
        _outFile = outFile;
        _bpTree = new BPTree();
    }
    
    public void ExecuteAll()
    {
        //Read and execute all commands in the inFile
        using StreamReader reader = new StreamReader(_inFile);
        string line = reader.ReadLine();
        string order = line.Split("/")[1];

        if (!line.StartsWith("FLH/") || order == "")
        {
            throw new Exception("Formato inválido para a primeira linha. Esperado: FLH/ordem");
        }

        _bpTree.Order = Int32.Parse(order);

        line = reader.ReadLine();
        while (line != null)
        {
            this.ExecuteNext(line);
            //Escrever ou salvar resultado!

            line = reader.ReadLine();
        }
        Console.WriteLine($"Resultados oficiais:\n {_result}");
        File.WriteAllText(_outFile, _result);
    }

    public void ExecuteNext(string line)
    {
        //execute next command in the inFile
        Command command = Command.FromString(line);
        if (command.Op == Operation.INC)
        {
            //bptree deve incluir o valor na árvore
            _result += _bpTree.Insert(command.Key) + "\n";
        }
        else
        {
            //bptree deve buscar o valor na árvore
            _result += _bpTree.Search(command.Key) + "\n";
        }
    }
}