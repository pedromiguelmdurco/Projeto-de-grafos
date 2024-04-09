using System.Text;
using ProjetoGrafos.Core.Interfaces;
using ProjetoGrafos.Core.Models;
using ProjetoGrafos.Helpers;

namespace ProjetoGrafos.Core.Implementations
{
    public class ListaAdjacencia<TVertice, TAresta> : IGrafo<TVertice, Aresta>
    where TVertice : IVertice
    where TAresta : IAresta
    {
        private Dictionary<TVertice, List<Aresta>> listaAdjacencia;
        private readonly Func<TVertice> _criadorVertice;

        public ListaAdjacencia(Func<TVertice> criadorVertice)
        {
            _criadorVertice = criadorVertice;
            listaAdjacencia = new Dictionary<TVertice, List<Aresta>>();
        }

        public ListaAdjacencia(int numeroVertices, Func<TVertice> criadorVertice)
        {
            _criadorVertice = criadorVertice;
            InicializarListaAdjacencia(numeroVertices);
        }

        private void InicializarListaAdjacencia(int numeroVertices)
        {
            listaAdjacencia = new Dictionary<TVertice, List<Aresta>>();
            AdicionarVertices(numeroVertices, _criadorVertice);
        }

        #region Manipulações

        public void AdicionarVertices(int numeroVertices, Func<TVertice> criadorVertice)
        {
            for (int i = 1; i <= numeroVertices; i++)
            {
                var vertice = criadorVertice();
                vertice.Id = i;
                listaAdjacencia[vertice] = new List<Aresta>();
            }
        }

        public void AdicionarAresta(Aresta aresta)
        {
            TVertice verticeOrigem = (TVertice)aresta.Origem;
            TVertice verticeDestino = (TVertice)aresta.Destino;

            // Verifica se os vértices de origem e destino existem
            if (!listaAdjacencia.ContainsKey(verticeOrigem))
            {
                Console.WriteLine($"Aviso: Vértice de origem com ID {verticeOrigem.Id} não encontrado. Aresta não adicionada.");
                return;
            }
            if (!listaAdjacencia.ContainsKey(verticeDestino))
            {
                Console.WriteLine($"Aviso: Vértice de destino com ID {verticeDestino.Id} não encontrado. Aresta não adicionada.");
                return;
            }

            // Verifica se a aresta já existe
            if (listaAdjacencia[verticeOrigem].Any(a => a.Origem.Equals(aresta.Origem) && a.Destino.Equals(aresta.Destino)))
            {
                Console.WriteLine($"Aviso: Aresta já existe entre os vértices {verticeOrigem.Id} e {verticeDestino.Id}. Aresta não adicionada.");
                return;
            }

            // Adiciona a aresta
            listaAdjacencia[verticeOrigem].Add(aresta);
        }

        public void RemoverAresta(Aresta aresta)
        {
            if (listaAdjacencia.ContainsKey((TVertice)aresta.Origem))
                listaAdjacencia[(TVertice)aresta.Origem].Remove(aresta);
        }

        public void PonderarVertice(TVertice vertice, int peso)
        {
            if (listaAdjacencia.ContainsKey(vertice))
                vertice.Peso = peso;
        }

        public void RotularVertice(TVertice vertice, string rotulo)
        {
            if (listaAdjacencia.ContainsKey(vertice))
                vertice.Rotulo = rotulo;
        }

        public void PonderarAresta(Aresta aresta, int peso)
        {
            if (ExisteAresta(aresta))
                aresta.Peso = peso;
        }

        public void RotularAresta(Aresta aresta, string rotulo)
        {
            if (ExisteAresta(aresta))
                aresta.Rotulo = rotulo;
        }
        #endregion

        #region Checagem
        public bool VerticesSaoAdjacentes(TVertice v1, TVertice v2)
        {
            return listaAdjacencia.ContainsKey(v1) && listaAdjacencia[v1].Any(aresta => aresta.Destino.Equals(v2));
        }

        public bool ArestasSaoAdjacentes(Aresta a1, Aresta a2)
        {
            return listaAdjacencia.ContainsKey((TVertice)a1.Origem) &&
                listaAdjacencia[(TVertice)a1.Origem].Any(aresta => aresta.Destino.Equals((TVertice)a2.Origem));
        }

        public bool ArestaIncideEmVertice(Aresta aresta, TVertice vertice) =>
            aresta.Origem.Equals(vertice) || aresta.Destino.Equals(vertice);

        public bool ExisteAresta(Aresta aresta) =>
            listaAdjacencia.ContainsKey((TVertice)aresta.Origem) &&
            listaAdjacencia[(TVertice)aresta.Origem].Contains(aresta);

        public int QuantidadeVertices() =>
            listaAdjacencia.Count;

        public int QuantidadeArestas() =>
            listaAdjacencia.Values.Sum(arestas => arestas.Count);

        public bool EhVazio() =>
            listaAdjacencia.Count == 0;

        public bool EhCompleto()
        {
            int n = QuantidadeVertices();

            foreach (var lista in listaAdjacencia.Values)
            {
                if (lista.Count != n - 1) return false;
            }

            return true;
        }

        public IEnumerable<TVertice> VizinhancaVertice(TVertice vertice) =>
            listaAdjacencia.ContainsKey(vertice) ?
                listaAdjacencia[vertice].Select(aresta => (TVertice)aresta.Destino) :
                Enumerable.Empty<TVertice>();
        #endregion

        #region Pajek NET   
        public void ExportarParaPajekNET()
        {
            string pathDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string nomeArquivo = "lista_pajek.net";

            string pathCompleto = Path.Combine(pathDocumentos, nomeArquivo);

            using (var writer = new StreamWriter(pathCompleto, false, Encoding.ASCII))
            {
                // Escreve vértices
                writer.WriteLine("*Vertices " + listaAdjacencia.Count);
                foreach (var vertice in listaAdjacencia.Keys)
                {
                    string rotulo = !string.IsNullOrEmpty(vertice.Rotulo) ? $" \"{vertice.Rotulo}\"" : "";
                    writer.WriteLine($"{vertice.Id}{rotulo}");
                }

                // Escreve arestas
                writer.WriteLine("*Edges");
                foreach (var kvp in listaAdjacencia)
                {
                    foreach (var aresta in kvp.Value)
                    {
                        writer.WriteLine($"{aresta.Origem.Id} {aresta.Destino.Id}");
                    }
                }
            }

            Console.WriteLine($"Arquivo Pajek NET exportado para {pathCompleto}");
        }

        public void ImportarParaPajekNET(string nomeArquivo)
        {
            string caminhoRaiz = ArquivosExternos.ObterDiretorioProjeto()
                                 ?? throw new Exception("Não foi possível obter o caminho atual.");

            List<string> linhasArquivo = File.ReadLines($"{caminhoRaiz}/Arquivos/{nomeArquivo}")
                .ToList();

            bool processandoVertices = false;
            bool processandoArestas = false;

            foreach (var linha in linhasArquivo)
            {
                if (linha.StartsWith("*Vertices", StringComparison.OrdinalIgnoreCase))
                {
                    processandoVertices = true;
                    processandoArestas = false;
                    continue;
                }

                if (linha.StartsWith("*Edges", StringComparison.OrdinalIgnoreCase))
                {
                    processandoVertices = false;
                    processandoArestas = true;
                    continue;
                }

                if (linha.Trim().Length == 0 || linha.StartsWith("*"))
                    continue;

                if (processandoVertices)
                {
                    var partes = linha.Split(' ');
                    var verticeId = int.Parse(partes[0]);
                    var vertice = new Vertice(verticeId) { Rotulo = partes.Length > 1 ? partes[1].Trim('"') : null };
                    listaAdjacencia.Add((TVertice)(object)vertice, new List<Aresta>());
                }

                if (processandoArestas)
                {
                    var partes = linha.Split(' ');
                    var origemId = int.Parse(partes[0]);
                    var destinoId = int.Parse(partes[1]);
                    var origem = listaAdjacencia.Keys.First(v => v.Id == origemId);
                    var destino = listaAdjacencia.Keys.First(v => v.Id == destinoId);
                    var aresta = new Aresta(origem, destino);
                    listaAdjacencia[origem].Add(aresta);
                }
            }
        }

        #endregion

        public TVertice ObterVerticePorId(int id) =>
            listaAdjacencia.Keys.First(vertice => vertice.Id == id);
    }
}