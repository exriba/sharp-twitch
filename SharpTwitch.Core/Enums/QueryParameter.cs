namespace SharpTwitch.Core.Enums
{
    /// <summary>
    /// Query Parameters
    /// </summary>
    public enum QueryParameter
    {
        ID,
        BROADCASTER_ID,
        ONLY_MANAGEABLE_REWARDS
    }

    public static class QueryParameterExtensions
    {
        /// <summary>
        /// Converts a query parameter into a string.
        /// </summary>
        /// <param name="queryParameter">query parameter</param>
        /// <returns>The string value of the query parameter</returns>
        /// <exception cref="ArgumentException">If query parameter is invalid</exception>
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
