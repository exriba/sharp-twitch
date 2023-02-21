namespace SharpTwitch.EventSub.Core.EventMessageArgs
{
    public class ErrorMessageArgs : System.EventArgs
    {
        public string Message { get; internal set; } = string.Empty;

        public Exception? Exception { get; internal set; }
    }
}
