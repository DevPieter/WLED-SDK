/*
 * This example shows how to use the WLED SDK to discover WLED devices on the network.
 *
 * We will use the WledDeviceDiscoverer to discover WLED devices on the network.
 * When a device is found, we will connect to it using the WledWebsocketClient and toggle its state on and off.
 */

/* ====== Console Settings (IGNORE) ====== */

using DevPieter.WLED_SDK.Client.WebSocket;
using DevPieter.WLED_SDK.Discoverer;
using DevPieter.WLED_SDK.Extensions.State;

Console.Clear();
Console.Title = "WLED SDK - Discoverer Example";
/* ======================================= */

// Create a new WLED device discoverer. (We use 'using' to automatically dispose of the discoverer when we're done.)
using var wledDiscoverer = new WledDeviceDiscoverer();

// Subscribe to the OnDeviceFound event.
wledDiscoverer.OnDeviceFound += async (_, eventArgs) =>
{
    // Get the IP address of the found device.
    var ip = eventArgs.Announcement.Addresses.First().ToString();

    Console.WriteLine($"New WLED device found at {ip}! Trying to connect...");

    // Create a new WLED Websocket Client with the IP address of the found device.
    using var client = new WledWebsocketClient(ip);
    await client.ConnectAsync();

    Console.WriteLine($"Connected to {client.Info!.Name} running WLED {client.Info.Version} ({client.Info.VersionId}). Blinking the device...");

    // Set client transition time to 4 seconds. (For a smooth blink effect.)
    client.ClientTransitionTime = 4;

    // Toggle the device on and off.
    await client.ToggleOnOffAsync();
    await Task.Delay(1_000);
    await client.ToggleOnOffAsync();

    Console.WriteLine("Blinked the device. Stopping the client...");

    // Stop the client.
    await client.StopAsync();
};

// Subscribe to the OnDeviceLost event.
wledDiscoverer.OnDeviceLost += (_, eventArgs) =>
{
    var ip = eventArgs.Announcement.Addresses.First().ToString();
    Console.WriteLine($"Lost WLED device at {ip}!");
};

// Start searching for WLED devices.
wledDiscoverer.Start();

Console.WriteLine("Searching for WLED devices...");
await Task.Delay(-1);