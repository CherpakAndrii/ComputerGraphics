namespace ImageFormatConverter.Console;

public class ConverterApp
{
    private readonly string _source;
    private readonly string _goalFormat;
    private readonly string? _output;
    public ConverterApp(string source, string goalFormat, string? output)
    {
        _source = source;
        _goalFormat = goalFormat;
        _output = output;
    }
    
    public void Run()
    {
        var fileData = FileReader.ReadFile(_source);

        var fileFactory = new FileFactory();
        var fileConverter = fileFactory.CreateFileConverter(fileData, Path.GetExtension(_source), _goalFormat);
        var targetFileData = fileConverter.ConvertImage(fileData);

        var fileWriter = new FileWriter(_source);

        fileWriter.Write(targetFileData, _goalFormat, _output);
    }
}