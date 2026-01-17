using Tmds.MDns;

namespace DevPieter.WLED_SDK.Discoverer.DiscovererEventArgs;

public class DeviceLostEventArgs(ServiceAnnouncement announcement) : EventArgs
{
    public ServiceAnnouncement Announcement { get; } = announcement;
}