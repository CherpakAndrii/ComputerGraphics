using System.Reflection;
using ImageFormatConverter.Abstractions.Interfaces;

namespace ImageFormatConverter.Console;

public class FileFactory
{
    private const string PluginsPath = "..\\..\\..\\..\\ImagePlugins\\net7.0";
    private const string SearchPattern = "*.dll";

    private IImageWriter[] _imageWriters;
    private IImageReader[] _imageReaders;

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
    public IImageReader? GetImageReader(byte[] fileData)
    {
        return _imageReaders.FirstOrDefault(reader => reader.ValidateFileStructure(fileData));
    }
    
    public IImageWriter? GetImageWriter(string goalFormat)
    {
        return _imageWriters.FirstOrDefault(writer => writer.FileExtension == goalFormat);
    }

    public string[] GetSupportedReadersExtensions()
    {
        return _imageReaders.Select(reader => reader.FileExtension).ToArray();
    }
    
    public string[] GetSupportedWritersExtensions()
    {
        return _imageWriters.Select(reader => reader.FileExtension).ToArray();
    }
}