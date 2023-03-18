using System.Text;
using System.Text.RegularExpressions;
using Core.Lights;
using ImageFormatConverter.Interfaces;

namespace ImageFormatConverter.Readers;

public class PpmFileReader : IImageReader
{
    public Color[,] ImageToPixels(string filename)
    {
        StreamReader sr = new StreamReader(filename);
        char second = sr.ReadLine()![1];
        sr.Close();
        return second == '3' ? P3ToPixels(filename) : P6ToPixels(filename);
    }

    private Color[,] P3ToPixels(string filename)
    {
        StreamReader sr = new StreamReader(filename);
        string filedata = sr.ReadToEnd();
        sr.Close();
        
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
        BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));
        string header = (Encoding.ASCII.GetString(br.ReadBytes(20)));
            string[] headerElements = header.Split(new char[]{' ', '\n', '\r', '\t'}, StringSplitOptions.RemoveEmptyEntries);
        br.Close();
            
        int width = int.Parse(headerElements[1]);
        int height = int.Parse(headerElements[2]);
        int maxColorValue = int.Parse(headerElements[3]);
        double colorCoefficient = 255.0 / maxColorValue;

        int headerLength = Regex.Match(header, @"^P6\s+(?:\d+\s+){3}").Length;

        byte[] filedata = File.ReadAllBytes(filename);
        Color[,] picture = new Color[width, height];
        
        for (int i = 0, ptr = headerLength; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                picture[i, j] = new Color(
                    (byte)(filedata[ptr++]*colorCoefficient), 
                    (byte)(filedata[ptr++]*colorCoefficient), 
                    (byte)(filedata[ptr++]*colorCoefficient));
            }
        }

        return picture;
    }
}