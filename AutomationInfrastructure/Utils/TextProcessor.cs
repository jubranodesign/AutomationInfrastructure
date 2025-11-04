using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace AutomationInfrastructure.Utils
{

    public static class TextProcessor
    {
        public static string NormalizeText(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText)) return string.Empty;

            string cleaned = rawText;

            cleaned = Regex.Replace(cleaned, @"<ref[^>]*>.*?<\/ref>", " ", RegexOptions.Singleline);
            cleaned = Regex.Replace(cleaned, @"<ref[^>]*\/>", " ");
            cleaned = Regex.Replace(cleaned, @"={2,}\s*[^=]+\s*={2,}", " ");
            cleaned = Regex.Replace(cleaned, @"{{[^}]+}}", " ");
            cleaned = WebUtility.HtmlDecode(cleaned);
            cleaned = Regex.Replace(cleaned, @"\[\d+\]", " ");
            cleaned = cleaned.ToLowerInvariant();
            cleaned = Regex.Replace(cleaned, @"[^a-z\s]", " ");
            cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();

            return cleaned;
        }

        public static int CountUniqueWords(string normalizedText)
        {
            if (string.IsNullOrWhiteSpace(normalizedText))
                return 0;

            // Split the string into a list of words by single space
            string[] words = normalizedText.Split(' ');

            // Use a HashSet to keep only one instance of each word
            HashSet<string> uniqueWords = new HashSet<string>(words);

            // Return the final count
            return uniqueWords.Count;
        }

        // Backwards-compatible wrapper
        public static int NormalizeAndCountUniqueWords(string rawText)
        {
            var normalized = NormalizeText(rawText);
            return CountUniqueWords(normalized);
        }
    }
}
