using System.Net;
using System.Text;
using Ardalis.GuardClauses;
using Newtonsoft.Json;
using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Exceptions;
using SharpTwitch.Core.Interfaces;

namespace SharpTwitch.Core
{
    public class DefaultApiCore : IApiCore
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public DefaultApiCore(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetAsync<T>(UrlFragment fragment, IDictionary<Header, string> headers, IDictionary<QueryParameter, string>? queryParams, CancellationToken cancellationToken) where T : IResponse
        {
            Guard.Against.NullOrEmpty(headers, nameof(headers));

            var uri = BuildUri(fragment, queryParams);
            var client = CreateClient(headers);
            var responseMessage = await client.GetAsync(uri, cancellationToken);

            return await HandleResponse<T>(responseMessage, cancellationToken);
        }

        public async Task<T> PostAsync<T>(UrlFragment fragment, IDictionary<Header, string> headers, FormUrlEncodedContent formUrlEncodedContent, CancellationToken cancellationToken) where T : IResponse
        {
            Guard.Against.NullOrEmpty(headers, nameof(headers));
            Guard.Against.Null(formUrlEncodedContent, nameof(formUrlEncodedContent));

            var client = CreateClient(headers);
            var responseMessage = await client.PostAsync(fragment.ConvertToString(), formUrlEncodedContent, cancellationToken);

            return await HandleResponse<T>(responseMessage, cancellationToken);
        }

        public async Task<T> PostAsync<T>(UrlFragment fragment, IDictionary<Header, string> headers, StringContent stringContent, CancellationToken cancellationToken) where T : IResponse
        {
            Guard.Against.NullOrEmpty(headers, nameof(headers));
            Guard.Against.Null(stringContent, nameof(stringContent));

            var client = CreateClient(headers);
            var responseMessage = await client.PostAsync(fragment.ConvertToString(), stringContent, cancellationToken);

            return await HandleResponse<T>(responseMessage, cancellationToken);
        }

        private static string BuildUri(UrlFragment fragment, IDictionary<QueryParameter, string>? queryParams)
        {
            var uri = fragment.ConvertToString();
            var builder = new StringBuilder();
            if (queryParams != null)
                builder.AppendJoin("&", queryParams.Select(kvp => $"{kvp.Key.ConvertToString()}={kvp.Value}").ToArray());
            return builder.Length == 0 ? uri : string.Join("?", uri, builder.ToString());
        }

        private HttpClient CreateClient(IDictionary<Header, string> headers)
        {
            var client = _httpClientFactory.CreateClient();
            headers.ToList().ForEach(param => {
                var kvp = param.Key.Transform(param.Value);
                client.DefaultRequestHeaders.TryAddWithoutValidation(kvp.Key, kvp.Value);
            });
            return client;
        }

        private static async Task<T> HandleResponse<T>(HttpResponseMessage responseMessage, CancellationToken cancellationToken) where T : IResponse
        {
            if (!responseMessage.IsSuccessStatusCode)
                await HandleException(responseMessage, cancellationToken);

            var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<T>(content, _jsonSerializerSettings);
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
