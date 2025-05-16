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

    public void ValidateDataFile()
    {
        if (!File.Exists(DataFile))
            throw new FileNotFoundException("Arquivo de dados vinhos.csv não encontrado em " + DataFile);
    }

    private string GenerateNodeId()
    {
        NodeCount++;
        return $"N{NodeCount.ToString()}";
    }
    
    public string Insert(string key)
    {
        int intKey = int.Parse(key);
        bool newRoot = false;
        List<Reference> references = GetReferencesForKey(key);
        if (references.Count == 0)
        {
            //Escrever no out.txt INC:[key]/0, pois chave não existe nos dados.
            Console.WriteLine($"INC:{key}/{references.Count()}");
            return $"INC:{key}/{references.Count()}";
        }
        
        //Insert the key in the index file
        if (IsEmpty)
        {
            Node node = new Node();
            node.Id = GenerateNodeId();
            node.IsLeaf = true;
            node.Keys.Add(intKey);
            node.RegistersReferences.Add(references);

            JsonIndexStructure indexStructure;
            indexStructure = new JsonIndexStructure() //If the index file not exists, create a new one
            {
              root = node.Id,
              order = Order
            };
            
            indexStructure.SaveNode(node, IndexPath);
            Root = node.Id;
            IsEmpty = false;
            Console.WriteLine($"INC:{key}/{references.Count()}");
            return $"INC:{key}/{references.Count()}";
        }
        else
        {
            //faz a busca nó a nó e depois insere
            
            //Primeiro, encontrar o nó em que o registro deve ocorrer
            Node node = GetLeafNodeForKey(key);
            
            //se a chave já existe
            if (node.Keys.Contains(intKey))
            {
                //Como os dados são estáticos, não é necessário atualizar nada, apenas ignorar o comando.
                //Escrever no out.txt INC:[key]/0, pois nada foi atualizado 
                Console.WriteLine($"INC:{key}/0");
                return $"INC:{key}/0";
            }
            else
            {
                bool didSplit = false;
                Node newNode = null;
                Node parentNode = null;
                Node newRootNode = null;
                string splittedNodeKey = "";
                
                JsonIndexStructure indexStructure = JsonIndexStructure.LoadIndexFromDisk(IndexPath);
                
                int insertIndex = 0;
                while (insertIndex < node.Keys.Count && intKey > node.Keys[insertIndex]) //enquanto chave de busca > chave atual, ando até achar uma maior(lugar certo)
                {
                    insertIndex++;
                }

                //Updates the key list, respecting the order
                node.Keys.Insert(insertIndex, intKey);
                
                //updates the references list, respecting the order
                node.RegistersReferences.Insert(insertIndex, references);
                
                
                //Insiro eles ordenados primeiro, para depois trocar de nó, aproveitando a ordenação prévia
                if (node.Keys.Count == Order)
                {
                    //Será necessário criar um novo nó, colocar metade do nó anterior nele assim como suas referências
                    //primeiro: criar um novo nó
                    didSplit = true;
                    newNode = new Node();
                    newNode.Id = GenerateNodeId();
                    newNode.IsLeaf = true;
                    
                    //Separar as chaves entre o nó antigo e o novo nó
                    decimal mid =  Math.Ceiling(Order / 2M);

                    //Armazeno uma chave do nó base para encontrá-lo na busca pelo pai
                    splittedNodeKey = node.Keys[0].ToString();
                    
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
                    
                    //Finalizo o nó original pós split.
                    indexStructure.SaveNode(node, IndexPath);;
                    
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
                        
                        indexStructure.SaveNode(newRootNode, IndexPath);
                        Root = newRootNode.Id;
                        indexStructure.order = Order;
                    }
                    else
                    {
                        //Identificar o pai e inserir nele o novo filho
                        //Isso é carregar um novo nó, então "node" já foi salvo acima
                        parentNode = GetParentNodeForKey(node.Id, splittedNodeKey);
                        
                        //Para manter a ordenação no pai:
                        int parentInsertIndex = 0;
                        while (parentInsertIndex < parentNode.Keys.Count && newParentKey > parentNode.Keys[parentInsertIndex])
                        {
                            parentInsertIndex++;
                        }
                        parentNode.Keys.Insert(parentInsertIndex, newParentKey);
                        parentNode.Children.Insert(parentInsertIndex + 1, newNode.Id);
                        
                        //Salva o nó pai com um filho novo e nova chave
                        indexStructure.SaveNode(parentNode, IndexPath);
                    }

                }
                else
                {
                    indexStructure.SaveNode(node, IndexPath);;
                }
               
                if (didSplit)
                {
                    indexStructure.SaveNode(newNode, IndexPath);
                }
                
                //Escrever no out.txt $"INC:{key}/{references.Count()}"
                Console.WriteLine($"INC:{key}/{references.Count()}");
                return $"INC:{key}/{references.Count()}";
            }
        }
    }
    
    public string Search(string key)
    {
        int intKey = int.Parse(key);
        
        if (IsEmpty)
        {
            Console.WriteLine($"BUS=:{key}/0");
            return $"BUS=:{key}/0";
        }
        
        List<Reference> references = GetReferencesForKey(key);
        if (references.Count == 0)
        {
            //Escrever no out.txt INC:[key]/0, pois chave não existe nos dados.
            Console.WriteLine($"BUS=:{key}/{references.Count()}");
            return $"BUS=:{key}/{references.Count()}";
        }
        else
        {
            //Carrega o nó após uma busca nó a nó
            Node node = GetLeafNodeForKey(key);
            //Procura no nó o índice da chave buscada. com o indice, vai na lista de referencias e conta quantas tem.
            int refIndex = node.Keys.IndexOf(int.Parse(key));
            if (refIndex == -1)
            {
                Console.WriteLine($"BUS=:{key}/0 -> pois o registro existe no banco mas não foi indexado com o comando INC.");
                return $"BUS=:{key}/0";
            }
            Console.WriteLine($"BUS=:{key}/{node.RegistersReferences[node.Keys.IndexOf(int.Parse(key))].Count.ToString()}");
            return $"BUS=:{key}/{node.RegistersReferences[node.Keys.IndexOf(int.Parse(key))].Count.ToString()}";
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

    public Node GetParentNodeForKey(string nodeId, string nodeKey)
    {
        JsonIndexStructure indexStructure = JsonIndexStructure.LoadIndexFromDisk(IndexPath);
        Node currentNode = indexStructure.Nodes[Root];
        int intKey = int.Parse(nodeKey);
        string nextNodeId = "";
        List<string> parentsPath = new List<string>();
        
        while (!currentNode.IsLeaf)
        {
            int i = 0;
            while (i < currentNode.Keys.Count && intKey >= currentNode.Keys[i])
            {
                i++;
            }
            parentsPath.Add(currentNode.Id);
            nextNodeId = currentNode.Children[i]; 
            currentNode = indexStructure.Nodes[nextNodeId]; //Pega o próximo nó, garantindo apenas um nó manipulado por vez para chegar à folha.
        }

        foreach (string id in parentsPath)
        {
            if (id == nodeId)
            {
                return indexStructure.Nodes[id];
            } 
        }
        throw new Exception($"Nó pai não encontrado para o nó {nodeId} com chave {nodeKey}");
        return null;
    }
    
    public List<Reference> GetReferencesForKey(string key)
    {
        ValidateDataFile();
        int lineOffset = 1;
        string line = "";
        string[] partLine;
        List<Reference> references = new List<Reference>();
        
        //Search the data looking for the key. if exists, adds the reference to the list.
        using StreamReader reader = new StreamReader(DataFile);
        line = reader.ReadLine(); //Read the header
        
        while ((line = reader.ReadLine()) != null)
        {
            partLine = line.Split(';');
            if (partLine[2] == key)
            {
                references.Add(new Reference(partLine[0], lineOffset));
            }
            lineOffset++;
        }

        return references;
    }
}