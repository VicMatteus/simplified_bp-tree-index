## Objetivo: 
Implementar uma estrutura de árvore B+ simplificada em C#, com persistência em disco. 
O índice será armazenado em um arquivo index.json, enquanto os registros completos residirão em um arquivo vinho.csv. 
A árvore suportará inserções e buscas exatas, simulando o funcionamento básico de um índice de SGBD, com foco em clareza didática 
e fidelidade conceitual à estrutura da B+ Tree.

## Como usar:
Arquivo principal: ````Program.cs````
Após configurar os caminhos, basta instanciar na main um objeto CommandProcessor, como já está exemplificado.

```csharp
 CommandProcessor commandProcessor = new CommandProcessor(@"C:\temp\bptree\in.txt", @"C:\temp\bptree\out.txt");
```

Após instanciado, basta usar o método ````ExecuteAll()````

```csharp
 commandProcecssor.ExecuteAll()
```
Após a build do projeto o executável estará em: <br> 
```pasta_do_projeto\bin\Debug\net9.0\simplified_BP-tree_index.exe```


## Funcionamento base:
A classe Command Processor é responsável por processar o in.txt e passar os comandos para a BPTree. Esta, por sua vez, é 
responsável por executar a lógica da inserção e busca, manipulando nós um por vez. O índice e os nós são gravados em disco pela 
JsonIndexStructure. <br>

Após o processamento, BPTree retorna o resultado para Command Processor que grava o out.txt.
Após a execução, out.txt e index.json estarão disponíveis para consulta no caminho informado pelo usuário.<br>

Para melhor leitura do index.json recomendo copiar seu conteúdo e colar em: https://jsonformatter.org/json-viewer

## Caminhos importantes:
Duas classes precisam de caminhos bem definidos.
- **CommandProcessor**: requer 2 caminhos na instanciação
  - in.txt(primeiro parâmetro do construtor) - caminho absoluto para obtenção dos comandos que serão processados.
    - ex: "C:\temp\bptree\in.txt"
  - out.txt(segundo parâmetro do construtor) - caminho absoluto para geração do out.txt com resultados.
    - ex: "C:\temp\bptree\out.txt"
- **BPTree**: está pré-definida com 2 caminhos
  - "C:\temp\bptree\index.json" - caminho para geração do arquivo de índice. (Tente rodar a IDE com privilégios de administrador).
  - "C:\temp\bptree\vinhos.csv" - caminho para obtenção dos dados do csv.
  - **Obs**: caso queira ou precise alterar, mude no arquivo BPTree.cs na linha 7. Troque apenas o conteúdo que está entre aspas.

 O projeto não cria o sistema de pastas, então, por gentileza, crie antes de executar.