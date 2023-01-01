using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Interfaces
{
    public interface ICoreSettings
    {
        string Secret { get; set; }
        string ClientId { get; set; }
        string RedirectUri { get; set; }
        List<Scope> Scopes { get; set; }
    }
}
