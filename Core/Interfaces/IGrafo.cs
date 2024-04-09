using ProjetoGrafos.Core.Models;

namespace ProjetoGrafos.Core.Interfaces
{
    public interface IGrafo<TVertice, TAresta>
     where TVertice : IVertice
     where TAresta : IAresta
    {
        //Manipulações
        void AdicionarVertices(int numeroVertices, Func<TVertice> criadorVertice);
        void AdicionarAresta(Aresta aresta);
        void RemoverAresta(Aresta aresta);
        void PonderarVertice(TVertice vertice, int peso);
        void RotularVertice(TVertice vertice, string rotulo);
        void PonderarAresta(Aresta aresta, int peso);
        void RotularAresta(Aresta aresta, string rotulo);

        //Checagens
        bool VerticesSaoAdjacentes(TVertice v1, TVertice v2);
        bool ArestasSaoAdjacentes(Aresta a1, TAresta a2);
        bool ArestaIncideEmVertice(Aresta aresta, TVertice vertice);
        bool ExisteAresta(Aresta aresta);
        int QuantidadeVertices();
        int QuantidadeArestas();
        bool EhVazio();
        bool EhCompleto();
        IEnumerable<TVertice> VizinhancaVertice(TVertice vertice);
        void ExportarParaPajekNET();
        void ImportarParaPajekNET(string nomeArquivo);

        TVertice ObterVerticePorId(int id);
    }
}