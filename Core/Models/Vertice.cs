using ProjetoGrafos.Core.Interfaces;

namespace ProjetoGrafos.Core.Models
{
    public class Vertice : IVertice
    {
        public int Id { get; set; }
        public int? Peso { get; set; }
        public string? Rotulo { get; set; }

        public Vertice(int id, int? peso = null, string? rotulo = null)
        {
            Id = id;
            Peso = peso;
            Rotulo = rotulo;
        }

        public Vertice()
        {
        }
    }
}