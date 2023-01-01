using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Interfaces
{
    public interface IApiCore
    {
        Task<T> GetAsync<T>(UrlFragment pathFragment, IDictionary<Header, string> headers, IDictionary<QueryParameter, string>? queryParams, CancellationToken cancellationToken) where T : IResponse;
        Task<T> PostAsync<T>(UrlFragment pathFragment, IDictionary<Header, string> headers, FormUrlEncodedContent formUrlEncodedContent, CancellationToken cancellationToken) where T : IResponse;
        Task<T> PostAsync<T>(UrlFragment pathFragment, IDictionary<Header, string> headers, StringContent stringContent, CancellationToken cancellationToken) where T : IResponse;
    }
}
