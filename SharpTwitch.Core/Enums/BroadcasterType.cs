namespace SharpTwitch.Core.Enums
{
    public enum BroadcasterType
    {
        NORMAL,
        AFFILIATE,
        PARTNER
    }

    public static class BroadcasterTypeExtensions
    {
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
