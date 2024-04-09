namespace ProjetoGrafos.Core.Interfaces
{
    public interface IAresta
    {
        IVertice Origem { get; }
        IVertice Destino { get; }
        int? Peso { get; set; }
        string? Rotulo { get; set; }
    }
}