using ProjetoGrafos.Core.Interfaces;

namespace ProjetoGrafos.Core.Models
{
    public class Aresta : IAresta
    {
        public IVertice Origem { get; private set; }
        public IVertice Destino { get; private set; }
        public int? Peso { get; set; }
        public string? Rotulo { get; set; }

        public Aresta(IVertice origem, IVertice destino, int? peso = null, string? rotulo = null)
        {
            Origem = origem;
            Destino = destino;
            Peso = peso;
            Rotulo = rotulo;
        }
    }
}