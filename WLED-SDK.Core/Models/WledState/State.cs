using DevPieter.WLED_SDK.Core.Enums;
using Newtonsoft.Json;

namespace DevPieter.WLED_SDK.Core.Models.WledState;

public class State
{
    /// <summary>
    /// On/Off state of the light.
    /// </summary>
    [JsonProperty("on")]
    public bool On { get; private set; }

    /// <summary>
    /// Brightness of the light.
    /// If <see cref="On"/> is false, contains last brightness when light was on (aka brightness when <see cref="On"/> is set to true).
    /// </summary>
    [JsonProperty("bri")]
    public int Brightness { get; private set; }

    /// <summary>
    /// Duration of the cross-fade between different colors/brightness levels.
    /// One unit is 100ms, so a value of 4 results in a transition of 400ms.
    /// </summary>
    [JsonProperty("transition")]
    public int TransitionDuration { get; private set; }

    /// <summary>
    /// ID of currently set preset.
    /// </summary>
    [JsonProperty("ps")]
    public int PresetId { get; private set; }

    /// <summary>
    /// ID of currently set playlist.
    /// </summary>
    [JsonProperty("pl")]
    public int PlaylistId { get; private set; }

    /// <summary>
    /// Nightlight settings.
    /// </summary>
    [JsonProperty("nl")]
    public Nightlight Nightlight { get; private set; } = null!;

    /// <summary>
    /// UDP notifier settings.
    /// </summary>
    [JsonProperty("udpn")]
    public UdpNotifier UdpNotifier { get; private set; } = null!;

    /// <summary>
    /// Live data override.
    /// </summary>
    [JsonProperty("lor")]
    public LiveDataOverride LiveDataOverride { get; private set; }

    /// <summary>
    /// ID of the main segment.
    /// </summary>
    [JsonProperty("mainseg")]
    public int MainSegmentId { get; private set; }

    /// <summary>
    /// Array of segment objects.
    /// </summary>
    [JsonProperty("seg")]
    public Segment[] Segments { get; private set; } = null!;
}