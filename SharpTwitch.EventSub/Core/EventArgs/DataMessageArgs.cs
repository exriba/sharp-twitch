namespace SharpTwitch.EventSub.Core.EventMessageArgs
{
    public class DataMessageArgs : System.EventArgs
    {
        public string Message { get; set; } = string.Empty;
    }
}
