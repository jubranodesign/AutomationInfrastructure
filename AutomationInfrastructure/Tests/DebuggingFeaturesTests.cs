using AutomationInfrastructure.ApiServices;
using AutomationInfrastructure.Fixtures;
using AutomationInfrastructure.Pages;
using AutomationInfrastructure.Utils;
using Microsoft.Playwright;
using System;

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
            WikipediaPage wikipediaPage = new WikipediaPage(page);
            var uiText = await wikipediaPage.ExtractTextFromDebuggingFeatures();

            var apiCtx = _fixture.ApiContext ?? throw new InvalidOperationException("ApiContext not initialized");
            // initialize API client with the ApiContext and the target page title
            var apiClient = new MediaWikiApiClient(apiCtx, "Playwright_(software)");
            string apiText = await apiClient.ExtractDebuggingFeaturesTextFromApi();

            int uiCount = TextProcessor.NormalizeAndCountUniqueWords(uiText);
            int apiCount = TextProcessor.NormalizeAndCountUniqueWords(apiText);

            Assert.Equal(apiCount, uiCount);
        }
    }
}