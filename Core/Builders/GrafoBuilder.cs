using ProjetoGrafos.Core.Implementations;
using ProjetoGrafos.Core.Interfaces;
using ProjetoGrafos.Core.Models;

namespace ProjetoGrafos.Core.Builders
{
    public class GrafoBuilder
    {
        private int numeroVertices;
        private string? _caminhoArquivoPajekNet;
        private TipoRepresentacao representacao;

        public GrafoBuilder ComNumeroVertices(int numero)
        {
            numeroVertices = numero;
            return this;
        }

        public GrafoBuilder ListaAdjacencia()
        {
            representacao = TipoRepresentacao.ListaAdjacencia;
            return this;
        }

        public GrafoBuilder MatrizAdjacencia()
        {
            representacao = TipoRepresentacao.MatrizAdjacencia;
            return this;
        }

        public GrafoBuilder ImportarDePajekNet(string caminhoArquivo)
        {
            _caminhoArquivoPajekNet = caminhoArquivo;
            return this;
        }

        public IGrafo<IVertice, Aresta> Construir()
        {
            if (!string.IsNullOrEmpty(_caminhoArquivoPajekNet))
            {
                if (representacao == TipoRepresentacao.ListaAdjacencia)
                {
                    var grafo = new ListaAdjacencia<IVertice, IAresta>(() => new Vertice());
                    grafo.ImportarParaPajekNET(_caminhoArquivoPajekNet);
                    return grafo;
                }

                if (representacao == TipoRepresentacao.MatrizAdjacencia)
                {
                    var grafo = new MatrizAdjacencia<IVertice, IAresta>(() => new Vertice());
                    grafo.ImportarParaPajekNET(_caminhoArquivoPajekNet);
                    return grafo;
                }

                throw new InvalidOperationException("Tipo de representação não suportado.");
            }

            switch (representacao)
            {
                case TipoRepresentacao.ListaAdjacencia:
                    return new ListaAdjacencia<IVertice, Aresta>(numeroVertices, () => new Vertice());
                case TipoRepresentacao.MatrizAdjacencia:
                    return new MatrizAdjacencia<IVertice, Aresta>(numeroVertices, () => new Vertice());
                default:
                    throw new InvalidOperationException("Tipo de representação não suportado.");
            }
        }
    }

    public enum TipoRepresentacao
    {
        ListaAdjacencia,
        MatrizAdjacencia
    }
}