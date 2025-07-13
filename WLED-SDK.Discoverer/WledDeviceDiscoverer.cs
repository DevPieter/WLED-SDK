using System.Text;
using System.Text.Json;
using DevPieter.WLED_SDK.Discoverer.DiscovererEventArgs;
using Microsoft.Extensions.Logging;
using Tmds.MDns;

namespace DevPieter.WLED_SDK.Discoverer;

public class WledDeviceDiscoverer : IDisposable
{
    private readonly HttpClient _client;
    private readonly ServiceBrowser _browser;
    private readonly ILogger<WledDeviceDiscoverer>? _logger;

    private readonly Dictionary<string, ServiceAnnouncement> _foundDevices = new();

    /// <summary>
    /// Returns a dictionary of all found WLED devices.
    /// </summary>
    public IReadOnlyDictionary<string, ServiceAnnouncement> FoundDevices => _foundDevices;

    /// <summary>
    /// Invoked when a WLED device is found on the network.
    /// </summary>
    public event EventHandler<WledDeviceFoundEventArgs>? OnDeviceFound;

    /// <summary>
    /// Invoked when a WLED device is lost on the network.
    /// </summary>
    public event EventHandler<DeviceLostEventArgs>? OnDeviceLost;

    public WledDeviceDiscoverer(ILogger<WledDeviceDiscoverer>? logger = null)
    {
        _logger = logger;

        _client = new HttpClient();
        _browser = new ServiceBrowser();

        _browser.ServiceAdded += (sender, args) => _ = HandleFoundDeviceAsync(args.Announcement);
        _browser.ServiceRemoved += (sender, args) => _ = HandleLostDeviceAsync(args.Announcement);
    }

    /// <summary>
    /// Returns whether the browser is currently searching for WLED devices.
    /// </summary>
    public bool IsSearching => _browser.IsBrowsing;

    /// <summary>
    /// Starts searching for WLED devices on the network.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the browser is already searching for WLED devices.</exception>
    public void Start()
    {
        if (IsSearching) throw new InvalidOperationException("The browser is already searching for WLED devices.");
        _logger?.LogInformation("Starting the search for WLED devices.");

        try
        {
            _browser.StartBrowse("_wled._tcp");
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Failed to start searching for WLED devices.");
            throw;
        }
    }

    /// <summary>
    /// Stops searching for WLED devices on the network.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the browser is not searching for WLED devices.</exception>
    public void Stop()
    {
        if (!IsSearching) throw new InvalidOperationException("The browser is not searching for WLED devices.");
        _logger?.LogInformation("Stopping the search for WLED devices.");

        try
        {
            _browser.StopBrowse();
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Failed to stop browsing for WLED devices.");
            throw;
        }
    }

    /// <summary>
    /// Stops searching for WLED devices on the network and disposes the <see cref="HttpClient"/>.
    /// </summary>
    public void Dispose()
    {
        try
        {
            _browser.StopBrowse();
        }
        catch
        {
            // Ignored
        }

        _client.Dispose();
    }

    private async Task HandleFoundDeviceAsync(ServiceAnnouncement announcement)
    {
        var address = announcement.Addresses.First().ToString();

        _logger?.LogDebug("Found device at {Address}. Checking if it's a WLED device...", address);
        if (_foundDevices.ContainsKey(address) || !await IsWledDeviceAsync(address)) return;

        _logger?.LogInformation("Found WLED device at {Address}!", address);

        _foundDevices.Add(address, announcement);
        OnDeviceFound?.Invoke(this, new WledDeviceFoundEventArgs(announcement));
    }

    private async Task HandleLostDeviceAsync(ServiceAnnouncement announcement)
    {
        var address = announcement.Addresses.First().ToString();

        _logger?.LogDebug("Lost device at {Address}. Checking if it's a WLED device...", address);
        if (!_foundDevices.ContainsKey(address)) return;

        _logger?.LogInformation("Lost WLED device at {Address}!", address);

        _foundDevices.Remove(address);
        OnDeviceLost?.Invoke(this, new DeviceLostEventArgs(announcement));
    }

    private async Task<bool> IsWledDeviceAsync(string address)
    {
        var json = JsonSerializer.Serialize(new { v = true });
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var uri = new Uri($"http://{address}/json");
            var response = await _client.PostAsync(uri, body);

            try
            {
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var root = JsonSerializer.Deserialize<JsonElement>(content);

                root.GetProperty("state");
                root.GetProperty("info");
                root.GetProperty("effects");
                root.GetProperty("palettes");

                return true;
            }
            catch
            {
                return false;
            }
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Failed to check if device at {Address} is a WLED device.", address);
            return false;
        }
    }
}