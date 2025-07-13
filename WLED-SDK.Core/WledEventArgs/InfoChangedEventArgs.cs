using DevPieter.WLED_SDK.Core.Models.WledInfo;

namespace DevPieter.WLED_SDK.Core.WledEventArgs;

public class InfoChangedEventArgs : EventArgs
{
    public readonly Info? PreviousInfo;
    public readonly Info? Info;

    public InfoChangedEventArgs(Info? previousInfo, Info? info)
    {
        PreviousInfo = previousInfo;
        Info = info;
    }
}