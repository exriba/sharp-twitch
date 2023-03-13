using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class QueryParameterTests
    {
        [Fact]
        public void QueryParameter_ConvertToString()
        {
            var values = (QueryParameter[])Enum.GetValues(typeof(QueryParameter));
            var enumValues = values.Select(x => x.ConvertToString());

            foreach (var value in values)
            {
                var enumValue = value.ConvertToString();
                Assert.Contains(enumValue, enumValues);
            }
        }
    }
}
