using System.Text;
using System.Text.RegularExpressions;
using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;

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
    
    public bool ValidateFileStructure(string filename)
    {
        StreamReader sr = new StreamReader(filename);
        string firstChars = sr.ReadLine()!;
        sr.Close();
        
        return firstChars.Length >= 2 && firstChars[0] == 'P' && (firstChars[1] == '3' ? ValidateP3Structure(filename) :
            firstChars[1] == '6' && ValidateP6Structure(filename));
    }

    public string FileExtension => "ppm";

    private bool ValidateP3Structure(string filename)
    {
        string filedata;
        using (StreamReader sr = new StreamReader(filename))
        {
            filedata = sr.ReadToEnd();
        }
        string[] words = filedata.Split(new char[]{' ', '\n', '\r', '\t'}, StringSplitOptions.RemoveEmptyEntries);
        
        if (words.Length < 7 || !Int32.TryParse(words[1], out int width) ||
            !Int32.TryParse(words[2], out int height) || !Int32.TryParse(words[3], out int maxColor) || 
            words.Length != width*height*3+4)
            return false;
        
        for (int i = 4; i < words.Length; i++)
        {
            if (!Int32.TryParse(words[i], out var colorData) || colorData < 0 || colorData > maxColor)
                return false;
        }

        return true;
    }
    
    private bool ValidateP6Structure(string filename)
    {
        BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));
        string[] header = (Encoding.ASCII.GetString(br.ReadBytes(20)))
            .Split(new char[]{' ', '\n', '\r', '\t'}, StringSplitOptions.RemoveEmptyEntries);
        br.Close();
        
        if (header.Length < 4) return false;

        int headerLength = 0;
        for (int i = 0; i < 4; i++) headerLength += header[i].Length + 1;
        
        byte[] filedata = File.ReadAllBytes(filename);
        
        if (filedata.Length < 12 || !Int32.TryParse(header[1], out int width) ||
            !Int32.TryParse(header[2], out int height) || !Int32.TryParse(header[3], out int maxColor) || 
            filedata.Length < width*height*3+headerLength)
            return false;

        return true;
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

        Color[,] picture = new Color[height, width];
        
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
        Color[,] picture = new Color[height, width];
        
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