using Core.Lights;

namespace RenderApp.ConsoleOutput;

public static class ColorConverter
{
    public static char ColorToSymbol(Color color)
    {
        int mostIntenseColor = int.Max(color.R, int.Max(color.G, color.B));
        return mostIntenseColor switch
        {
            <= 5 => ' ',
            <= 50 => '.',
            <= 125 => '*',
            <= 205 => 'O',
            _ => '#'
        };
    }

    public static ConsoleColor GetConsoleColor(Color color, bool retroColor)
    {
        if (!retroColor || color is { R: >= 160, B: >= 160, G: >= 160 })
            return ConsoleColor.White;
        if (color.R - color.B > 100 && color.R - color.G > 100)
        {
            return color.R >= 150
                ? ConsoleColor.Red
                : ConsoleColor.DarkRed;
        }
        if (color.B - color.R > 100 && color.B - color.G > 100)
        {
            return color.B >= 150
                ? ConsoleColor.Blue
                : ConsoleColor.DarkBlue;
        }
        if (color.G - color.R > 100 && color.G - color.B > 100)
        {
            return color.G >= 150
                ? ConsoleColor.Green
                : ConsoleColor.DarkGreen;
        }
        return color is { R: >= 50, B: >= 50, G: >= 50 }
            ? ConsoleColor.Gray
            : ConsoleColor.DarkGray;
    }
}