using System.ComponentModel;

namespace DevPieter.WLED_SDK.Core.Enums;

public enum SoundSimulationType
{
    [Description("Beat sin")] BeatSin = 0,
    [Description("We will rock you")] WeWillRockYou = 1,
    [Description("10/3")] TenThree = 2,
    [Description("14/3")] FourteenThree = 3
}