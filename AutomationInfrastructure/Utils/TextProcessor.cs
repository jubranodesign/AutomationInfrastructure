using System.Net;
using System.Text.RegularExpressions;

namespace AutomationInfrastructure.Utils
{

    public static class TextProcessor
    {
        public static string NormalizeText(string rawText)
        {
            string normalized = rawText.ToLowerInvariant();

            // Remove punctuation, square brackets and numbers (e.g. [14]) using a regular expression
            // [^a-z\s] matches any character that is not a letter (a-z) and not whitespace (\s)
            normalized = Regex.Replace(normalized, @"[^a-z\s]", " ");

            // Replace multiple whitespace characters with a single space
            normalized = Regex.Replace(normalized, @"\s+", " ").Trim();

            return normalized;
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

        public static string NormalizeWikitext(string wikitext)
        {
            if (string.IsNullOrWhiteSpace(wikitext)) return string.Empty;

            var cleaned = Regex.Replace(wikitext, @"<ref[^>]*>.*?<\/ref>", " ", RegexOptions.Singleline);

            cleaned = Regex.Replace(cleaned, @"<ref[^>]*\/>", " ");

            cleaned = Regex.Replace(cleaned, @"={2,}\s*[^=]+\s*={2,}", " ");

            cleaned = Regex.Replace(cleaned, @"{{[^}]+}}", " ");

            cleaned = WebUtility.HtmlDecode(cleaned);

            cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();

            return cleaned;
        }
    }
}
