using System.Text;
using DevPieter.WLED_SDK.Core;
using DevPieter.WLED_SDK.Core.Extensions;
using Newtonsoft.Json;

namespace DevPieter.WLED_SDK.Builders.Extensions;

public static class SegmentExtensions
{
    /// <summary>
    /// Removes all segments and adds a single segment that covers all LEDs.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="setDefaults"><see cref="SegmentBuilder.SetDefaults(bool)"/></param>
    public static async Task ResetSegmentsAsync(this IWledClient client, bool setDefaults = false)
    {
        var info = client.GetInfoOrThrow();
        var builder = new SegmentBuilder(client);

        // Add 'default' segment
        builder.AddSegment(0, info.LedInfo.Count, id: 0).Selected();
        if (setDefaults) builder.SetDefaults();

        // Add empty segments (to reset all other segments)
        for (var i = 1; i < info.LedInfo.MaxSegments; i++)
        {
            builder.AddSegment(0, 0, id: i);
        }

        await client.SetSegmentsAsync(builder, true);
    }

    /// <summary>
    /// Sets the segments of the WLED device.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="builder">The builder that contains the segments.</param>
    /// <param name="forceOneRequest">If true, the segments will be sent in one request. Otherwise, the segments will be split into multiple requests if the size exceeds 500 bytes.</param>
    public static async Task SetSegmentsAsync(this IWledClient client, SegmentBuilder builder, bool forceOneRequest = false)
    {
        var json = builder.Build();
        var size = Encoding.UTF8.GetByteCount(json.ToString(Formatting.None));

        // TODO - Check max size (for now we use 500) (Around 500 bytes will cause MY esp8266 to crash (WLED 0.14.0))
        if (forceOneRequest || size <= 500)
        {
            await client.SendJsonAsync(json);
            return;
        }

        foreach (var segment in builder.BuildSplit())
        {
            await client.SendJsonAsync(segment);
        }
    }
}