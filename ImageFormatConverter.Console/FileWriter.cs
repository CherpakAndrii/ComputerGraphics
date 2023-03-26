namespace ImageFormatConverter.Console;

public class FileWriter
{
    private readonly string _defaultPath;
    
    public FileWriter(string defaultPath)
    {
        _defaultPath = defaultPath;
    }

    public void Write(byte[] targetFileData, string goalFormat, string? outputPath)
    {
        var path = outputPath is not null
            ? outputPath + goalFormat
            : Path.ChangeExtension(_defaultPath, goalFormat);

        File.WriteAllBytes(path, targetFileData);
    }
} 