namespace DevPieter.WLED_SDK.Client.WebSocket;

public class WledWebsocketException : Exception
{
    public int? ErrorCode { get; }

    internal WledWebsocketException(int? errorCode, string error) : base(errorCode.HasValue ? $"WLED rejected the WebSocket command with error {errorCode.Value}." : error)
    {
        ErrorCode = errorCode;
    }
}