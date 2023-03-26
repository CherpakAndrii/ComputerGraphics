using System.Reflection;
using ImageFormatConverter.Abstractions.Interfaces;

namespace ImageFormatConverter.Console;

public class FileFactory
{
    private const string PluginsPath = "..\\..\\..\\..\\ImagePlugins\\net7.0";
    private const string SearchPattern = "*.dll";

    private readonly IImageWriter[] _imageWriters;
    private readonly IImageReader[] _imageReaders;

    public FileFactory()
    {
        var directoryInfo = new DirectoryInfo(PluginsPath);
        var allAssemblies = Directory.GetFiles(directoryInfo.FullName, SearchPattern)
            .Select(Assembly.LoadFile).ToList();
        
        _imageReaders = allAssemblies
            .SelectMany(s => s.GetTypes())
            .Where(type => typeof(IImageReader) .IsAssignableFrom(type))
            .Select(type => (IImageReader)Activator.CreateInstance(type)!)
            .ToArray();
        
        _imageWriters = allAssemblies
            .SelectMany(s => s.GetTypes())
            .Where(type => typeof(IImageWriter) .IsAssignableFrom(type))
            .Select(type => (IImageWriter)Activator.CreateInstance(type)!)
            .ToArray();
    }
    public IImageReader GetImageReader(byte[] fileData, string? inputExtension)
    {
        var reader = _imageReaders.FirstOrDefault(reader => reader.ValidateFileStructure(fileData));
        if (reader is not null)
            return reader;

        var supportedReaderFormats = GetSupportedReadersExtensions();

        if (string.IsNullOrEmpty(inputExtension))
        {
            throw new Exception($"Error: you are trying to open file with unsupported extension. Only {string.Join(" and ", supportedReaderFormats)} files are supported");
        }
        throw new Exception($"Error: you are trying to open {inputExtension} file, but only {string.Join(" and ", supportedReaderFormats)} files are supported");
        
    }
    
    public IImageWriter GetImageWriter(string goalFormat)
    {
        var writer = _imageWriters.FirstOrDefault(writer => writer.FileExtension == goalFormat);
        if (writer is not null)
            return writer;
        
        var supportedWriterFormats = GetSupportedWritersExtensions();
        throw new Exception($"Error: you are trying to write .{goalFormat} file, but only {string.Join(" and ", supportedWriterFormats)} files are supported");
    }

    private string[] GetSupportedReadersExtensions()
    {
        return _imageReaders.Select(reader => $".{reader.FileExtension}").ToArray();
    }

    private string[] GetSupportedWritersExtensions()
    {
        return _imageWriters.Select(reader => $".{reader.FileExtension}").ToArray();
    }
}