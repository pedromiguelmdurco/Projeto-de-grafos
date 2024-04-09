# Biblioteca de Grafos para Algoritmos em Grafos

## Descrição
Este repositório contém uma biblioteca de grafos desenvolvida como parte do trabalho prático para a disciplina de Algoritmos em Grafos. A biblioteca é implementada em C# e oferece funcionalidades para manipulação de grafos, incluindo a criação de grafos, adição e remoção de vértices e arestas, além de suporte a grafos ponderados e rotulados.

## Estrutura do Projeto
O projeto está dividido em várias partes, conforme os requisitos do trabalho prático:

- **Parte 1:** Implementação das estruturas básicas da biblioteca, incluindo representações de grafos por lista de adjacência e matriz de adjacência.
- **Parte 2:** (Em desenvolvimento) Implementação de algoritmos para caminhos mínimos (Dijkstra, Bellman-Ford, Floyd-Warshall) e manipulação adicional.
- **Parte 3:** (A ser definida)

## Como Usar
Para utilizar a biblioteca, inclua o projeto em sua solução .NET e faça referência a ele. Você pode criar um grafo utilizando o `GrafoBuilder` e manipulá-lo usando os métodos fornecidos pelas classes de grafo.

Exemplo básico de uso:

```csharp
// Gerar grafo
var grafo = new GrafoBuilder()
    .ComNumeroVertices(5)
    .ListaAdjacencia()
    .Construir();

// Obter vértices do grafo
var v1 = grafo.ObterVerticePorId(1);
var v2 = grafo.ObterVerticePorId(2);
var v3 = grafo.ObterVerticePorId(3);

// Criar novas arestas
var a1 = new Aresta(v1, v2);
var a2 = new Aresta(v2, v3);

// Adicionar arestas ao grafo
grafo.AdicionarAresta(a1);
grafo.AdicionarAresta(a2);
```

## Colaboradores do Projeto
- Caio Henrique da Silva Bento
- Lucas Gomes Silva
- Pedro Henrique Ramalho de Castro
- Pedro Henrique Trindade Silva
- Pedro Miguel Moraes Durço

