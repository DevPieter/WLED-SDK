using Tmds.MDns;

namespace DevPieter.WLED_SDK.Discoverer.DiscovererEventArgs;

public class WledDeviceFoundEventArgs(ServiceAnnouncement announcement) : EventArgs
{
    public readonly ServiceAnnouncement Announcement = announcement;
}