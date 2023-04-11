namespace ImageFormatConverter.Console;

public class FileWriter
{
    private readonly string _defaultPath;
    
    public FileWriter(string defaultPath)
    {
        _defaultPath = defaultPath;
    }

    public void Write(byte[] targetFileData, string goalFormat, string? outputPathWithName)
    {
        var path = outputPathWithName is not null
            ? outputPathWithName + goalFormat
            : Path.ChangeExtension(_defaultPath, goalFormat);

        File.WriteAllBytes(path, targetFileData);
    }
    
    public void Write(byte[] targetFileData, string goalFormat, string outputPathWithName, string fileName)
    {
        Write(targetFileData, goalFormat, $"{outputPathWithName}\\{fileName}");
    }
} 