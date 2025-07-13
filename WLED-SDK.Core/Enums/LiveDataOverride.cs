using System.ComponentModel;

namespace DevPieter.WLED_SDK.Core.Enums;

public enum LiveDataOverride
{
    [Description("Off")] Off = 0,
    [Description("Until live data ends")] UntilLiveDataEnds = 1,
    [Description("Until ESP reboot")] UntilEspReboot = 2
}