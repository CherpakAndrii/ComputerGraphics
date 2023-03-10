using Core.Lights;
namespace RenderApp;

public class ConsoleRenderer
{
    public ConsoleRenderer(bool retro, bool retroColor)
    {
        Retro = retro;
        RetroColor = retroColor;
    }
    public bool Retro { get; }
    public bool RetroColor { get; }
    public void PrintToConsole(Color[,] pixels)
    {
        Console.ForegroundColor = ConsoleColor.White;
        for (int  i = 0; i < pixels.GetLength(0); i++)
        {
            for (int j = 0; j < pixels.GetLength(1); j++)
            {
                if (Retro)
                {
                    Console.ForegroundColor = ColorConverter.GetConsoleColor(pixels[i, j], RetroColor);
                    Console.Write(ColorConverter.ColorToSymbol(pixels[i, j]));
                }
                else Console.Write("\x1b[38;2;" + pixels[i, j].R + ";" + pixels[i, j].G + ";" + pixels[i, j].B + "m" + "██");
            }
            Console.Write('\n');
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}
