using DevPieter.WLED_SDK.Core;
using DevPieter.WLED_SDK.Core.Extensions;
using DevPieter.WLED_SDK.Core.Utils;

namespace DevPieter.WLED_SDK.Extensions.State;

public static class PaletteExtensions
{
    public static bool IsValidPaletteId(this IWledClient client, int paletteId)
        => GetValidPaletteIds(client).Contains(paletteId);

    public static int ClampPaletteId(this IWledClient client, int paletteId)
    {
        var paletteIds = GetValidPaletteIds(client);
        return paletteIds.MinBy(validId => Math.Abs((long)validId - paletteId));
    }

    public static int GetRandomPaletteId(this IWledClient client)
    {
        var paletteIds = GetValidPaletteIds(client);
        return paletteIds[Random.Shared.Next(paletteIds.Length)];
    }

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

    private static int[] GetValidPaletteIds(IWledClient client)
    {
        var info = client.GetInfoOrThrow();

        var customPaletteCount = Math.Max(0, info.CustomPaletteCount);
        var usermodPaletteCount = Math.Max(0, info.UsermodPaletteCount);
        var builtInPaletteCount = Math.Max(0, info.PaletteCount - customPaletteCount - usermodPaletteCount);

        var paletteIds = Enumerable.Range(0, builtInPaletteCount).ToList();

        if (info.MaxCustomPaletteCount.HasValue)
        {
            AddDescendingRange(paletteIds, 200, customPaletteCount);
            AddDescendingRange(paletteIds, 255, usermodPaletteCount);
        }
        else
        {
            AddDescendingRange(paletteIds, 255, customPaletteCount);
        }

        if (paletteIds.Count is 0) throw new InvalidOperationException("The WLED device did not report any available palettes.");
        return paletteIds.ToArray();
    }

    private static void AddDescendingRange(List<int> paletteIds, int firstId, int count)
    {
        for (var offset = 0; offset < count; offset++)
        {
            paletteIds.Add(firstId - offset);
        }
    }
}