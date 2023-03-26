using System.Reflection;
using ImageFormatConverter.Abstractions.Interfaces;

namespace ImageFormatConverter.Console;

public class FileFactory
{
    private const string PluginsPath = "..\\..\\..\\..\\ImagePlugins\\net7.0";
    private const string SearchPattern = "*.dll";
    public IImageReader? GetImageReader(byte[] fileData)
    {
        var directoryInfo = new DirectoryInfo(PluginsPath);
        var allAssemblies = Directory.GetFiles(directoryInfo.FullName, SearchPattern)
            .Select(Assembly.LoadFile).ToList();
        
        var readers = allAssemblies
            .SelectMany(s => s.GetTypes())
            .Where(type => typeof(IImageReader) .IsAssignableFrom(type))
            .Select(type => (IImageReader)Activator.CreateInstance(type)!)
            .ToArray();
        
        return readers.FirstOrDefault(reader => reader.ValidateFileStructure(fileData));
    }
    
    public IImageWriter? GetImageWriter(string goalFormat)
    {
        var directoryInfo = new DirectoryInfo(PluginsPath);
        var allAssemblies = Directory.GetFiles(directoryInfo.FullName, SearchPattern)
            .Select(Assembly.LoadFile).ToList();
        
        var writers = allAssemblies
            .SelectMany(s => s.GetTypes())
            .Where(type => typeof(IImageWriter) .IsAssignableFrom(type))
            .Select(type => (IImageWriter)Activator.CreateInstance(type)!)
            .ToArray();
        
        return writers.FirstOrDefault(writer => writer.FileExtension == goalFormat);
    }
}