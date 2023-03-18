namespace ImageFormatConverter.Validators;

public class BasicValidator
{
    private string[] _supportedFormats = { "bmp", "ppm" };
    public bool CheckFileExistence(string filename)
    {
        return !String.IsNullOrWhiteSpace(filename) && File.Exists(filename);
    }
    
    public bool CheckFileExtension(string filename)
    {
        return filename.Contains('.') && _supportedFormats.Contains(filename.Split('.')[^1]);
    }
}