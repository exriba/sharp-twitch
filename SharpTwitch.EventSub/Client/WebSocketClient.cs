using System.Text;
using Ardalis.GuardClauses;
using System.Net.WebSockets;
using SharpTwitch.EventSub.Core.EventMessageArgs;
using SharpTwitch.EventSub.Core.Models.Client;

namespace SharpTwitch.EventSub.Client
{
    /// <summary>
    /// WebSocketClient.
    /// </summary>
    public class WebSocketClient : IAsyncDisposable
    {
        #region Events
        internal event EventHandler<T>? OnDataMessage;
        internal event EventHandler<ErrorMessageArgs>? OnErrorMessage;
        #endregion

        private readonly ClientWebSocket _webSocket;

        public WebSocketClient()
        {
            _webSocket = new ClientWebSocket();
        }

        public bool Connected => _webSocket.State is WebSocketState.Open;

        public bool Faulted => _webSocket.CloseStatus is not WebSocketCloseStatus.Empty && 
            _webSocket.CloseStatus is not WebSocketCloseStatus.NormalClosure;

        internal async Task<bool> ConnectAsync(Uri uri)
        {
            Guard.Against.Null(uri, nameof(uri));

            try
            {
                if (_webSocket.State is WebSocketState.Open || _webSocket.State is WebSocketState.Connecting)
                    return true;

                await _webSocket.ConnectAsync(uri, CancellationToken.None);

#pragma warning disable 4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(ProcessDataAsync).ConfigureAwait(false);
#pragma warning disable 4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                return Connected;
            }
            catch (Exception ex)
            {
                var errorMessage = CreateErrorMessage("An error ocurred while connecting to the server.", ex);
                OnErrorMessage?.Invoke(this, errorMessage);
                return false;
            }
        }

        internal async Task<bool> DisconnectAsync()
        {
            try
            {
                if (_webSocket.State is WebSocketState.Open || _webSocket.State is WebSocketState.Connecting)
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);

                return true;
            }
            catch (Exception ex)
            {
                var errorMessage = CreateErrorMessage("An error ocurred while disconnecting from the server.", ex);
                OnErrorMessage?.Invoke(this, errorMessage);
                return false;
            }
        }

        private async Task ProcessDataAsync()
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);

            while (Connected)
            {
                try
                {
                    WebSocketReceiveResult result;
                    using var memoryStream = new MemoryStream();
                    do
                    {
                        result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
                        memoryStream.Write(buffer.Array!, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);
                    
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    switch (result.MessageType)
                    {
                        case WebSocketMessageType.Text:
                        {
                            if (memoryStream.Length is 0)
                                continue;

                            using var reader = new StreamReader(memoryStream, Encoding.UTF8);
                            var message = await reader.ReadToEndAsync();
                            var dataMessage = new T{ Message = message };
                            OnDataMessage?.Invoke(this, dataMessage);
                            break;
                        }
                        case WebSocketMessageType.Close:
                        case WebSocketMessageType.Binary:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"Unknown message type: {result.MessageType}");
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = CreateErrorMessage("An error ocurred while handling incoming message.", ex);
                    OnErrorMessage?.Invoke(this, errorMessage);
                    break;
                }
            }
        }

        internal CloseDetails GetCloseDetails() 
        {
            return new CloseDetails
            {
                CloseStatus = _webSocket.CloseStatus ?? WebSocketCloseStatus.Empty,
                Description = _webSocket.CloseStatusDescription ?? string.Empty,
            };
        }

        private static ErrorMessageArgs CreateErrorMessage(string message, Exception exception)
        {
            return new ErrorMessageArgs
            {
                Message = message,
                Exception = exception
            };
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (Connected)
                await DisconnectAsync().ConfigureAwait(false);
            
            _webSocket.Dispose();
        }
    }
}
