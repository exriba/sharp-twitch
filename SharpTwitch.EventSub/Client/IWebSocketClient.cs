using SharpTwitch.EventSub.Core.EventMessageArgs;

namespace SharpTwitch.EventSub.Client
{
    /// <summary>
    /// Represents a WebSocket client capable of connecting to a server, sending and receiving messages, 
    /// and notifying subscribers of incoming data or errors.
    /// </summary>
    public interface IWebSocketClient : IAsyncDisposable
    {
        /// <summary>
        /// Occurs when a data message is received from the WebSocket server.
        /// </summary>
        event EventHandler<T>? OnDataMessage;

        /// <summary>
        /// Occurs when an error message or exception is raised by the WebSocket client.
        /// </summary>
        event EventHandler<ErrorMessageArgs>? OnErrorMessage;

        /// <summary>
        /// Gets a value indicating whether the WebSocket client is currently connected to the server.
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Gets a value indicating whether the WebSocket client is in a faulted or unusable state.
        /// </summary>
        bool Faulted { get; }

        /// <summary>
        /// Connects the WebSocket client to the specified server URI asynchronously.
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="cancellationToken">cancellation requests.</param>
        Task ConnectAsync(Uri uri, CancellationToken cancellationToken = default);

        /// <summary>
        /// Disconnects the WebSocket client from the server asynchronously.
        /// </summary>
        /// <param name="cancellationToken">cancellation requests.</param>
        Task DisconnectAsync(CancellationToken cancellationToken = default);
    }
}
