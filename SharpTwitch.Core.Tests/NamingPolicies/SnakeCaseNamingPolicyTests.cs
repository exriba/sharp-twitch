using SharpTwitch.Core.NamingPolicies;
using System.Text.Json;

namespace SharpTwitch.Core.Tests.NamingPolicies
{
    public class SnakeCaseNamingPolicyTests
    {
        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };

        private const string NAME = "name";

        [Theory]
        [InlineData(NAME)]
        public void SnakeCaseNamingPolicy_ConvertName(string key)
        {
            var data = new { Name = "Test" };
            var json = JsonSerializer.Serialize(data, JsonSerializerOptions);
            Assert.Contains(key, json);
        }
    }
}
