using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Interfaces;

namespace SharpTwitch.Core.Configuration
{
    public class CoreSettings : ICoreSettings
    {
        public const string Key = "CoreSettings";

        public string Secret { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public List<Scope> Scopes { get; set; }
    }
}
