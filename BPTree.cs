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
        bool newRoot = false;
        List<Reference> referencies = GetReferencesForKey(key);
        if (referencies.Count == 0)
        {
            //Escrever no out.txt INC:[key]/0, pois chave não existe nos dados. 
            return;
        }
        
        //Insert the key in the index file
        if (IsEmpty)
        {
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
            
            //Primeiro, encontrar o nó em que o registro deve ocorrer
            Node node = GetLeafNodeForKey(key);
            
            //se a chave já existe
            if (node.Keys.Contains(int.Parse(key)))
            {
                //Como os dados são estáticos, não é necessário atualizar nada, apenas ignorar o comando.
                //Escrever no out.txt INC:[key]/0, pois nada foi atualizado 
            }
            else
            {
                bool split = false;
                Node newNode = null;
                Node parentNode = null;
                Node newRootNode = null;
                JsonIndexStructure indexStructure = JsonIndexStructure.LoadIndexFromDisk(IndexPath);
                
                int insertIndex = 0;
                while (insertIndex < node.Keys.Count && int.Parse(key) > node.Keys[insertIndex]) //enquanto chave de busca > chave atual, ando até achar uma maior(lugar certo)
                {
                    insertIndex++;
                }

                //Updates the key list, respecting the order
                node.Keys.Insert(insertIndex, int.Parse(key));
                
                //updates the references list, respecting the order
                node.RegistersReferences.Insert(insertIndex, referencies);
                
                
                //Insiro eles ordenados primeiro, para depois trocar de nó, aproveitando a ordenação prévia
                if (node.Keys.Count == Order)
                {
                    //Será necessário criar um novo nó, colocar metade do nó anterior nele assim como suas referências
                    //primeiro: criar um novo nó
                    split = true;
                    newNode = new Node();
                    newNode.Id = GenerateNodeId();
                    newNode.IsLeaf = true;
                    
                    //Separar as chaves entre o nó antigo e o novo nó
                    decimal mid =  Math.Ceiling(Order / 2M);

                    //Manipulando apenas node, pois newNode é auxiliar e ainda não entrou no índice
                    for (int i = (int) mid; i < node.Keys.Count; i++)
                    {
                        //Trago itens do meio para o fim do nó antigo para o nó novo
                        newNode.Keys.Add(node.Keys[i]);
                        newNode.RegistersReferences.Add(node.RegistersReferences[i]);
                    }
                    //Removo itens do nó antigo
                    node.Keys.RemoveRange((int)mid, node.Keys.Count - (int)mid);
                    node.RegistersReferences.RemoveRange((int)mid, node.RegistersReferences.Count - (int)mid);
                    
                    //Agora deve promover o menor elemento do nó novo à pai
                    int newParentKey = newNode.Keys[0];
                    
                    //Identificar se o nó atual era raiz. Se for, precisa criar mais um nó
                    if (node.Id == Root)
                    {
                        newRoot = true;
                        newRootNode = new Node();
                        newRootNode.Id = GenerateNodeId();
                        newRootNode.IsLeaf = false;
                        newRootNode.Keys.Add(newParentKey);
                        newRootNode.Children.Add(node.Id);
                        newRootNode.Children.Add(newNode.Id);
                        
                        Root = newRootNode.Id;
                    }
                    else
                    {
                        //Identificar o pai e inserir nele o novo filho
                        string parentNodeId = GetParentNodeId(newNode.Id);
                        parentNode = indexStructure.Nodes[parentNodeId];
                        parentNode.Children.Add(newParentKey.ToString());
                    }

                }
                //Save the nodes: node(old, splitted), newNode(newborn), parentNode(the new root or the original parent node)
                indexStructure.SaveNode(node, IndexPath);;
                if (split)
                {
                    indexStructure.SaveNode(newNode, IndexPath);
                    indexStructure.SaveNode(newRoot ? newRootNode : parentNode, IndexPath);;
                };
                //Escrever no out.txt INC:[key]/referencies.Count()
            }
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


    public Node GetLeafNodeForKey(string key)
    {
        JsonIndexStructure indexStructure = JsonIndexStructure.LoadIndexFromDisk(IndexPath);
        Node currentNode = indexStructure.Nodes[Root];
        int intKey = int.Parse(key);
        string nextNodeId = "";
        
        while (!currentNode.IsLeaf)
        {
            //Percorre todas as chaves do nó atual, verificando se a chave de busca é >= do que a atual. Se for, para e descobre a posição da filha.
            //A medida que compara, o ponteiro do filho também anda. Primeiro, ele começa no zero, ou seja, se key for menor que a primeira chave do nó atual
            //o filho zero é o proximo ponteiro que precisa ser carregado. Se não, avança o ponteiro do filho(1) e avança a chave do nó atual, até que a quantidade acabe.
            int i = 0;
            while (i < currentNode.Keys.Count && intKey >= currentNode.Keys[i])
            {
                i++;
            }

            nextNodeId = currentNode.Children[i]; //Esse é o próximo nó que precisa ser carregado.
            currentNode = indexStructure.Nodes[nextNodeId]; //Pega o próximo nó, garantindo apenas um nó manipulado por vez para chegar à folha.
        }

        return currentNode;
    }
    
    //Revisar
    public string? GetParentNodeId(string childNodeId)
    {
        if (childNodeId == Root)
            return null; // A raiz não tem pai

        JsonIndexStructure index = JsonIndexStructure.LoadIndexFromDisk(IndexPath);
        Node currentNode = index.Nodes[Root];

        while (!currentNode.IsLeaf)
        {
            foreach (string child in currentNode.Children)
            {
                if (child == childNodeId)
                    return currentNode.Id; // Achou o pai
            }

            // Descobre o próximo filho que deve conter o nó desejado
            // Aqui a gente assume que os nomes dos nós não carregam a ordem, então seguimos baseado nas chaves
            int i = 0;
            while (i < currentNode.Keys.Count && CompareNodeId(childNodeId, currentNode.Children[i]) > 0)
            {
                i++;
            }

            if (i >= currentNode.Children.Count)
                i = currentNode.Children.Count - 1;

            currentNode = index.Nodes[currentNode.Children[i]];
        }

        return null; // Se não encontrar (não era pra acontecer)
    }
    //Revisar
    private int CompareNodeId(string a, string b)
    {
        int numA = int.Parse(new string(a.SkipWhile(c => !char.IsDigit(c)).ToArray()));
        int numB = int.Parse(new string(b.SkipWhile(c => !char.IsDigit(c)).ToArray()));
        return numA.CompareTo(numB);
    }
    
    public List<Reference> GetReferencesForKey(string key)
    {
        int lineOffset = 1;
        string line = "";
        string[] partLine;
        List<Reference> referencies = new List<Reference>();
        
        //Search the data looking for the key. if exists, adds the reference to the list.
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

        return referencies;
    }
}