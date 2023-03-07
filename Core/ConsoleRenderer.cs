using Core.Lights;

namespace Core;

public static class ConsoleRenderer
{
    public static bool Retro { get; set; } = true;

    public static void PrintToConsole(Color[,] pixels)
    {
        for (int  i = 0; i < pixels.GetLength(0); i++)
        {
            for (int j = 0; j < pixels.GetLength(1); j++)
            {
                if (Retro)
                {
                    Console.ForegroundColor = GetConsoleColor(pixels[i, j]);
                    Console.Write(ColorToSymbol(pixels[i, j]));
                }
                else Console.Write("\x1b[38;2;" + pixels[i, j].R + ";" + pixels[i, j].G + ";" + pixels[i, j].B + "m" + "██");
            }
            Console.Write('\n');
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static char ColorToSymbol(Color color)
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

    private static ConsoleColor GetConsoleColor(Color color)
    {
        if (color is { R: >= 160, B: >= 160, G: >= 160 })
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
