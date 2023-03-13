using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class SubscriptionTypeTests
    {
        [Fact]
        public void SubscriptionType_ConvertToString()
        {
            var values = (SubscriptionType[])Enum.GetValues(typeof(SubscriptionType));
            var enumValues = values.Select(x => x.ConvertToString());

            foreach (var value in values)
            {
                var enumValue = value.ConvertToString();
                Assert.Contains(enumValue, enumValues);
            }
        }
    }
}
