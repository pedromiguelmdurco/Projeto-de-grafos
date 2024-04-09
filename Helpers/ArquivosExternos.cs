namespace ProjetoGrafos.Helpers;

public static class ArquivosExternos
{
    public static string? ObterDiretorioProjeto()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        DirectoryInfo? directoryInfo = new(currentDirectory);

        while (directoryInfo != null && !VerificarSeDiretorioPossuiArquivo(directoryInfo.FullName, "ProjetoGrafos.csproj"))
        {
            directoryInfo = directoryInfo.Parent;
        }

        return directoryInfo?.FullName;
    }
    
    private static bool VerificarSeDiretorioPossuiArquivo(string diretorio, string arquivoParaEncontrar)
    {
        return Directory.GetFiles(diretorio, arquivoParaEncontrar, SearchOption.TopDirectoryOnly).Length > 0;
    }
}