using AutomationInfrastructure.Utils;
using Microsoft.Playwright;
using System;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutomationInfrastructure.ApiServices
{
    public class MediaWikiApiClient
    {
        private readonly IAPIRequestContext _apiContext;
        private readonly string _pageTitle;

        public MediaWikiApiClient(IAPIRequestContext apiContext, string pageTitle = "Playwright_(software)")
        {
            _apiContext = apiContext ?? throw new ArgumentNullException(nameof(apiContext));
            _pageTitle = string.IsNullOrWhiteSpace(pageTitle) ? throw new ArgumentNullException(nameof(pageTitle)) : pageTitle;
        }

        public async Task<string> ExtractDebuggingFeaturesTextFromApi()
        {
            // 1) find section index for "Debugging features"
            string sectionIndex = await GetSectionIndexByTitleAsync("Debugging features");
            if (string.IsNullOrEmpty(sectionIndex))
                throw new InvalidOperationException("Section 'Debugging features' not found.");

            // 2) request the section HTML
            var resp = await _apiContext.GetAsync($"w/api.php?action=parse&page={Uri.EscapeDataString(_pageTitle)}&section={sectionIndex}&prop=wikitext&format=json");
            var body = await resp.TextAsync();

            using var doc = JsonDocument.Parse(body);
            if (!doc.RootElement.TryGetProperty("parse", out var parseElem) ||
                !parseElem.TryGetProperty("wikitext", out var textElem) || 
                !textElem.TryGetProperty("*", out var wikitextElem)) 
            {
                return string.Empty;
            }

            string wikitext = wikitextElem.GetString() ?? string.Empty;

            return TextProcessor.NormalizeWikitext(wikitext);
        }

        private async Task<string?> GetSectionIndexByTitleAsync(string sectionTitle)
        {
            var resp = await _apiContext.GetAsync($"w/api.php?action=parse&page={Uri.EscapeDataString(_pageTitle)}&prop=sections&format=json");
            var body = await resp.TextAsync();

            using var doc = JsonDocument.Parse(body);
            if (!doc.RootElement.TryGetProperty("parse", out var parseElem) ||
                !parseElem.TryGetProperty("sections", out var sectionsElem))
            {
                return null;
            }

            foreach (var s in sectionsElem.EnumerateArray())
            {
                if (s.TryGetProperty("line", out var lineElem))
                {
                    var line = lineElem.GetString() ?? string.Empty;
                    if (string.Equals(line.Trim(), sectionTitle, StringComparison.OrdinalIgnoreCase))
                    {
                        if (s.TryGetProperty("index", out var indexElem))
                            return indexElem.GetString();
                    }
                }
            }

            return null;
        }
      
    }
}
