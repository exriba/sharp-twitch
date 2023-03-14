using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Models.Response;
using System.Text.Json;

namespace SharpTwitch.Core
{
    /// <summary>
    /// Defines HTTP request handlers.  
    /// </summary>
    public interface IApiCore
    {
        /// <summary>
        /// The JSON serialization options.
        /// </summary>
        JsonSerializerOptions? JsonSerializerOptions { get; }

        /// <summary>
        /// Send a GET request to the specified Uri.
        /// </summary>
        /// <typeparam name="T">A type that implements IResponse</typeparam>
        /// <param name="pathFragment">base route of the endpoint</param>
        /// <param name="headers">request headers</param>
        /// <param name="queryParams">query parameters (Optional)</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>A concrete implementation of IResponse</returns>
        Task<T> GetAsync<T>(UrlFragment pathFragment, IDictionary<Header, string> headers, IEnumerable<KeyValuePair<QueryParameter, string>>? queryParams, CancellationToken cancellationToken) where T : IResponse;

        /// <summary>
        /// Send a POST request to the specified Uri.
        /// </summary>
        /// <typeparam name="T">A type that implements IResponse</typeparam>
        /// <param name="pathFragment">base route of the endpoint</param>
        /// <param name="headers">request headers</param>
        /// <param name="formUrlEncodedContent">form url encoded content</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>A concrete implementation of IResponse</returns>
        Task<T> PostAsync<T>(UrlFragment pathFragment, IDictionary<Header, string> headers, FormUrlEncodedContent formUrlEncodedContent, CancellationToken cancellationToken) where T : IResponse;

        /// <summary>
        /// Send a POST request to the specified Uri.
        /// </summary>
        /// <typeparam name="T">A type that implements IResponse</typeparam>
        /// <param name="pathFragment">base route of the endpoint</param>
        /// <param name="headers">request headers</param>
        /// <param name="stringContent">string content</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>A concrete implementation of IResponse</returns>
        Task<T> PostAsync<T>(UrlFragment pathFragment, IDictionary<Header, string> headers, StringContent stringContent, CancellationToken cancellationToken) where T : IResponse;

        /// <summary>
        /// Send a DELETE request to the specified Uri.
        /// </summary>
        /// <param name="pathFragment">base route of the endpoint</param>
        /// <param name="headers">request headers</param>
        /// <param name="queryParams">query parameters (Optional)</param>
        /// <param name="cancellationToken">cancellation token</param>
        Task DeleteAsync(UrlFragment pathFragment, IDictionary<Header, string> headers, IEnumerable<KeyValuePair<QueryParameter, string>>? queryParams, CancellationToken cancellationToken);
    }
}
