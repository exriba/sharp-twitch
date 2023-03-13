using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class TransportMethodTests
    {
        [Fact]
        public void TransportMethod_ConvertToString()
        {
            var values = (TransportMethod[])Enum.GetValues(typeof(TransportMethod));
            var enumValues = values.Select(x => x.ConvertToString());

            foreach (var value in values)
            {
                var enumValue = value.ConvertToString();
                Assert.Contains(enumValue, enumValues);
            }
        }
    }
}
