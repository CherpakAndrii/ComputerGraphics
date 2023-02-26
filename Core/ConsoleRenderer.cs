using Core.Lights;

namespace Core;

public static class ConsoleRenderer
{
    public static void PrintToConsole(Color[,] pixels)
    {
        for (int  i = 0; i < pixels.GetLength(0); i++)
        {
            for (int j = 0; j < pixels.GetLength(1); j++)
            {
                Console.Write("\x1b[38;2;" + pixels[i, j].R + ";" + pixels[i, j].G + ";" + pixels[i, j].B + "m" + "██");
            }
            Console.Write('\n');
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}
