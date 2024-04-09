using ProjetoGrafos.Core.Builders;
using ProjetoGrafos.Core.Interfaces;
using ProjetoGrafos.Core.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        // Grafo "manual"
        // var grafo = new GrafoBuilder()
        //     .ComNumeroVertices(5)
        //     .ListaAdjacencia()
        //     .Construir();

        //Grafo importado do Pajek
        var grafo = new GrafoBuilder()
            .ImportarDePajekNet("pajek.net")
            .MatrizAdjacencia()
            .Construir();

        TestarGrafo(ref grafo);
    }

    private static void TestarGrafo(ref IGrafo<IVertice, Aresta> grafo)
    {
        // Obtendo vértices do grafo
        var v1 = grafo.ObterVerticePorId(1);
        var v2 = grafo.ObterVerticePorId(2);
        var v3 = grafo.ObterVerticePorId(3);
        var v4 = grafo.ObterVerticePorId(4);
        
        // Criação de arestas
        var a1 = new Aresta(v1, v2);
        var a2 = new Aresta(v2, v3);
        var a3 = new Aresta(v1, v3);
        var a4 = new Aresta(v1, v4);
        
        // Adicionando arestas ao grafo
        grafo.AdicionarAresta(a1);
        grafo.AdicionarAresta(a2);
        grafo.AdicionarAresta(a3);
        grafo.AdicionarAresta(a4);
        
        // Testando manipulações
        grafo.PonderarVertice(v1, 10);
        grafo.RotularVertice(v2, "RotuloV2");
        grafo.PonderarAresta(a1, 5);
        grafo.RotularAresta(a2, "RotuloA2");
        grafo.RotularAresta(a3, "RotuloA3");
        
        grafo.ExportarParaPajekNET();

        // Testando checagens
        Console.WriteLine($"Vertices {v1.Id} e {v2.Id} são adjacentes: {grafo.VerticesSaoAdjacentes(v1, v2)}");
        Console.WriteLine($"Arestas A1 e A2 são adjacentes: {grafo.ArestasSaoAdjacentes(a1, a2)}");
        Console.WriteLine($"Aresta A1 incide no vértice {v1.Id}: {grafo.ArestaIncideEmVertice(a1, v1)}");
        Console.WriteLine($"Existe aresta A1: {grafo.ExisteAresta(a1)}");
        Console.WriteLine($"Quantidade de vértices: {grafo.QuantidadeVertices()}");
        Console.WriteLine($"Quantidade de arestas: {grafo.QuantidadeArestas()}");
        Console.WriteLine($"Grafo está vazio: {grafo.EhVazio()}");
        Console.WriteLine($"Grafo é completo: {grafo.EhCompleto()}");

        // Vizinhança do vértice v1
        Console.WriteLine($"Vizinhança do vértice {v1.Id}:");
        foreach (var vizinho in grafo.VizinhancaVertice(v1))
        {
            Console.WriteLine($"Vértice {vizinho.Id}");
        }
    }
}