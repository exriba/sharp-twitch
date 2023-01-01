using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class QueryParameterTests
    {
        [Fact]
        public void QueryParameter()
        {
            var values = (QueryParameter[])Enum.GetValues(typeof(QueryParameter));
            var queryParameters = values.Select(x => x.ConvertToString());

            foreach (var value in values)
            {
                var queryParameter = value.ConvertToString();
                Assert.Contains(queryParameter, queryParameters);
            }
        }
    }
}
