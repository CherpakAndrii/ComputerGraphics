using System.Text;
using ImageFormatConverter.Interfaces;

namespace ImageFormatConverter.Validators;

public class PpmStructureValidator : IImageFileStructureValidator
{
    public bool ValidateFileStructure(string filename)
    {
        StreamReader sr = new StreamReader(filename);
        string firstChars = sr.ReadLine()!;
        sr.Close();
        
        return firstChars.Length >= 2 && firstChars[0] == 'P' && (firstChars[1] == '3' ? ValidateP3Structure(filename) :
            firstChars[1] == '6' && ValidateP6Structure(filename));
    }

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
}