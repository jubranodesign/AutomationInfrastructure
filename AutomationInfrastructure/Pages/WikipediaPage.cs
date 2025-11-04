using Microsoft.Playwright;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutomationInfrastructure.Pages
{
    public class WikipediaPage
    {
        private readonly IPage _page;

        public WikipediaPage(IPage page) => _page = page;

        public async Task<string> ExtractTextFromDebuggingFeatures()
        {
            // wait for the span/h3 to appear
            await _page.WaitForSelectorAsync("#Debugging_features", new PageWaitForSelectorOptions { Timeout = 15000 });

            // evaluate in page: find the parent div that contains the h3/span and walk siblings
            var raw = await _page.EvaluateAsync<string>(@"() => {
                const anchor = document.getElementById('Debugging_features');
                if (!anchor) return '';
                // the h3 is typically inside a div with class 'mw-heading' — use that as the container
                const container = anchor.closest('div.mw-heading') || anchor.closest('h1,h2,h3,h4,h5,h6');
                if (!container) return '';
                const parts = [];
                let node = container.nextElementSibling;
                while (node) {
                    // stop when we reach the next heading container or a heading element
                    if (node.matches && (node.matches('div.mw-heading') || /^H[1-6]$/i.test(node.tagName))) break;

                    if (node.matches && node.matches('p')) {
                        const t = node.innerText && node.innerText.trim();
                        if (t) parts.push(t);
                    } else if (node.matches && node.matches('ul')) {
                        const items = Array.from(node.querySelectorAll('li')).map(li => li.innerText.trim()).filter(Boolean);
                        if (items.length) parts.push(items.join(' '));
                    }

                    node = node.nextElementSibling;
                }
                return parts.join(' ');
            }");

            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
           
            return raw;
        }
    }
}
