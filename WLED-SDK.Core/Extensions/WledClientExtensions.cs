using DevPieter.WLED_SDK.Core.Models.WledInfo;
using DevPieter.WLED_SDK.Core.Models.WledState;

namespace DevPieter.WLED_SDK.Core.Extensions;

public static class WledClientExtensions
{
    public static State GetStateOrThrow(this IWledClient client)
        => client.State ?? throw new InvalidOperationException("Client is not connected or the state has not been received yet.");

    public static Info GetInfoOrThrow(this IWledClient client)
        => client.Info ?? throw new InvalidOperationException("Client is not connected or the info has not been received yet.");
}