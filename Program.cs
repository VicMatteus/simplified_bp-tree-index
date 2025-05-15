// See https://aka.ms/new-console-template for more information
using System;
using System.Text.Json;
using simplified_BP_tree_index;

namespace simplified_BP_tree_index
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // List<string[]> fullFile = ReadFileToMemory("../../../vinhos.csv");
            // fullFile.RemoveAt(0);
            //fullFile.Sort(CompareHarvestYear); 
            Register register = new Register()
            {
                Id = "1",
                Label = "rotulo",
                HarvestYear = "2020",
                Type = "tipo 1"
            };
            
            //Teste de diretórios
            Console.WriteLine($"Diretório atual: " + Directory.GetCurrentDirectory());
            Console.WriteLine($"Caminho dos dados: " + Path.Combine(Directory.GetCurrentDirectory(), "data.csv"));
            Console.WriteLine(register.ToJson());

            //Teste da estrutura json
            Node node = new Node()
            {
                Id = "1",
                IsLeaf = true,
                Keys = [1900, 1901],
                RegistersReferences = new List<List<Reference>>()
                {
                    new List<Reference>()
                    {
                        new Reference("1", 1),
                        new Reference("2", 2)
                    }
                }
            };
            Dictionary<string, Node> dict = new Dictionary<string, Node>();
            dict.Add("1914", node);
            
            JsonIndexStructure test = new JsonIndexStructure()
            {
                root = "1",
                order = 2,
                Nodes = dict
            };
            Console.WriteLine(JsonSerializer.Serialize(test));

            
            //Teste de escrita de json
            using (StreamWriter x = File.CreateText(Path.Combine(@"c:\temp\bptree", "index.json")))
            {
                x.WriteLine(JsonSerializer.Serialize(test));
                // x.WriteLine("Teste de escrita 3");
                // x.WriteLine("Teste de escrita 4");
            }
            //Teste de leitura do json
            using (StreamReader reader = new StreamReader(Path.Combine(@"c:\temp\bptree", "index.json")))
            {
                string fullfile = reader.ReadToEnd();
                Console.WriteLine(fullfile);
                JsonIndexStructure json = JsonSerializer.Deserialize<JsonIndexStructure>(fullfile);
                Console.WriteLine(json);
            };
            
            //Teste de escrita mais conciso
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(Path.Combine(@"c:\temp\bptree", "index.json"), JsonSerializer.Serialize(test));
        }
        
        //Lê um arquivo inteiro e retorna uma lista de arrays de string onde cada elemento da lista é uma linha e cada item do array de strings é uma coluna.
        static List<string[]> ReadFileToMemory(string filePath)
        {
            List<string[]> file = new List<string[]>();
            
            StreamReader reader = new StreamReader(filePath);
            string line = reader.ReadLine();
            file.Add(line.Split(";"));

            
            line = reader.ReadLine();
            while(line != null)
            {
                file.Add(line.Split(";"));
                line = reader.ReadLine();
            }

            //return the list with the rows n columns to the caller
            return file;
        }
        
        // Compara duas linhas do arquivos de vinhos para saber qual tem o mais ano (atributo do índice 2). 
        static int CompareHarvestYear(string[] x, string[] y)
        {
            int x_int = 0;
            int y_int = 0;

            if (x[2] == null)
            {
                if (y[2] == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y[2] == null)
                {
                    return 1;
                }
                else
                {
                    x_int = Int32.Parse(x[2]);
                    y_int = Int32.Parse(y[2]);
                    if ( x_int > y_int )
                    {
                        return 1;
                    }
                    else if (x_int == y_int)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
        
    }
}