namespace ImageFormatConverter.Console;

public class FileReader
{
    public static byte[] ReadFile(string path)
    {
        if (!File.Exists(path))
            throw new Exception("Source file doesn't exist");

        return File.ReadAllBytes(path);
    }
}