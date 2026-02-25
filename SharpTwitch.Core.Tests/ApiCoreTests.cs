using Moq;
using Moq.Protected;
using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Models.Response;
using System.Text.Json;

namespace SharpTwitch.Core.Tests
{
    public class ApiCoreTests
    {
        [Fact]
        public void ApiCore_GetAsync_Throws_InvalidArgs()
        {
            var httpClientFactory = new DefaultHttpClientFactory();
            var apiCore = new ApiCore(httpClientFactory);
            var headers = new Dictionary<Header, string>
            {
                { Header.CLIENT_ID, Guid.NewGuid().ToString() }
            };

            Assert.ThrowsAnyAsync<ArgumentException>(async () => await apiCore.GetAsync<IResponse>(UrlFragment.HELIX_USER, null!, null, CancellationToken.None));
            Assert.ThrowsAnyAsync<ArgumentException>(async () => await apiCore.GetAsync<IResponse>(UrlFragment.HELIX_USER, headers, null, CancellationToken.None));
        }

        [Fact]
        public async Task ApiCore_GetAsync()
        {
            var headers = new Dictionary<Header, string>
            {
                { Header.CLIENT_ID, Guid.NewGuid().ToString() }
            };
            var queryParameters = new Dictionary<QueryParameter, string>
            {
                { QueryParameter.BROADCASTER_ID, Guid.NewGuid().ToString() }
            };

            var content = new SampleResponse { Text = "text" };
            var serializedContent = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(serializedContent);
            var mockHttpMessageHandler = CreateHttpMessageHandler(stringContent);
            var httpClientFactory = new DefaultHttpClientFactory(mockHttpMessageHandler.Object);
            var apiCore = new ApiCore(httpClientFactory);

            var data = await apiCore.GetAsync<SampleResponse>(UrlFragment.HELIX_USER, headers, queryParameters, CancellationToken.None);

            Assert.NotNull(data);
            Assert.Equal(content.Text, data.Text);
        }

        [Fact]
        public void ApiCore_PostAsync_FormUrlEncodedContent_Throws_InvalidArgs()
        {
            var headers = new Dictionary<Header, string>
            {
                { Header.CLIENT_ID, Guid.NewGuid().ToString() }
            };
            var content = new List<KeyValuePair<string, string>>();
            var httpClientFactory = new DefaultHttpClientFactory();
            var apiCore = new ApiCore(httpClientFactory);
            var formUrlEncodedContent = new FormUrlEncodedContent(content);

            Assert.ThrowsAnyAsync<ArgumentException>(async () => await apiCore.PostAsync<IResponse>(UrlFragment.HELIX_USER, null!, default(FormUrlEncodedContent)!, CancellationToken.None));
            Assert.ThrowsAnyAsync<ArgumentException>(async () => await apiCore.PostAsync<IResponse>(UrlFragment.HELIX_USER, new Dictionary<Header, string>(), default(FormUrlEncodedContent)!, CancellationToken.None));
            Assert.ThrowsAnyAsync<ArgumentException>(async () => await apiCore.PostAsync<IResponse>(UrlFragment.HELIX_USER, headers, default(FormUrlEncodedContent)!, CancellationToken.None));
            Assert.ThrowsAnyAsync<ArgumentException>(async () => await apiCore.PostAsync<IResponse>(UrlFragment.HELIX_USER, headers, formUrlEncodedContent, CancellationToken.None));
        }

        [Fact]
        public async Task ApiCore_PostAsync_FormUrlEncodedContent()
        {
            var headers = new Dictionary<Header, string>
            {
                { Header.CLIENT_ID, Guid.NewGuid().ToString() }
            };
            var content = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("test", "test")
            };

            var sampleResponse = new SampleResponse { Text = "text" };
            var formUrlEncodedContent = new FormUrlEncodedContent(content);
            var serializedContent = JsonSerializer.Serialize(sampleResponse);
            var stringContent = new StringContent(serializedContent);
            var mockHttpMessageHandler = CreateHttpMessageHandler(stringContent);
            var httpClientFactory = new DefaultHttpClientFactory(mockHttpMessageHandler.Object);
            var apiCore = new ApiCore(httpClientFactory);

