using Core.Lights;
using ImageFormatConverter.Interfaces;

namespace ImageFormatConverter.Readers;

public class PpmFileReader : IImageReader
{
    public Color[,] ImageToPixels(string filename)
    {
        return new StreamReader(filename).ReadLine()![1] == '3' ? P3ToPixels(filename) : P6ToPixels(filename);
    }

    private Color[,] P3ToPixels(string filename)
    {
        string filedata = new StreamReader(filename).ReadToEnd();
        string[] words = filedata.Split(new char[]{' ', '\n', '\r', '\t'}, StringSplitOptions.RemoveEmptyEntries);

        int width = int.Parse(words[1]);
        int height = int.Parse(words[2]);
        int maxColorValue = int.Parse(words[3]);
        double colorCoefficient = 255.0 / maxColorValue;

        Color[,] picture = new Color[width, height];
        
        for (int i = 0, ptr = 4; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                picture[i, j] = new Color(
                    (byte)(int.Parse(words[ptr++]) * colorCoefficient),
                    (byte)(int.Parse(words[ptr++]) * colorCoefficient),
                    (byte)(int.Parse(words[ptr++]) * colorCoefficient));
            }
        }

        return picture; 
    }

    private Color[,] P6ToPixels(string filename)
    {
        throw new NotImplementedException();
    }
}