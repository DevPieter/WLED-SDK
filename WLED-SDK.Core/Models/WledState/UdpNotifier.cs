using Newtonsoft.Json;

namespace DevPieter.WLED_SDK.Core.Models.WledState;

public class UdpNotifier
{
    /// <summary>
    /// Send WLED broadcast (UDP sync) packet on state change.
    /// </summary>
    [JsonProperty("send")]
    public bool Send { get; private set; }

    /// <summary>
    /// Receive WLED broadcast (UDP sync) packet.
    /// </summary>
    [JsonProperty("recv")]
    public bool Receive { get; private set; }

    /// <summary>
    /// Bitfield for broadcast send groups 1-8
    /// </summary>
    [JsonProperty("sgrp")]
    public int SendGroups { get; private set; }

    /// <summary>
    /// Bitfield for broadcast receive groups 1-8
    /// </summary>
    [JsonProperty("rgrp")]
    public int ReceiveGroups { get; private set; }
}