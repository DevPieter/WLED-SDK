using DevPieter.WLED_SDK.Core.Models.WledInfo;

namespace DevPieter.WLED_SDK.Core.WledEventArgs;

public class InfoChangedEventArgs(Info? previousInfo, Info? info) : EventArgs
{
    public readonly Info? PreviousInfo = previousInfo;
    public readonly Info? Info = info;
}