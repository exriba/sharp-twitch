namespace SharpTwitch.Core.Enums
{
    /// <summary>
    /// Twitch broadcaster types.
    /// </summary>
    public enum BroadcasterType
    {
        NORMAL,
        AFFILIATE,
        PARTNER
    }

    public static class BroadcasterTypeExtensions
    {
        /// <summary>
        /// Converts a broadcaster type into a string.
        /// </summary>
        /// <param name="broadcasterType">broadcaster type</param>
        /// <returns>The string value of the broadcaster type</returns>
        /// <exception cref="ArgumentException">If broadcaster type is invalid</exception>
        public static string ConvertToString(this BroadcasterType broadcasterType)
        {
            return broadcasterType switch
            {
                BroadcasterType.NORMAL => "",
                BroadcasterType.AFFILIATE => "affiliate",
                BroadcasterType.PARTNER => "partner",
                _ => throw new ArgumentException("Invalid Broadcaster Type", nameof(broadcasterType)),
            };
        }
    }
}
