using System.Net;

namespace SharpTwitch.Core.Exceptions
{
    /// <summary>
    /// Unauthorized Http Request Exception
    /// </summary>
    public class UnauthorizedRequestException : HttpRequestException
    {
        public UnauthorizedRequestException(string message) : base(message, null, HttpStatusCode.Unauthorized) 
        {
        }

        public UnauthorizedRequestException(string message, Exception exception) : base(message, exception, HttpStatusCode.Unauthorized)
        {
        }
    }
}
