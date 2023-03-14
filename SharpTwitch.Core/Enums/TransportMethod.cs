namespace SharpTwitch.Core.Enums
{
    /// <summary>
    /// Twitch EventSub Transport Methods.
    /// </summary>
    public enum TransportMethod
    {
        WEBHOOK,
        WEBSOCKET,
    }

    public static class TransportMethodExtensions
    {
        /// <summary>
        /// Converts a transport method into a string.
        /// </summary>
        /// <param name="transportMethod">transport method</param>
        /// <returns>The string value of the transport method</returns>
        /// <exception cref="ArgumentException">If transport method is invalid</exception>
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
