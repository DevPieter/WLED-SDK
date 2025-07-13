using System.ComponentModel;

namespace DevPieter.WLED_SDK.Core.Enums;

public enum NightlightMode
{
    [Description("Instant")] Instant = 0,
    [Description("Fade")] Fade = 1,
    [Description("Color fade")] ColorFade = 2,
    [Description("Sunrise")] Sunrise = 3
}