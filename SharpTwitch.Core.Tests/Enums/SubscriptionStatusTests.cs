using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class SubscriptionStatusTests
    {
        [Fact]
        public void SubscriptionStatus_ConvertToString()
        {
            var values = (SubscriptionStatus[])Enum.GetValues(typeof(SubscriptionStatus));
            var enumValues = values.Select(x => x.ConvertToString());

            foreach (var value in values)
            {
                var enumValue = value.ConvertToString();
                Assert.Contains(enumValue, enumValues);
            }
        }
    }
}
