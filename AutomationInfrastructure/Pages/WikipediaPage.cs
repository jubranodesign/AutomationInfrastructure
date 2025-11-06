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
            var pLocator = _page.Locator(
                "xpath=//div[h3[@id='Debugging_features']]/following-sibling::p[1]"
             );

            var ulLocator = _page.Locator(
                "xpath=//div[h3[@id='Debugging_features']]/following-sibling::ul[1]"
            );

            var pText = await pLocator.TextContentAsync() ?? string.Empty;
            var liTexts = await ulLocator.Locator("li").AllTextContentsAsync();
            var ulText = string.Join(" ", liTexts.Select(t => t.Trim()));

            return $"{pText} {ulText}";
        }
        public async Task<IReadOnlyList<string>> GetTestingAndDebuggingToolLinks()
        {
            var allRowsLocator = _page.Locator("table.navbox-inner tr");

            var specificRowLocator = allRowsLocator.Filter(new LocatorFilterOptions
            {
                Has = _page.Locator("th:has-text('debugging')")
            }).First;

            if (await specificRowLocator.CountAsync() == 0)
            {
                Console.WriteLine("DEBUG: Specific TR for 'Testing and debugging' was NOT found.");
                return new List<string>();
            }

            var toolsContainerLocator = specificRowLocator.Locator("td").First;

            if (await toolsContainerLocator.CountAsync() == 0)
            {
                Console.WriteLine("DEBUG: TD element not found within the located TR.");
                return new List<string>();
            }

            var linkLocators = toolsContainerLocator.Locator("a[href]");

            return await linkLocators.AllTextContentsAsync();
        }
    }
}
