using Tmds.MDns;

namespace DevPieter.WLED_SDK.Discoverer.DiscovererEventArgs;

public class WledDeviceFoundEventArgs(ServiceAnnouncement announcement) : EventArgs
{
    public ServiceAnnouncement Announcement { get; } = announcement;

    public string? FirstIpAddress()
        => Announcement.Addresses.FirstOrDefault()?.ToString();
}