using DevPieter.WLED_SDK.Core.Models.WledState;

namespace WLED_SDK.Examples.ConsoleApp;

public static class PrintHelper
{
    public static void PrintState(State? state, int top, int left, int maxHeight)
    {
        if (state is null)
        {
            for (var i = top; i < maxHeight; i++) ClearLine(i, left);
            Print("State is null!", top, left, ConsoleColor.Red);

            return;
        }

        Print("State:", top, left);
        Print(state.On ? "On" : "Off", top, left + 7, state.On ? ConsoleColor.Green : ConsoleColor.Red);

        Print("Brightness:", top + 1, left);
        Print($"{state.Brightness}", top + 1, left + 12, ConsoleColor.Blue);

        for (var i = top + 2; i < maxHeight; i++) ClearLine(i, left);
        var newTop = top + 3;

        foreach (var segment in state.Segments)
        {
            /* Name */
            Print($"{segment.Id,-1} | {segment.Name ?? "Unnamed"}", newTop, left, segment.Id == state.MainSegmentId ? ConsoleColor.Cyan : ConsoleColor.White);

            newTop++;
            /* State */
            Print("  | State:", newTop, left);
            Print(segment.On ? "On" : "Off", newTop, left + 11, segment.On ? ConsoleColor.Green : ConsoleColor.Red);

            newTop++;
            /* Effect */
            Print("  | Effect:", newTop, left);
            Print($"{segment.EffectId}", newTop, left + 12, ConsoleColor.Blue);

            newTop++;
            /* Palette */
            Print("  | Palette:", newTop, left);
            Print($"{segment.PaletteId}", newTop, left + 13, ConsoleColor.Blue);

            newTop++;
            /* Brightness */
            Print("  | Brightness:", newTop, left);
            Print($"{segment.Brightness}", newTop, left + 16, ConsoleColor.Blue);
            
            newTop++;
            /* Colors */
            Print("  | Colors:", newTop, left);

            var primary = segment.Colors[0].Select(c => c.ToString("X2")).Aggregate((a, b) => a + b);
            var secondary = segment.Colors[1].Select(c => c.ToString("X2")).Aggregate((a, b) => a + b);
            var tertiary = segment.Colors[2].Select(c => c.ToString("X2")).Aggregate((a, b) => a + b);

            Print($"#{primary} - #{secondary} - #{tertiary}", newTop, left + 12, ConsoleColor.Blue);

            newTop += 2;
            if (newTop < maxHeight) continue;
            Print("...", newTop, left);
            break;
        }
    }

    public static void Print(string text, int top, int left, ConsoleColor color = ConsoleColor.White)
    {
        var currentTop = Console.CursorTop;
        var currentLeft = Console.CursorLeft;

        Console.SetCursorPosition(left, top);
        ClearLine(top, left);

        Console.ForegroundColor = color;
        Console.WriteLine(text);

        Console.ResetColor();
        Console.SetCursorPosition(currentLeft, currentTop);
    }

    public static void ClearLine(int top, int left)
    {
        var currentTop = Console.CursorTop;
        var currentLeft = Console.CursorLeft;

        Console.SetCursorPosition(left, top);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(currentLeft, currentTop);
    }
}