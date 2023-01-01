using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class UrlFragmentTests
    {
        [Fact]
        public void UrlFragment()
        {
            var values = (UrlFragment[]) Enum.GetValues(typeof(UrlFragment));
            var urls = values.Select(x => x.ConvertToString());

            foreach (var value in values)
            {
                var url = value.ConvertToString();
                Assert.Contains(url, urls);
            }
        }
    }
}
