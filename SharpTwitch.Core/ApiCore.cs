using Ardalis.GuardClauses;
using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Exceptions;
using SharpTwitch.Core.Models.Response;
using SharpTwitch.Core.NamingPolicies;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SharpTwitch.Core
{
    /// <summary>
    /// Default implementation of IApiCore.
    /// </summary>
    public class ApiCore : IApiCore
    {
        /// <inheritdoc/>
        public JsonSerializerOptions? JsonSerializerOptions => new()
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(15);

        public ApiCore(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">If headers is null</exception>
        /// <exception cref="ArgumentException">If headers is empty</exception>
        public async Task<T> GetAsync<T>(UrlFragment fragment, IDictionary<Header, string> headers, IEnumerable<KeyValuePair<QueryParameter, string>>? queryParams, CancellationToken cancellationToken) where T : IResponse
        {
            Guard.Against.NullOrEmpty(headers, nameof(headers));

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var uri = BuildUri(fragment, queryParams);
            using var client = CreateClient(headers);
            cts.CancelAfter(_requestTimeout);

            try
            {
                var responseMessage = await client.GetAsync(uri, cancellationToken).ConfigureAwait(false);
                return await HandleResponse<T>(responseMessage, cancellationToken);
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException($"Request to {uri} exceeded {_requestTimeout.TotalSeconds}.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">If headers or formUrlEncodedContent is null</exception>
        /// <exception cref="ArgumentException">If headers is empty</exception>
        public async Task<T> PostAsync<T>(UrlFragment fragment, IDictionary<Header, string> headers, FormUrlEncodedContent formUrlEncodedContent, CancellationToken cancellationToken) where T : IResponse
        {
            Guard.Against.NullOrEmpty(headers, nameof(headers));
            Guard.Against.Null(formUrlEncodedContent, nameof(formUrlEncodedContent));

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            using var client = CreateClient(headers);
            var uri = fragment.ConvertToString();
            cts.CancelAfter(_requestTimeout);

            try
            {
                var responseMessage = await client.PostAsync(uri, formUrlEncodedContent, cancellationToken).ConfigureAwait(false);
                return await HandleResponse<T>(responseMessage, cancellationToken);
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException($"Request to {uri} exceeded {_requestTimeout.TotalSeconds}.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">If headers or stringContent is null</exception>
        /// <exception cref="ArgumentException">If headers is empty</exception>
        public async Task<T> PostAsync<T>(UrlFragment fragment, IDictionary<Header, string> headers, StringContent stringContent, CancellationToken cancellationToken) where T : IResponse
        {
            Guard.Against.NullOrEmpty(headers, nameof(headers));
            Guard.Against.Null(stringContent, nameof(stringContent));

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            using var client = CreateClient(headers);
            var uri = fragment.ConvertToString();
            cts.CancelAfter(_requestTimeout);

            try
            {
                var responseMessage = await client.PostAsync(uri, stringContent, cancellationToken).ConfigureAwait(false);
                return await HandleResponse<T>(responseMessage, cancellationToken);
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException($"Request to {uri} exceeded {_requestTimeout.TotalSeconds}.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">If headers is null</exception>
        /// <exception cref="ArgumentException">If headers is empty</exception>
        public async Task DeleteAsync(UrlFragment fragment, IDictionary<Header, string> headers, IEnumerable<KeyValuePair<QueryParameter, string>>? queryParams, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrEmpty(headers, nameof(headers));

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var uri = BuildUri(fragment, queryParams);
            using var client = CreateClient(headers);
            cts.CancelAfter(_requestTimeout);

            try
            {
                var responseMessage = await client.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);
                await HandleResponse(responseMessage, cancellationToken);
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException($"Request to {uri} exceeded {_requestTimeout.TotalSeconds}.");
            }
        }

        private HttpClient CreateClient(IDictionary<Header, string> headers)
        {
            var client = _httpClientFactory.CreateClient();
            headers.ToList().ForEach(param =>
            {
                var kvp = param.Key.Transform(param.Value);
                client.DefaultRequestHeaders.TryAddWithoutValidation(kvp.Key, kvp.Value);
            });
            return client;
        }

        private static async Task HandleResponse(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
        {
            if (!responseMessage.IsSuccessStatusCode)
                await HandleException(responseMessage, cancellationToken);
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage responseMessage, CancellationToken cancellationToken) where T : IResponse
        {
            await HandleResponse(responseMessage, cancellationToken);
            var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<T>(content, JsonSerializerOptions);

            if (result is null)
                throw new InvalidOperationException($"Failed to deserialize response as {typeof(T).Name}. Response: {content}");

            return result;
        }

        private static string BuildUri(UrlFragment fragment, IEnumerable<KeyValuePair<QueryParameter, string>>? queryParams)
        {
            var uri = fragment.ConvertToString();
            var builder = new StringBuilder();
            if (queryParams is not null)
                builder.AppendJoin("&", queryParams.Select(kvp =>
                {
                    var key = kvp.Key.ConvertToString();
                    var value = Uri.EscapeDataString(kvp.Value);
                    return $"{key}={value}";
                }).ToArray());
            return builder.Length is 0 ? uri : string.Join("?", uri, builder.ToString());
        }

        private static async Task HandleException(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
        {
            var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException(content);
                case HttpStatusCode.NotFound:
                case HttpStatusCode.UnprocessableEntity:
                case HttpStatusCode.TooManyRequests:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.GatewayTimeout:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.Forbidden:
                default:
                    throw new HttpRequestException(content, null, responseMessage.StatusCode);
            }
        }
    }
}
