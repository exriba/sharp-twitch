using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class ScopeTests
    {
        [Fact]
        public void Scope()
        {
            var values = (Scope[])Enum.GetValues(typeof(Scope));
            var scopes = values.Select(x => x.ConvertToString());

            foreach (var value in values)
            {
                var scope = value.ConvertToString();
                Assert.Contains(scope, scopes);
            }
        }
    }
}
