namespace ProjetoGrafos.Core.Interfaces
{
    public interface IVertice
    {
        int Id { get; set;}
        int? Peso { get; set; }
        string? Rotulo { get; set; }
    }
}