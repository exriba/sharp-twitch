using SharpTwitch.Core.Extensions;
using System.Text.Json;

namespace SharpTwitch.Core.NamingPolicies
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToSnakeCase();
        }
    }
}
