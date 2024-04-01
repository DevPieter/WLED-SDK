﻿using System.Drawing;
using Newtonsoft.Json.Linq;
using WLED_SDK.Utils;
using WLED_SDK.WledClients;

namespace WLED_SDK.Extensions.StateExtensions;

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