using System.Text;
using System.Text.RegularExpressions;
using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;

namespace Reader.PPM;

public class PpmFileReader : IImageReader
{
    public Color[,] ImageToPixels(byte[] fileData)
    {
        var sr = new StreamReader(new MemoryStream(fileData));
        var second = sr.ReadLine()![1];
        sr.Close();
        return second == '3' ? P3ToPixels(fileData) : P6ToPixels(fileData);
    }
    
    public bool ValidateFileStructure(byte[] fileData)
    {
        using var sr = new StreamReader(new MemoryStream(fileData));
        var firstChars = sr.ReadLine()!;

        return firstChars is ['P', _, ..]
               && firstChars[1] switch
               {
                   '3' => ValidateP3Structure(fileData),
                   '6' => ValidateP6Structure(fileData),
                   _ => false
               };
    }

    public string FileExtension => "ppm";

    private static bool ValidateP3Structure(byte[] fileData)
    {
        string filedata;
        using var sr = new StreamReader(new MemoryStream(fileData));
        filedata = sr.ReadToEnd();
        var words = filedata.Split(new []{' ', '\n', '\r', '\t'}, StringSplitOptions.RemoveEmptyEntries);
        
        if (words.Length < 7 || !int.TryParse(words[1], out var width) ||
            !int.TryParse(words[2], out var height) || !int.TryParse(words[3], out var maxColor) || 
            words.Length != width*height*3+4)
            return false;
        
        for (var i = 4; i < words.Length; i++)
        {
            if (!int.TryParse(words[i], out var colorData) || colorData < 0 || colorData > maxColor)
                return false;
        }

        return true;
    }
    
    private static bool ValidateP6Structure(byte[] fileData)
    {
        var br = new BinaryReader(new MemoryStream(fileData));
        string[] header = (Encoding.ASCII.GetString(br.ReadBytes(20)))
            .Split(new char[]{' ', '\n', '\r', '\t'}, StringSplitOptions.RemoveEmptyEntries);
        br.Close();
        
        if (header.Length < 4) return false;

        var headerLength = 0;
        for (var i = 0; i < 4; i++) headerLength += header[i].Length + 1;

        return fileData.Length >= 12 && int.TryParse(header[1], out var width) &&
               int.TryParse(header[2], out var height) && int.TryParse(header[3], out _) && 
               fileData.Length >= width*height*3+headerLength;
    }

    private static Color[,] P3ToPixels(byte[] fileData)
    {
        var sr = new StreamReader(new MemoryStream(fileData));
        var filedata = sr.ReadToEnd();
        sr.Close();
        
        string[] words = filedata.Split(new char[]{' ', '\n', '\r', '\t'}, StringSplitOptions.RemoveEmptyEntries);

        int width = int.Parse(words[1]);
        int height = int.Parse(words[2]);
        int maxColorValue = int.Parse(words[3]);
        double colorCoefficient = 255.0 / maxColorValue;

        var picture = new Color[height, width];
        
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

    private static Color[,] P6ToPixels(byte[] fileData)
    {
        BinaryReader br = new BinaryReader(new MemoryStream(fileData));
        string header = Encoding.ASCII.GetString(br.ReadBytes(20));
            string[] headerElements = header.Split(new char[]{' ', '\n', '\r', '\t'}, StringSplitOptions.RemoveEmptyEntries);
        br.Close();
            
        int width = int.Parse(headerElements[1]);
        int height = int.Parse(headerElements[2]);
        int maxColorValue = int.Parse(headerElements[3]);
        double colorCoefficient = 255.0 / maxColorValue;

        int headerLength = Regex.Match(header, @"^P6\s+(?:\d+\s+){3}").Length;

        Color[,] picture = new Color[height, width];
        
        for (int i = 0, ptr = headerLength; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                picture[i, j] = new Color(
                    (byte)(fileData[ptr++]*colorCoefficient), 
                    (byte)(fileData[ptr++]*colorCoefficient), 
                    (byte)(fileData[ptr++]*colorCoefficient));
            }
        }

        return picture;
    }
}