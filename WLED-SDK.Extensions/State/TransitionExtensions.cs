using DevPieter.WLED_SDK.Core;

namespace DevPieter.WLED_SDK.Extensions.State;

public static class TransitionExtensions
{
    public static async Task SetTransitionDurationAsync(this IWledClient client, int duration = 4)
        => await client.SendJsonAsync(new { transition = Math.Clamp(duration, 0, 65535) });
}