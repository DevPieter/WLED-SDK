using DevPieter.WLED_SDK.Core;
using DevPieter.WLED_SDK.Core.Extensions;
using DevPieter.WLED_SDK.Core.Utils;

namespace DevPieter.WLED_SDK.Extensions.State;

public static class BrightnessExtensions
{
    public static async Task SetBrightnessAsync(this IWledClient client, int brightness = 128)
        => await client.SendJsonAsync(new { bri = Math.Clamp(brightness, 0, 255) });

    public static async Task SetBrightnessAsync(this IWledClient client, int brightness = 128, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { bri = Math.Clamp(brightness, 0, 255) }, segments));
    }

    public static async Task SetRandomBrightnessAsync(this IWledClient client)
        => await client.SendJsonAsync(new { bri = "r" });

    public static async Task SetRandomBrightnessAsync(this IWledClient client, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { bri = "r" }, segments));
    }

    public static async Task IncreaseBrightnessAsync(this IWledClient client, int amount = 10)
        => await client.SendJsonAsync(new { bri = $"~{Math.Clamp(amount, 0, 255)}" });

    public static async Task IncreaseBrightnessAsync(this IWledClient client, int amount = 10, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { bri = $"~{Math.Clamp(amount, 0, 255)}" }, segments));
    }

    public static async Task DecreaseBrightnessAsync(this IWledClient client, int amount = 10)
        => await client.SendJsonAsync(new { bri = $"~-{Math.Clamp(amount, 0, 255)}" });

    public static async Task DecreaseBrightnessAsync(this IWledClient client, int amount = 10, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { bri = $"~-{Math.Clamp(amount, 0, 255)}" }, segments));
    }
}