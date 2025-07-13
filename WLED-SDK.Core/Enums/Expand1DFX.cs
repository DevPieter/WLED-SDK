using System.ComponentModel;

namespace DevPieter.WLED_SDK.Core.Enums;

public enum Expand1Dfx
{
    [Description("Pixels")] Pixels = 0,
    [Description("Bar")] Bar = 1,
    [Description("Arc")] Arc = 2,
    [Description("Corner")] Corner = 3
}