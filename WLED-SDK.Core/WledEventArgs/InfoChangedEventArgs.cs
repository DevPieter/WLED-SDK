using DevPieter.WLED_SDK.Core.Models.WledInfo;

namespace DevPieter.WLED_SDK.Core.WledEventArgs;

public class InfoChangedEventArgs(Info? previousInfo, Info? info) : EventArgs
{
    public Info? PreviousInfo { get; } = previousInfo;
    public Info? Info { get; } = info;
}