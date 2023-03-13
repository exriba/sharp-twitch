using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class ScopeTests
    {
        [Fact]
        public void Scope_ConvertToString()
        {
            var values = (Scope[])Enum.GetValues(typeof(Scope));
            var enumValues = values.Select(x => x.ConvertToString());

            foreach (var value in values)
            {
                var enumValue = value.ConvertToString();
                Assert.Contains(enumValue, enumValues);
            }
        }
    }
}
