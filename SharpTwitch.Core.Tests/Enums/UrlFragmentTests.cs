using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class UrlFragmentTests
    {
        [Fact]
        public void UrlFragment_ConvertToString()
        {
            var values = (UrlFragment[])Enum.GetValues(typeof(UrlFragment));
            var enumValues = values.Select(x => x.ConvertToString());

            foreach (var value in values)
            {
                var enumValue = value.ConvertToString();
                Assert.Contains(enumValue, enumValues);
            }
        }
    }
}
