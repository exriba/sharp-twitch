using SharpTwitch.Core.Extensions;
using System.Text.Json;

namespace SharpTwitch.Core.NamingPolicies
{
    /// <summary>
    /// JSON policy to convert object keys into snake_case.
    /// </summary>
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToSnakeCase();
        }
    }
}
