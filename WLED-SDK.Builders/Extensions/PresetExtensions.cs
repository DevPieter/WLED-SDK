using DevPieter.WLED_SDK.Core;

namespace DevPieter.WLED_SDK.Builders.Extensions;

public static class PresetExtensions
{
    public static async Task LoadPresetAsync(this IWledClient client, int presetId)
        => await client.SendJsonAsync(new { ps = Math.Clamp(presetId, -1, 250) });

    public static async Task LoadPresetAsync(this IWledClient client, int startPresetId, int endPresetId, bool shuffle = false)
        => await client.SendJsonAsync(new { ps = $"{Math.Clamp(startPresetId, -1, 250)}~{Math.Clamp(endPresetId, -1, 250)}~{(shuffle ? "r" : "")}" });

    public static async Task SavePresetAsync(this IWledClient client, PresetBuilder builder)
        => await client.SendJsonAsync(builder.Build());

    public static async Task DeletePresetAsync(this IWledClient client, int presetId)
        => await client.SendJsonAsync(new { pdel = Math.Clamp(presetId, 1, 250) });
}