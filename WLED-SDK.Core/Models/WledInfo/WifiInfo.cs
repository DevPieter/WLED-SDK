using Newtonsoft.Json;

namespace DevPieter.WLED_SDK.Core.Models.WledInfo;

public class WifiInfo
{
    /// <summary>
    /// The BSSID of the currently connected network.
    /// </summary>
    [JsonProperty("bssid")]
    public string Bssid { get; private set; } = null!;

    /// <summary>
    /// Received signal strength indicator of the current connection.
    /// <remarks>Not found in the docs.</remarks>
    /// </summary>
    [JsonProperty("rssi")]
    public int Rssi { get; private set; }

    /// <summary>
    /// Relative signal quality of the current connection.
    /// </summary>
    [JsonProperty("signal")]
    public int SignalQuality { get; private set; }

    /// <summary>
    /// The current WiFi channel.
    /// </summary>
    [JsonProperty("channel")]
    public int Channel { get; private set; }
}