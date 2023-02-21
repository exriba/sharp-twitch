namespace SharpTwitch.Core.Enums
{
    public enum TransportMethod
    {
        WEBHOOK,
        WEBSOCKET,
    }

    public static class TransportMethodExtensions
    {
        public static string ConvertToString(this TransportMethod transportMethod)
        {
            return transportMethod switch
            {
                TransportMethod.WEBHOOK => "webhook",
                TransportMethod.WEBSOCKET => "websocket",
                _ => throw new ArgumentException("Invalid Transport Method", nameof(transportMethod)),
            };
        }
    }
}
