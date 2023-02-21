namespace SharpTwitch.EventSub.Core.EventMessageArgs
{
    public class T : System.EventArgs
    {
        public string Message { get; internal set; } = string.Empty; 
    }
}
