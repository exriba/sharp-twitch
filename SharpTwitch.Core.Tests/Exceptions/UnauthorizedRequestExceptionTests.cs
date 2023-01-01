using SharpTwitch.Core.Exceptions;
using System.Net;

namespace SharpTwitch.Core.Tests.Exceptions
{
    public class UnauthorizedRequestExceptionTests
    {
        public void UnauthorizedRequestException_Created()
        {
            var message = "Unauthorized";
            var unauthorizedRequestException = new UnauthorizedRequestException(message);
            Assert.Equal(message, unauthorizedRequestException.Message);
            Assert.Equal(HttpStatusCode.Unauthorized, unauthorizedRequestException.StatusCode);

            var exception = new HttpRequestException(message);
            unauthorizedRequestException = new UnauthorizedRequestException(message, exception);
            Assert.Equal(message, unauthorizedRequestException.Message);
            Assert.Equal(exception, unauthorizedRequestException.InnerException);
            Assert.Equal(HttpStatusCode.Unauthorized, unauthorizedRequestException.StatusCode);
        }
    }
}
