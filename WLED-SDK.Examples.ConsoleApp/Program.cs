using DevPieter.WLED_SDK.Builders.Extensions;
using DevPieter.WLED_SDK.Client.WebSocket;
using DevPieter.WLED_SDK.Extensions.State;
using WLED_SDK.Examples.ConsoleApp;

/* ====== Console Settings (IGNORE) ====== */
var inputTop = Console.WindowHeight - 2;
var errorTop = inputTop - 10;
var maxStateHeight = errorTop - 2;

Console.Clear();
Console.Title = "WLED SDK - Example Console App";
Console.CursorTop = inputTop;

Console.CursorLeft = 0;
Console.Write("> ");
Console.CursorLeft = 2;
/* ======================================= */

using var client = new WledWebsocketClient("ledstrip.local");
client.OnStateChanged += (sender, eventArgs) => PrintHelper.PrintState(eventArgs.State, 1, 1, maxStateHeight);
client.OnDisconnected += (sender, eventArgs) => PrintHelper.Print($"Disconnected: {eventArgs.Type}", errorTop, 1, ConsoleColor.Red);

try
{
    await client.ConnectAsync();
}
catch (Exception e)
{
    PrintHelper.Print(e.Message, errorTop, 1, ConsoleColor.Red);
    return;
}

while (client.IsConnected)
{
    var input = Console.ReadLine();
    if (input is null) continue;

    Console.CursorTop = inputTop;
    Console.Write(new string(' ', Console.WindowWidth));
    Console.CursorTop = inputTop;

    Console.CursorLeft = 0;
    Console.Write("> ");
    Console.CursorLeft = 2;

    var command = input.Split(' ')[0];
    var commandArgs = input.Split(' ').Skip(1).ToArray();

    try
    {
        switch (command)
        {
            /* On/Off */
            case "on":
                await client.TurnOnAsync();
                break;

            case "off":
                await client.TurnOffAsync();
                break;

            case "toggle":
                await client.ToggleOnOffAsync();
                break;

            /* Brightness */
            case "brightness":
            case "bri":
                if (commandArgs.Length == 0) throw new ArgumentException("brightness [0-255]");
                if (!int.TryParse(commandArgs[0], out var brightness)) throw new ArgumentException("Brightness must be an integer.");

                await client.SetBrightnessAsync(brightness);
                break;

            case "brightness_increase":
            case "bri_inc":
                if (commandArgs.Length == 0) throw new ArgumentException("brightness_increase [0-255]");
                if (!int.TryParse(commandArgs[0], out var brightnessInc))
                    throw new ArgumentException("Brightness increment must be an integer.");

                await client.IncreaseBrightnessAsync(brightnessInc);
                break;

            case "brightness_decrease":
            case "bri_dec":
                if (commandArgs.Length == 0) throw new ArgumentException("brightness_decrease [0-255]");
                if (!int.TryParse(commandArgs[0], out var brightnessDec))
                    throw new ArgumentException("Brightness decrement must be an integer.");

                await client.DecreaseBrightnessAsync(brightnessDec);
                break;

            /* Transition */
            case "transition":
            case "trans":
                if (commandArgs.Length == 0) throw new ArgumentException("transition [0-65535]");
                if (!int.TryParse(commandArgs[0], out var transition))
                    throw new ArgumentException("Transition duration must be an integer.");

                await client.SetTransitionDurationAsync(transition);
                break;

            case "transition_client":
            case "trans_client":
                if (commandArgs.Length == 0) throw new ArgumentException("transition_client [0-65535]");
                if (!int.TryParse(commandArgs[0], out var transitionClient)) throw new ArgumentException("Transition duration must be an integer.");

                client.ClientTransitionTime = transitionClient;
                break;

            /* Presets */
            case "preset":
                if (commandArgs.Length == 0) throw new ArgumentException("preset [0-?]");
                if (!int.TryParse(commandArgs[0], out var preset)) throw new ArgumentException("Preset must be an integer.");

                await client.LoadPresetAsync(preset);
                break;

            case "preset_range":
                if (commandArgs.Length == 0) throw new ArgumentException("preset_range [0-?] [0-?] {random}");
                if (!int.TryParse(commandArgs[0], out var presetStart)) throw new ArgumentException("Start preset must be an integer.");
                if (!int.TryParse(commandArgs[1], out var presetEnd)) throw new ArgumentException("End preset must be an integer.");
                var presetRandom = commandArgs is [_, _, "random"];

                await client.LoadPresetAsync(presetStart, presetEnd, presetRandom);
                break;

            /* Effects */
            case "effect":
                if (commandArgs.Length == 0) throw new ArgumentException("effect [0-?] {segment segment ...}");
                if (!int.TryParse(commandArgs[0], out var effect)) throw new ArgumentException("Effect must be an integer.");
                var segments = commandArgs.Skip(1).Select(int.Parse).ToArray();

                await client.SetEffectAsync(effect, segments.Length > 0 ? segments : null);
                break;

            /* Palettes */
            case "palette":
                if (commandArgs.Length == 0) throw new ArgumentException("palette [0-?] {segment segment ...}");
                if (!int.TryParse(commandArgs[0], out var palette)) throw new ArgumentException("Palette must be an integer.");
                var paletteSegments = commandArgs.Skip(1).Select(int.Parse).ToArray();

                await client.SetPaletteAsync(palette, paletteSegments.Length > 0 ? paletteSegments : null);
                break;

            /* Misc */
            case "exit":
                await client.StopAsync();
                break;
        }
    }
    catch (Exception e)
    {
        PrintHelper.Print(e.Message, errorTop, 1, ConsoleColor.Red);
    }
}