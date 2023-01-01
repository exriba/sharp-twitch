namespace SharpTwitch.Core.Enums
{
    public enum QueryParameter
    {
        BROADCASTER_ID,
        ONLY_MANAGEABLE_REWARDS
    }

    public static class QueryParameterExtensions
    {
        public static string ConvertToString(this QueryParameter parameter)
        {
            return parameter switch
            {
                QueryParameter.BROADCASTER_ID => "broadcaster_id",
                QueryParameter.ONLY_MANAGEABLE_REWARDS => "only_manageable_rewards",
                _ => throw new ArgumentException("Invalid Query Parameter", nameof(parameter)),
            };
        }
    }
}
