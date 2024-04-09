using System.Text;
using ProjetoGrafos.Core.Interfaces;
using ProjetoGrafos.Core.Models;
using ProjetoGrafos.Helpers;

namespace ProjetoGrafos.Core.Implementations
{
    public class MatrizAdjacencia<TVertice, TAresta> : IGrafo<TVertice, Aresta>
        where TVertice : IVertice
        where TAresta : IAresta
    {
        private bool[,] matriz;
        private Dictionary<int, TVertice> vertices;
        private readonly Func<TVertice> _criadorVertice;

        private Dictionary<(int, int), Aresta> arestas;

        public MatrizAdjacencia(Func<TVertice> criadorVertice)
        {
            _criadorVertice = criadorVertice;
        }

        public MatrizAdjacencia(int numeroVertices, Func<TVertice> criadorVertice)
        {
            _criadorVertice = criadorVertice;
            InicializarMatrizAdjacencia(numeroVertices);
        }

        private void InicializarMatrizAdjacencia(int numeroVertices)
        {
            matriz = new bool[numeroVertices, numeroVertices];
            vertices = new Dictionary<int, TVertice>();
            arestas = new Dictionary<(int, int), Aresta>();
            AdicionarVertices(numeroVertices, _criadorVertice);
        }

        #region Manipulações

        public void AdicionarVertices(int numeroVertices, Func<TVertice> criadorVertice)
        {
            for (int i = 0; i < numeroVertices; i++)
            {
                var vertice = criadorVertice();
                vertice.Id = i + 1;
                vertices.Add(vertice.Id, vertice);
            }
        }

        public void AdicionarAresta(Aresta aresta)
        {
            int origemId = aresta.Origem.Id;
            int destinoId = aresta.Destino.Id;

            // Verifica se os IDs dos vértices são válidos
            if (!vertices.ContainsKey(origemId))
            {
                Console.WriteLine($"Aviso: Vértice de origem com ID {origemId} não encontrado. Aresta não adicionada.");
                return;
            }
            if (!vertices.ContainsKey(destinoId))
            {
                Console.WriteLine($"Aviso: Vértice de destino com ID {destinoId} não encontrado. Aresta não adicionada.");
                return;
            }

            // Marca a presença da aresta na matriz de adjacência
            matriz[origemId - 1, destinoId - 1] = true;

            // Cria uma chave para identificar a aresta
            var chaveAresta = (origemId, destinoId);

            // Verifica se a aresta já existe no dicionário
            if (arestas.ContainsKey(chaveAresta))
            {
                Console.WriteLine($"Aviso: Aresta já existe entre os vértices {origemId} e {destinoId}. Aresta não adicionada.");
                return;
            }

            // Adiciona a aresta no dicionário
            arestas.Add(chaveAresta, aresta);
        }

        public void RemoverAresta(Aresta aresta)
        {
            int origemId = aresta.Origem.Id;
            int destinoId = aresta.Destino.Id;

            if (vertices.ContainsKey(origemId) && vertices.ContainsKey(destinoId))
                matriz[origemId - 1, destinoId - 1] = false;
        }

        public void PonderarVertice(TVertice vertice, int peso)
        {
            if (vertices.ContainsKey(vertice.Id))
                vertices[vertice.Id].Peso = peso;
        }

        public void RotularVertice(TVertice vertice, string rotulo)
        {
            if (vertices.ContainsKey(vertice.Id))
                vertices[vertice.Id].Rotulo = rotulo;
        }

        public void PonderarAresta(Aresta aresta, int peso)
        {
            var chave = (aresta.Origem.Id, aresta.Destino.Id);

            if (arestas.ContainsKey(chave))
                arestas[chave].Peso = peso;
        }

        public void RotularAresta(Aresta aresta, string rotulo)
        {
            var chave = (aresta.Origem.Id, aresta.Destino.Id);

            if (arestas.ContainsKey(chave))
                arestas[chave].Rotulo = rotulo;
        }
        #endregion

        #region Checagem

        public bool VerticesSaoAdjacentes(TVertice v1, TVertice v2)
        {
            return matriz[v1.Id - 1, v2.Id - 1];
        }

        public bool ArestasSaoAdjacentes(Aresta a1, Aresta a2)
        {
            return matriz[a1.Destino.Id - 1, a2.Origem.Id - 1];
        }

        public bool ArestaIncideEmVertice(Aresta aresta, TVertice vertice)
        {
            return aresta.Origem.Equals(vertice) || aresta.Destino.Equals(vertice);
        }

        public bool ExisteAresta(Aresta aresta)
        {
            int origemId = aresta.Origem.Id;
            int destinoId = aresta.Destino.Id;

            return matriz[origemId - 1, destinoId - 1];
        }

        public int QuantidadeVertices() => vertices.Count;

        public int QuantidadeArestas()
        {
            int totalArestas = 0;

            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    if (matriz[i, j])
                        totalArestas++;
                }
            }

