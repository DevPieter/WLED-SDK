using System.Drawing;
using DevPieter.WLED_SDK.Core;
using DevPieter.WLED_SDK.Core.Extensions;
using DevPieter.WLED_SDK.Core.Utils;
using Newtonsoft.Json.Linq;

namespace DevPieter.WLED_SDK.Extensions.State;

public static class ColorExtensions
{
    public static async Task SetColorsAsync(this IWledClient client, Color? primary, Color? secondary, Color? tertiary, params int[]? segmentsIds)
    {
        var segments = SegmentUtils.ClampIds(client.GetStateOrThrow(), segmentsIds);
        var colors = new JArray
        {
            primary?.ToHexString(false) ?? "",
            secondary?.ToHexString(false) ?? "",
            tertiary?.ToHexString(false) ?? ""
        };

        await client.SendJsonAsync(SegmentUtils.CreateSegmentsWith(new { col = colors }, segments));
    }

    public static async Task SetPrimaryColorAsync(this IWledClient client, Color? color, params int[]? segmentsIds)
        => await client.SetColorsAsync(color, null, null, segmentsIds);

    public static async Task SetSecondaryColorAsync(this IWledClient client, Color? color, params int[]? segmentsIds)
        => await client.SetColorsAsync(null, color, null, segmentsIds);

    public static async Task SetTertiaryColorAsync(this IWledClient client, Color? color, params int[]? segmentsIds)
        => await client.SetColorsAsync(null, null, color, segmentsIds);
}