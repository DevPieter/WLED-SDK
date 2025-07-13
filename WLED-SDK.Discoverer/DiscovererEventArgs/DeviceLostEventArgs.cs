using Tmds.MDns;

namespace DevPieter.WLED_SDK.Discoverer.DiscovererEventArgs;

public class DeviceLostEventArgs : EventArgs
{
    public readonly ServiceAnnouncement Announcement;

    public DeviceLostEventArgs(ServiceAnnouncement announcement)
    {
        Announcement = announcement;
    }
}