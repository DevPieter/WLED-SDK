using DevPieter.WLED_SDK.Core.Models.WledState;

namespace DevPieter.WLED_SDK.Core.WledEventArgs;

public class StateChangedEventArgs(State? previousState, State? state) : EventArgs
{
    public readonly State? PreviousState = previousState;
    public readonly State? State = state;
}