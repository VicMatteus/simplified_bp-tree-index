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
            CommandProcessor commandProcessor = new CommandProcessor(@"C:\temp\bptree\in.txt", @"C:\temp\bptree\out.txt");
            commandProcessor.ExecuteAll();
        }
    }
}