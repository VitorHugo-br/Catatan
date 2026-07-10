using System.Globalization;
using System.Text;

namespace MyBlazorApp.Extensions;

public static class StringExtensions
{
    private static readonly string[] Preposicoes = { "de", "da", "do", "das", "dos", "e" };

    /// <summary>
    /// Retorna as iniciais das duas primeiras palavras significativas do nome
    /// (ignorando preposições como "de", "da", "do", "dos", "das", "e").
    /// Se houver apenas uma palavra válida, retorna somente a inicial dela.
    /// Exemplo: "Vitor Hugo Silva" -> "VH"
    /// Exemplo: "Vitor" -> "V"
    /// Exemplo: "Maria de Souza" -> "MS"
    /// </summary>
    public static string ToInitials(this string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return string.Empty;

        var palavras = nome
            .Trim()
            .Split([' '], StringSplitOptions.RemoveEmptyEntries)
            .Where(p => !Preposicoes.Contains(p.ToLower(CultureInfo.InvariantCulture)))
            .ToArray();

        if (palavras.Length == 0)
            return string.Empty;

        var sb = new StringBuilder();

        // Pega a primeira palavra
        sb.Append(char.ToUpper(palavras[0][0], CultureInfo.InvariantCulture));

        // Pega a segunda palavra, se existir
        if (palavras.Length > 1)
            sb.Append(char.ToUpper(palavras[1][0], CultureInfo.InvariantCulture));

        return sb.ToString();
    }
}
