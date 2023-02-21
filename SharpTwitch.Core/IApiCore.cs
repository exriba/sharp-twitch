using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Models.Response;
using System.Text.Json;

namespace SharpTwitch.Core
{
    public interface IApiCore
    {
        JsonSerializerOptions? JsonSerializerOptions { get; }
        Task<T> GetAsync<T>(UrlFragment pathFragment, IDictionary<Header, string> headers, IEnumerable<KeyValuePair<QueryParameter, string>>? queryParams, CancellationToken cancellationToken) where T : IResponse;
        Task<T> PostAsync<T>(UrlFragment pathFragment, IDictionary<Header, string> headers, FormUrlEncodedContent formUrlEncodedContent, CancellationToken cancellationToken) where T : IResponse;
        Task<T> PostAsync<T>(UrlFragment pathFragment, IDictionary<Header, string> headers, StringContent stringContent, CancellationToken cancellationToken) where T : IResponse;
        Task DeleteAsync(UrlFragment pathFragment, IDictionary<Header, string> headers, IEnumerable<KeyValuePair<QueryParameter, string>>? queryParams, CancellationToken cancellationToken);
    }
}
