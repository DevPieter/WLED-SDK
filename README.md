# WLED-SDK: .NET Wrapper for WLED

An unofficial .NET Wrapper for the [WLED](https://kno.wled.ge/) [JSON API](https://kno.wled.ge/interfaces/json-api/), enabling real-time communication with WLED devices via WebSockets ~~or HTTP~~. Designed to simplify control and interaction with WLED devices using C#.

> [!IMPORTANT]
> This library is not officially supported or affiliated with WLED or its creator. For official information and support, please refer to the [WLED website](https://kno.wled.ge/) or the [GitHub repository](https://github.com/Aircoookie/WLED) maintained by the original developers.

## Features

1. **Realtime Communication**: Send and receive data from WLED devices in real-time using WebSockets.
2. **Network Discovery**: Automatically discover WLED devices on the local network using mDNS.
3. **Easy to Use**: Simple and intuitive extension methods for controlling WLED devices.

## Compatibility

This library is compatible with [WLED v0.15.x](https://github.com/Aircoookie/WLED/releases/tag/v0.15.0) and later.

## Installation

This is Work in Progress, so the package is not yet available on NuGet. You can clone the repository and build the project locally.

## Example Usage

Here’s a simple example of how you can use the WLED-SDK to control a WLED device.

```csharp
// Create a new WLED WebSocket client for the specified device (by IP or hostname)
//  We use the 'using' statement to ensure the client is properly disposed of when we're done
using var client = new WledWebsocketClient("[Your WLED Device IP or Hostname]");

// Connect to the WLED device
//  This will wait until the WLED device sends it's initial state (can be disabled by passing false)
await client.ConnectAsync();

Console.WriteLine($"Connected to {client.Info!.Name} running WLED {client.Info.Version} ({client.Info.VersionId})");

// Turn on the WLED device
await client.TurnOnAsync();

// Set the brightness to 50% (0-255)
await client.SetBrightnessAsync(128);

// Set an effect (e.g., Rainbow) (https://kno.wled.ge/features/effects/)
await client.SetEffectAsync(9);

// Set a palette (e.g., Default) (https://kno.wled.ge/features/palettes/)
await client.SetPaletteAsync(0);

await Task.Delay(1000);

// Stop the connection to the WLED device
await client.StopAsync();
```

## License

This project is licensed under the [MIT License](https://github.com/DevPieter/WLED-SDK/blob/main/LICENSE). Feel free to explore, contribute, and build upon it!

## Dependencies

The WLED-SDK relies on the following libraries:

- [**Websocket.Client**](https://www.nuget.org/packages/Websocket.Client): Used for WebSocket communication.
- [**Newtonsoft.Json**](https://www.nuget.org/packages/Newtonsoft.Json): Used for JSON serialization and deserialization.
- [**Tmds.MDns**](https://www.nuget.org/packages/Tmds.MDns): Used for WLED device discovery using mDNS.
