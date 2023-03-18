using ImageFormatConverter.Interfaces;

namespace ImageFormatConverter.Validators;

public class PpmStructureValidator : IImageFileStructureValidator
{
    public bool ValidateFileStructure(string filename)
    {
        string filedata = new StreamReader(filename).ReadToEnd();
        string[] words = filedata.Split(new char[]{' ', '\n', '\r', '\t'}, StringSplitOptions.RemoveEmptyEntries);
        
        if (words.Length < 7 || words[0].ToLower() != "p3" || !Int32.TryParse(words[1], out int width) ||
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
}