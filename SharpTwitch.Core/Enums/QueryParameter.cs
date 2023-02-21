namespace SharpTwitch.Core.Enums
{
    public enum QueryParameter
    {
        ID,
        BROADCASTER_ID,
        ONLY_MANAGEABLE_REWARDS
    }

    public static class QueryParameterExtensions
    {
        public static string ConvertToString(this QueryParameter queryParameter)
        {
            return queryParameter switch
            {
                QueryParameter.ID => "id",
                QueryParameter.BROADCASTER_ID => "broadcaster_id",
                QueryParameter.ONLY_MANAGEABLE_REWARDS => "only_manageable_rewards",
                _ => throw new ArgumentException("Invalid Query Parameter", nameof(queryParameter)),
            };
        }
    }
}
