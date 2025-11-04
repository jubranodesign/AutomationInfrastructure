using Microsoft.Playwright;
using System.Text.Json;

namespace AutomationInfrastructure.Fixtures
{
    public class PlaywrightFixture : IAsyncLifetime
    {
        public IPlaywright? Playwright { get; private set; }
        public IBrowser? Browser { get; private set; }
        public IAPIRequestContext? ApiContext { get; private set; }

        public async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            ApiContext = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {
                BaseURL = "https://en.wikipedia.org/"
            });

            Console.WriteLine("TestCollectionFixture Initialized.");
        }

        public async Task DisposeAsync()
        {
            try
            {
                if (ApiContext != null)
                    await ApiContext.DisposeAsync().AsTask();
                if (Browser != null)
                    await Browser.CloseAsync();
            }
            finally
            {
                Playwright?.Dispose();
                Console.WriteLine("TestCollectionFixture Disposed.");
            }
        }
    }
}