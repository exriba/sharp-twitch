using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class BroadcasterTypeTests
    {
        [Fact]
        public void BroadcasterTypeTests_ConvertToString()
        {
            var values = (BroadcasterType[])Enum.GetValues(typeof(BroadcasterType));
            var enumValues = values.Select(x => x.ConvertToString());

            foreach (var value in values)
            {
                var enumValue = value.ConvertToString();
                Assert.Contains(enumValue, enumValues);
            }
        }
    }
}
