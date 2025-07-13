using Newtonsoft.Json;

namespace DevPieter.WLED_SDK.Core.Models.WledInfo;

public class Info
{
    /// <summary>
    /// WLED version name.
    /// </summary>
    [JsonProperty("ver")]
    public string Version { get; private set; } = null!;

    /// <summary>
    /// WLED version ID.
    /// </summary>
    [JsonProperty("vid")]
    public int VersionId { get; private set; }

    /// <summary>
    /// Info about the LEDs.
    /// </summary>
    [JsonProperty("leds")]
    public LedInfo LedInfo { get; private set; } = null!;

    /// <summary>
    /// If true, an UI with only a single button for toggling sync should toggle receive+send, otherwise send only.
    /// </summary>
    [JsonProperty("str")]
    public bool SyncToggleSendReceive { get; private set; }

    /// <summary>
    /// Friendly name of the WLED device. Intended for display in lists and titles.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; private set; } = null!;

    /// <summary>
    /// The UDP port for realtime packets and WLED broadcast.
    /// </summary>
    [JsonProperty("udpport")]
    public int UdpPort { get; private set; }

    /// <summary>
    /// If true, WLED is currently receiving realtime data via UDP or E1.31.
    /// </summary>
    [JsonProperty("live")]
    public bool RealtimeActive { get; private set; }

    /// <summary>
    /// TODO - Not found in docs.
    /// <remarks>Not found in the docs.</remarks>
    /// </summary>
    [JsonProperty("liveseg")]
    public int RealtimeSegment { get; private set; } // TODO - Check if correct naming

    /// <summary>
    /// Info about the realtime data source.
    /// </summary>
    [JsonProperty("lm")]
    public string RealtimeDataSourceInfo { get; private set; } = null!;

    /// <summary>
    /// The IP address of the realtime data source.
    /// </summary>
    [JsonProperty("lip")]
    public string RealtimeDataSourceIp { get; private set; } = null!;

    /// <summary>
    /// Number of currently connected WebSockets clients.
    /// -1 indicates that WS is unsupported in this build.
    /// </summary>
    [JsonProperty("ws")]
    public int WebsocketClients { get; private set; }

    /// <summary>
    /// Number of included effects.
    /// </summary>
    [JsonProperty("fxcount")]
    public int EffectCount { get; private set; }

    /// <summary>
    /// Number of configured palettes.
    /// </summary>
    [JsonProperty("palcount")]
    public int PaletteCount { get; private set; }

    /// <summary>
    /// TODO - Not found in docs.
    /// <remarks>Not found in the docs.</remarks>
    /// </summary>
    [JsonProperty("cpalcount")]
    public int CustomPaletteCount { get; private set; } // TODO - Check if correct naming

    // /// <summary>
    // /// TODO - Not found in docs.
    // /// <remarks>Not found in the docs.</remarks>
    // /// </summary>
    // [JsonProperty("maps")]
    // public int[] Maps { get; private set; }

    /// <summary>
    /// Info about the WiFi connection.
    /// </summary>
    [JsonProperty("wifi")]
    public WifiInfo WifiInfo { get; private set; } = null!;

    // [JsonProperty("fs")]
    // public FileSystemInfo FileSystemInfo { get; private set; } // TODO - Implement

    /// <summary>
    /// Number of other WLED devices discovered on the network.
    /// -1 if Node discovery disabled.
    /// </summary>
    [JsonProperty("ndc")]
    public int DiscoveredDevices { get; private set; }

    /// <summary>
    /// The name of the architecture.
    /// </summary>
    [JsonProperty("arch")]
    public string Architecture { get; private set; } = null!;

    /// <summary>
    /// Version of the underlying (Arduino core) SDK.
    /// </summary>
    [JsonProperty("core")]
    public string CoreVersion { get; private set; } = null!;

    /// <summary>
    /// Bytes of heap memory (RAM) currently available.
    /// Problematic if below 10k.
    /// </summary>
    [JsonProperty("freeheap")]
    public int FreeHeap { get; private set; }

    /// <summary>
    /// Time since the last boot/reset in seconds.
    /// </summary>
    [JsonProperty("uptime")]
    public int Uptime { get; private set; }

    /// <summary>
    /// TODO - Not found in docs.
    /// <remarks>Not found in the docs.</remarks>
    /// </summary>
    [JsonProperty("time")]
    public string Time { get; private set; } = null!;

    /// <summary>
    /// Used for debugging purposes only.
    /// </summary>
    [JsonProperty("opt")]
    public int DebugOpt { get; private set; }

    /// <summary>
    /// The producer/vendor of the light.
    /// Always WLED for standard installations.
    /// </summary>
    [JsonProperty("brand")]
    public string Brand { get; private set; } = null!;

    /// <summary>
    /// The product name.
    /// Always FOSS for standard installations.
    /// </summary>
    [JsonProperty("product")]
    public string Product { get; private set; } = null!;

    /// <summary>
    /// The hexadecimal hardware MAC address of the light, lowercase and without colons.
    /// </summary>
    [JsonProperty("mac")]
    public string MacAddress { get; private set; } = null!;

    /// <summary>
    /// The IP address of this instance.
    /// Empty string if not connected to a network.
    /// </summary>
    [JsonProperty("ip")]
    public string IpAddress { get; private set; } = null!;
}