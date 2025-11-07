using AutomationInfrastructure.ApiServices;
using AutomationInfrastructure.Fixtures;
using AutomationInfrastructure.Pages;
using AutomationInfrastructure.Utils;
using Microsoft.Playwright;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace AutomationInfrastructure
{
    [Collection("Playwright collection")]

    public class DebuggingFeaturesTests
    {
        private readonly PlaywrightFixture _fixture;

        public DebuggingFeaturesTests(PlaywrightFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task Verify_Unique_Word_Count_UI_Equals_API()
        {
            IPage page = await (_fixture.Browser ?? throw new InvalidOperationException("Browser not initialized")).NewPageAsync();
            await page.GotoAsync("https://en.wikipedia.org/wiki/Playwright_(software)");
            WikipediaPlaywrightPage wikipediaPlaywrightPage = new WikipediaPlaywrightPage(page);
            var uiText = await wikipediaPlaywrightPage.ExtractTextFromDebuggingFeaturesAsync();

            var apiCtx = _fixture.ApiContext ?? throw new InvalidOperationException("ApiContext not initialized");
            // initialize API client with the ApiContext and the target page title
            var apiClient = new MediaWikiApiClient(apiCtx, "Playwright_(software)");
            string apiText = await apiClient.GetSectionContentByTitleAsync("Debugging features");

            int uiCount = TextProcessor.NormalizeAndCountUniqueWords(uiText);
            int apiCount = TextProcessor.NormalizeAndCountUniqueWords(apiText);

            Assert.Equal(apiCount, uiCount);

            await page.CloseAsync();
        }

        [Fact]
        public async Task TechnologyNames_InTestingAndDebuggingRow_MustBeLinks()
        {
            IPage page = await (_fixture.Browser ?? throw new InvalidOperationException("Browser not initialized")).NewPageAsync();
            await page.GotoAsync("https://en.wikipedia.org/wiki/Playwright_(software)");
            WikipediaPlaywrightPage wikipediaPlaywrightPage = new WikipediaPlaywrightPage(page);

            var toolNames = await wikipediaPlaywrightPage.GetTestingAndDebuggingToolLinksAsync();

            Assert.NotEmpty(toolNames);

            var expectedTools = new List<string>
        {
            "CodeView",
            "OneFuzz",
            "Playwright",
            "Script Debugger",
            "WinDbg",
            "xUnit.net"
        };

            Assert.Equal(
                expectedTools.OrderBy(t => t),
                toolNames.OrderBy(t => t)
            );

            await page.CloseAsync();
        }

        [Fact]
        public async Task Settings_ShouldChangeThemeToDark()
        {
            IPage page = await (_fixture.Browser ?? throw new InvalidOperationException("Browser not initialized")).NewPageAsync();
            await page.GotoAsync("https://en.wikipedia.org/wiki/Playwright_(software)");

            var darkOptionLocator = page.GetByLabel("Dark");
            await darkOptionLocator.ClickAsync();

            const string ExpectedRgbColor = "rgb(32, 33, 34)";
            const string ElementToInspect = "body";

            var actualBackgroundColor = await page.EvaluateAsync<string>(
           $"element => window.getComputedStyle(document.querySelector('{ElementToInspect}')).backgroundColor",
           ElementToInspect);

            Assert.Equal(ExpectedRgbColor, actualBackgroundColor, StringComparer.OrdinalIgnoreCase);

            await page.CloseAsync();
        }
    }
}