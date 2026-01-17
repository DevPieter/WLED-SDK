using DevPieter.WLED_SDK.Core.Models.WledState;

namespace DevPieter.WLED_SDK.Core.WledEventArgs;

public class StateChangedEventArgs(State? previousState, State? state) : EventArgs
{
    public State? PreviousState { get; } = previousState;
    public State? State { get; } = state;
}