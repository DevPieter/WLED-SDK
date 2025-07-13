using DevPieter.WLED_SDK.Core;
using DevPieter.WLED_SDK.Core.Extensions;
using DevPieter.WLED_SDK.Core.Utils;

namespace DevPieter.WLED_SDK.Extensions.State;

public static class StateExtensions
{
    public static async Task TurnOnAsync(this IWledClient client)
        => await client.SendJsonAsync(new { on = true });

    public static async Task TurnOnAsync(this IWledClient client, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { on = true }, segments));
    }
    
    public static async Task TurnOffAsync(this IWledClient client)
        => await client.SendJsonAsync(new { on = false });
    
    public static async Task TurnOffAsync(this IWledClient client, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { on = false }, segments));
    }

    public static async Task ToggleOnOffAsync(this IWledClient client)
        => await client.SendJsonAsync(new { on = "t" });

    public static async Task ToggleOnOffAsync(this IWledClient client, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { on = "t" }, segments));
    }
}