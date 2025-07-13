using Newtonsoft.Json;

namespace DevPieter.WLED_SDK.Core.Models.WledInfo;

public class LedInfo
{
    /// <summary>
    /// Total number of LEDs.
    /// </summary>
    [JsonProperty("count")]
    public int Count { get; private set; }

    /// <summary>
    /// Current frames per second.
    /// </summary>
    [JsonProperty("fps")]
    public int FramesPerSecond { get; private set; }

    /// <summary>
    /// Current LED power usage in milliamps as determined by the ABL.
    /// 0 if ABL is disabled.
    /// </summary>
    [JsonProperty("pwr")]
    public int CurrentPowerUsage { get; private set; }

    /// <summary>
    /// Maximum power budget in milliamps for the ABL.
    /// 0 if ABL is disabled.
    /// </summary>
    [JsonProperty("maxpwr")]
    public int MaxPowerBudget { get; private set; }

    /// <summary>
    /// Maximum number of segments supported by this version.
    /// </summary>
    [JsonProperty("maxseg")]
    public int MaxSegments { get; private set; }

    /// <summary>
    /// Logical AND of all active segment's virtual light capabilities.
    /// </summary>
    [JsonProperty("lc")]
    public byte LedCapabilities { get; private set; }

    // /// <summary> TODO fix
    // /// Per-segment virtual light capabilities.
    // /// </summary>
    // [JsonProperty("seglc")]
    // public byte[] SegmentVirtualLightCapabilities { get; private set; }
}