using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Settings
{
    /// <summary>
    /// Default implementation of ICoreSettings. (Configuration Class)
    /// </summary>
    public class CoreSettings : ICoreSettings
    {
        public const string Key = "CoreSettings";

        public string Secret { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public List<Scope> Scopes { get; set; } = new List<Scope>();
    }
}
