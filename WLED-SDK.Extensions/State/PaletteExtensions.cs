using DevPieter.WLED_SDK.Core;
using DevPieter.WLED_SDK.Core.Extensions;
using DevPieter.WLED_SDK.Core.Utils;

namespace DevPieter.WLED_SDK.Extensions.State;

public static class PaletteExtensions
{
    public static bool IsValidPaletteId(this IWledClient client, int paletteId)
        => paletteId >= 0 && paletteId <= client.GetInfoOrThrow().PaletteCount - 1;

    public static int ClampPaletteId(this IWledClient client, int paletteId)
        => Math.Clamp(paletteId, 0, client.GetInfoOrThrow().PaletteCount - 1);

    public static int GetRandomPaletteId(this IWledClient client)
        => Random.Shared.Next(0, client.GetInfoOrThrow().PaletteCount - 1);

    public static async Task SetPaletteAsync(this IWledClient client, int paletteId, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { pal = client.ClampPaletteId(paletteId) }, segments));
    }

    public static async Task SetRandomPaletteAsync(this IWledClient client, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { pal = client.GetRandomPaletteId() }, segments));
    }

    public static async Task StepThroughPalettesAsync(this IWledClient client, bool forward = true, int steps = 1, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { pal = $"{(forward ? "~" : "~-")}{steps}" }, segments));
    }
}