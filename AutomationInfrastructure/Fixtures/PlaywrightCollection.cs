using AutomationInfrastructure.Fixtures;
using Xunit;

[CollectionDefinition("Playwright collection")]
public class PlaywrightCollection : ICollectionFixture<PlaywrightFixture>
{
    // empty — xUnit uses this to wire the fixture to the collection name
}