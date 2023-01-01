namespace SharpTwitch.Core.Enums
{
    public enum Header
    {
        CODE,
        AUTHORIZATION_BEARER,
        CONTENT_TYPE_FORM_URL_ENCODED,
        CLIENT_ID,
        CLIENT_SECRET,
        GRANT_TYPE,
        AUTHORIZATION_ACCESS_TOKEN,
        REDIRECT_URI,
        REFRESH_TOKEN,
    }

    public static class HeaderExtensions
    {
        public static KeyValuePair<string, string> Transform(this Header parameter, string value)
        {
            return parameter switch
            {
                Header.CODE => throw new NotImplementedException(),
                Header.AUTHORIZATION_BEARER => new KeyValuePair<string, string>("Authorization", $"Bearer {value}"),
                Header.CONTENT_TYPE_FORM_URL_ENCODED => new KeyValuePair<string, string>("Content-Type", "application/x-www-form-urlencoded"),
                Header.CLIENT_ID => new KeyValuePair<string, string>("Client-Id", value),
                Header.CLIENT_SECRET => throw new NotImplementedException(),
                Header.GRANT_TYPE => throw new NotImplementedException(),
                Header.AUTHORIZATION_ACCESS_TOKEN => new KeyValuePair<string, string>("Authorization", $"OAuth {value}"),
                Header.REDIRECT_URI => throw new NotImplementedException(),
                Header.REFRESH_TOKEN => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
