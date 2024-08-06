using System.Globalization;
using System.Text;

namespace Pendu.GameStart{
    /// <summary>
    /// Provides functionality to normalize by removing diacritcical marks and converting to uppercase.
    /// </summary>
public static class WordNormalizer
{
    public static string NormalizeString(string input)
    {
        string normalizedString = input.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToUpper(); 
    }
}
}
