// See https://aka.ms/new-console-template for more information
using System;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // List<string[]> fullFile = ReadFileToMemory("../../../vinhos.csv");
            // fullFile.RemoveAt(0);
            //fullFile.Sort(CompareHarvestYear); 
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