            return totalArestas;
        }

        public bool EhVazio() => QuantidadeVertices() == 0;

        public bool EhCompleto()
        {
            int n = QuantidadeVertices();

            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    // Ignora diagonal principal e verifica se existe alguma aresta que não está preenchida
                    if (i != j && !matriz[i, j])
                        return false;
                }
            }

            return true;
        }

        public IEnumerable<TVertice> VizinhancaVertice(TVertice vertice)
        {
            var vizinhos = new List<TVertice>();

            // Percorre coluna da matriz
            for (int i = 0; i < matriz.GetLength(1); i++)
            {
                // Se forem adjacentes, adiciona na lista
                if (matriz[vertice.Id - 1, i])
                    vizinhos.Add(vertices[i + 1]);
            }

            return vizinhos;
        }
        #endregion

        public void ExportarParaPajekNET()
        {
            string pathDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string nomeArquivo = "matriz_pajek.net";

            string pathCompleto = Path.Combine(pathDocumentos, nomeArquivo);

            using (var writer = new StreamWriter(pathCompleto, false, Encoding.ASCII))
            {
                // Escreve vértices
                writer.WriteLine($"*Vertices {QuantidadeVertices()}");
                foreach (var vertice in vertices)
                {
                    string rotulo = string.IsNullOrEmpty(vertice.Value.Rotulo) ? "" : $"\"{vertice.Value.Rotulo}\"";
                    writer.WriteLine($"{vertice.Value.Id} {rotulo}");
                }

                // Escreve arestas
                writer.WriteLine($"*Edges {QuantidadeArestas()}");
                foreach (var aresta in arestas)
                {
                    string rotulo = string.IsNullOrEmpty(aresta.Value.Rotulo) ? "" : $"\"{aresta.Value.Rotulo}\"";
                    writer.WriteLine($"{aresta.Value.Origem.Id} {aresta.Value.Destino.Id} {aresta.Value.Peso} {rotulo}");
                }
            }

            Console.WriteLine($"Exported Pajek NET file to {pathCompleto}");
        }

        #region Pajek NET
        public void ImportarParaPajekNET(string nomeArquivo)
        {
            string caminhoRaiz = ArquivosExternos.ObterDiretorioProjeto()
                                 ?? throw new Exception("Não foi possível obter o caminho atual.");

            List<string> linhasArquivo = File.ReadLines($"{caminhoRaiz}/Arquivos/{nomeArquivo}")
                .ToList();

            CriarGrafoPorArquivoPajekNet(linhasArquivo);
        }

        private void CriarGrafoPorArquivoPajekNet(IReadOnlyList<string> linhasArquivo)
        {
            bool calculandoArestas = false;
            foreach (var linha in linhasArquivo)
            {
                if (linha.StartsWith("*Vertices", StringComparison.OrdinalIgnoreCase))
                {
                    int numVertices = int.Parse(linha.Split(" ")[1]);
                    InicializarMatrizAdjacencia(numVertices);
                    continue;
                }

                if (linha.StartsWith("*Edges", StringComparison.OrdinalIgnoreCase))
                {
                    calculandoArestas = true;
                    continue;
                }

                if (linha.Trim().Length == 0 || linha.StartsWith("*"))
                    continue;

                if (!calculandoArestas)
                {
                    var dadosVertice = linha.Split(new[] { ' ' }, 2);
                    int idVertice = int.Parse(dadosVertice[0]);
                    string rotulo = dadosVertice.Length > 1 ? dadosVertice[1].Trim('"') : string.Empty;

                    TVertice vertice = ObterVerticePorId(idVertice);
                    vertice.Rotulo = rotulo;
                }
                else
                {
                    var dadosAresta = linha.Split(' ');
                    int verticeOrigemId = int.Parse(dadosAresta[0]);
                    int verticeDestinoId = int.Parse(dadosAresta[1]);
                    int? peso = dadosAresta.Length > 2 && int.TryParse(dadosAresta[2], out var parsedPeso) ? parsedPeso : null;
                    string? rotulo = dadosAresta.Length > 3 ? dadosAresta[3].Trim('"') : null;

                    TVertice verticeOrigem = ObterVerticePorId(verticeOrigemId);
                    TVertice verticeDestino = ObterVerticePorId(verticeDestinoId);
                    Aresta aresta = new(verticeOrigem, verticeDestino)
                    {
                        Peso = peso,
                        Rotulo = rotulo
                    };

                    AdicionarAresta(aresta);
                }
            }
        }
        #endregion

        public TVertice ObterVerticePorId(int id)
        {
            if (vertices.TryGetValue(id, out var vertice))
                return vertice;
            else
                throw new KeyNotFoundException($"Não foi encontrado um vértice com o ID {id}.");
        }
    }
}
