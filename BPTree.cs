namespace simplified_BP_tree_index;

//Represents the BP-tree. Coordinate the search and insert operations.
public class BPTree
{
    private static bool UseDiskC { get; set; } = true;
    private static string dataDirectory = UseDiskC ? @"C:\temp\bptree" : Directory.GetCurrentDirectory();
    
    public int Order {get; set;}
    public string Root { get; set; } = "root";
    public string IndexPath { get; set; } = Path.Combine(dataDirectory, "index.json");
    private string DataFile { get; set; } = Path.Combine(dataDirectory, "vinhos.csv");
    public bool IsEmpty { get; set; } = true;
    public int NodeCount { get; set; } = 0;

    public void CreateDirectory()
    {
        try
        {
            Directory.CreateDirectory(dataDirectory);
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro na criação dos diretórios da árvore.");
            throw;
        }
    }

    private string GenerateNodeId()
    {
        NodeCount++;
        return $"N{NodeCount.ToString()}";
    }
    
    public void Insert(string key)
    {
        int lineOffset = 1;
        string line = "";
        string[] partLine;
        List<Reference> referencies = new List<Reference>();
        
        //Insert the key in the index file
        if (IsEmpty)
        {
            //Busca nos dados pra ver se o valor existe e monta uma lista com as referencias
            using StreamReader reader = new StreamReader(DataFile);
            line = reader.ReadLine(); //Read the header
            
            while ((line = reader.ReadLine()) != null)
            {
                partLine = line.Split(';');
                if (partLine[2] == key)
                {
                    referencies.Add(new Reference(partLine[0], lineOffset));
                }
                lineOffset++;
                line = reader.ReadLine();
            }
            
            //Apenas insere
            Node node = new Node();
            node.Id = GenerateNodeId();
            node.IsLeaf = true;
            node.Keys.Add(int.Parse(key));
            node.RegistersReferences.Add(referencies);

            JsonIndexStructure indexStructure;
            indexStructure = new JsonIndexStructure() //If the index file not exists, create a new one
            {
                root = node.Id,
                order = Order
            };
            
            indexStructure.SaveNode(node, IndexPath);
            
            Root = node.Id;
            IsEmpty = false;
        }
        else
        {
            //faz a busca nó a nó e depois insere
        }
    }
    
    public List<int> Search(string key)
    {
        //Look up for the key in the index file
        if (IsEmpty)
        {
            return new List<int>();
        }
        else
        {
            //Faz a busca nó a nó
            return new List<int>();
        }
    }
    
}