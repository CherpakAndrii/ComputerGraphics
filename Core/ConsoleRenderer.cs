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
        if (mostIntenseColor <= 5) return ' ';
        if (mostIntenseColor <= 50) return '.';
        if (mostIntenseColor <= 125) return '*';
        if (mostIntenseColor <= 205) return 'O';
        else return '#';
    }

    private static ConsoleColor GetConsoleColor(Color color)
    {
        if (color.R >= 160 && color.B >= 160 && color.G >= 160) return ConsoleColor.White;
        if (color.R - color.B > 100 && color.R - color.G > 100)
        {
            if (color.R >= 150) return ConsoleColor.Red;
            return ConsoleColor.DarkRed;
        }
        if (color.B - color.R > 100 && color.B - color.G > 100)
        {
            if (color.B >= 150) return ConsoleColor.Blue;
            return ConsoleColor.DarkBlue;
        }
        if (color.G - color.R > 100 && color.G - color.B > 100)
        {
            if (color.G >= 150) return ConsoleColor.Green;
            return ConsoleColor.DarkGreen;
        }
        if (color.R >= 50 && color.B >= 50 && color.G >= 50) return ConsoleColor.Gray;
        return ConsoleColor.DarkGray;
    }
}