            var data = await apiCore.PostAsync<SampleResponse>(UrlFragment.HELIX_USER, headers, formUrlEncodedContent, CancellationToken.None);

            Assert.NotNull(data);
            Assert.Equal(sampleResponse.Text, data.Text);
        }

        [Fact]
        public void ApiCore_PostAsync_StringContent_Throws_InvalidArgs()
        {
            var headers = new Dictionary<Header, string>
            {
                { Header.CLIENT_ID, Guid.NewGuid().ToString() }
            };
            var content = new List<KeyValuePair<string, string>>();
            var httpClientFactory = new DefaultHttpClientFactory();
            var apiCore = new ApiCore(httpClientFactory);

            Assert.ThrowsAnyAsync<ArgumentException>(async () => await apiCore.PostAsync<IResponse>(UrlFragment.HELIX_USER, headers, default(StringContent)!, CancellationToken.None));
        }

        [Fact]
        public async Task ApiCore_PostAsync_StringContent()
        {
            var headers = new Dictionary<Header, string>
            {
                { Header.CLIENT_ID, Guid.NewGuid().ToString() }
            };

            var content = new SampleResponse { Text = "text" };
            var serializedContent = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(serializedContent);
            var mockHttpMessageHandler = CreateHttpMessageHandler(stringContent);
            var httpClientFactory = new DefaultHttpClientFactory(mockHttpMessageHandler.Object);
            var apiCore = new ApiCore(httpClientFactory);

            var data = await apiCore.PostAsync<SampleResponse>(UrlFragment.HELIX_USER, headers, stringContent, CancellationToken.None);

            Assert.NotNull(data);
            Assert.Equal(content.Text, data.Text);
        }

        [Fact]
        public void ApiCore_DeleteAsync_Throws_InvalidArgs()
        {
            var httpClientFactory = new DefaultHttpClientFactory();
            var apiCore = new ApiCore(httpClientFactory);
            var queryParameters = new Dictionary<QueryParameter, string>
            {
                { QueryParameter.BROADCASTER_ID, Guid.NewGuid().ToString() }
            };

            Assert.ThrowsAnyAsync<ArgumentException>(async () => await apiCore.DeleteAsync(UrlFragment.HELIX_USER, null!, queryParameters, CancellationToken.None));
        }

        [Fact]
        public async Task ApiCore_DeleteAsync()
        {
            var headers = new Dictionary<Header, string>
            {
                { Header.CLIENT_ID, Guid.NewGuid().ToString() }
            };
            var queryParameters = new Dictionary<QueryParameter, string>
            {
                { QueryParameter.BROADCASTER_ID, Guid.NewGuid().ToString() }
            };

            var content = new SampleResponse { Text = "text" };
            var serializedContent = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(serializedContent);
            var mockHttpMessageHandler = CreateHttpMessageHandler(stringContent);
            var httpClientFactory = new DefaultHttpClientFactory(mockHttpMessageHandler.Object);
            var apiCore = new ApiCore(httpClientFactory);

            await apiCore.DeleteAsync(UrlFragment.HELIX_USER, headers, queryParameters, CancellationToken.None);
        }

        private Mock<HttpMessageHandler> CreateHttpMessageHandler(StringContent content)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = content
                });
            return mockHttpMessageHandler;
        }
    }

    internal class SampleResponse : IResponse
    {
        public string Text { get; set; } = string.Empty;
    }

    internal class DefaultHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpMessageHandler? _httpMessageHandler;

        internal DefaultHttpClientFactory() { }

        internal DefaultHttpClientFactory(HttpMessageHandler httpMessageHandler)
        {
            _httpMessageHandler = httpMessageHandler;
        }

        public HttpClient CreateClient(string name)
        {
            return _httpMessageHandler == null
                ? new HttpClient()
                : new HttpClient(_httpMessageHandler);
        }
    }
}