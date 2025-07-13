using DevPieter.WLED_SDK.Core;
using DevPieter.WLED_SDK.Core.Extensions;
using DevPieter.WLED_SDK.Core.Utils;

namespace DevPieter.WLED_SDK.Extensions.State;

public static class EffectExtensions
{
    public static bool IsValidEffectId(this IWledClient client, int effectId)
        => effectId >= 0 && effectId <= client.GetInfoOrThrow().EffectCount - 1;
    
    public static int ClampEffectId(this IWledClient client, int effectId)
        => Math.Clamp(effectId, 0, client.GetInfoOrThrow().EffectCount - 1);
    
    public static int GetRandomEffectId(this IWledClient client)
        => Random.Shared.Next(0, client.GetInfoOrThrow().EffectCount - 1);
    
    public static async Task SetEffectAsync(this IWledClient client, int effectId, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { fx = client.ClampEffectId(effectId) }, segments));
    }
    
    public static async Task SetRandomEffectAsync(this IWledClient client, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { fx = client.GetRandomEffectId() }, segments));
    }

    public static async Task StepThroughEffectsAsync(this IWledClient client, bool forward = true, int steps = 1, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { fx = $"{(forward ? "~" : "~-")}{steps}" }, segments));
    }
}