namespace SharpTwitch.Core.Enums
{
    /// <summary>
    /// Headers.
    /// </summary>
    public enum Header
    {
        CODE,
        AUTHORIZATION_BEARER,
        CONTENT_TYPE_FORM_URL_ENCODED,
        CONTENT_TYPE_APPLICATION_JSON,
        CLIENT_ID,
        CLIENT_SECRET,
        GRANT_TYPE,
        AUTHORIZATION_ACCESS_TOKEN,
        REDIRECT_URI,
        REFRESH_TOKEN,
    }

    public static class HeaderExtensions
    {
        /// <summary>
        /// Converts a header into a string.
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>The string value of the header</returns>
        /// <exception cref="ArgumentException">If header is invalid</exception>
        public static string ConvertToString(this Header header)
        {
            return header switch
            {
                Header.CODE => throw new NotImplementedException(),
                Header.AUTHORIZATION_BEARER => throw new NotImplementedException(),
                Header.CONTENT_TYPE_FORM_URL_ENCODED => "application/x-www-form-urlencoded",
                Header.CONTENT_TYPE_APPLICATION_JSON => "application/json",
                Header.CLIENT_ID => throw new NotImplementedException(),
                Header.CLIENT_SECRET => throw new NotImplementedException(),
                Header.GRANT_TYPE => throw new NotImplementedException(),
                Header.AUTHORIZATION_ACCESS_TOKEN => throw new NotImplementedException(),
                Header.REDIRECT_URI => throw new NotImplementedException(),
                Header.REFRESH_TOKEN => throw new NotImplementedException(),
                _ => throw new ArgumentException("Invalid Header", nameof(header)),
            };
        }

        /// <summary>
        /// Converts a header into proper http header key value pairs.
        /// </summary>
        /// <param name="header">header</param>
        /// <param name="value">value</param>
        /// <returns>The http header key value pairs</returns>
        /// <exception cref="NotImplementedException">If header transformation is not implemented/exception>
        public static KeyValuePair<string, string> Transform(this Header header, string value)
        {
            return header switch
            {
                Header.CODE => throw new NotImplementedException(),
                Header.AUTHORIZATION_BEARER => new KeyValuePair<string, string>("Authorization", $"Bearer {value}"),
                Header.CONTENT_TYPE_FORM_URL_ENCODED => new KeyValuePair<string, string>("Content-Type", "application/x-www-form-urlencoded"),
                Header.CONTENT_TYPE_APPLICATION_JSON => new KeyValuePair<string, string>("Content-Type", "application/json"),
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
