using DevPieter.WLED_SDK.Core.Models.WledInfo;
using DevPieter.WLED_SDK.Core.Models.WledState;

namespace DevPieter.WLED_SDK.Core;

public interface IWledClient
{
    /// <summary>
    /// The current info of the WLED device.
    /// This is null if the client isn't connected or the info has not been received yet.
    /// </summary>
    public Info? Info { get; }

    /// <summary>
    /// The current state of the WLED device.
    /// This is null if the client isn't connected or the state has not been received yet.
    /// </summary>
    public State? State { get; }

    /// <summary>
    /// Sends a JSON message to the WLED device.
    /// Please read the <see href="https://kno.wled.ge/interfaces/json-api/">WLED JSON API documentation</see> for more information.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <typeparam name="T">The type of the message.</typeparam>
    Task SendJsonAsync<T>(T message);
}