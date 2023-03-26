using System.Reflection;
using ImageFormatConverter.Abstractions.Interfaces;

namespace ImageFormatConverter.Console;

public class FileFactory
{
    public IImageReader? GetImageReader(byte[] fileData)
    {
        const string path = @"C:\Users\Acer\Documents\computerGraphics\ComputerGraphics\ImagePlugins\net7.0";

        var allAssemblies = Directory.GetFiles(path, "*.dll")
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
        const string path = @"C:\Users\Acer\Documents\computerGraphics\ComputerGraphics\ImagePlugins\net7.0";

        var allAssemblies = Directory.GetFiles(path, "*.dll")
            .Select(Assembly.LoadFile).ToList();
        
        var writers = allAssemblies
            .SelectMany(s => s.GetTypes())
            .Where(type => typeof(IImageWriter) .IsAssignableFrom(type))
            .Select(type => (IImageWriter)Activator.CreateInstance(type)!)
            .ToArray();
        
        return writers.FirstOrDefault(writer => writer.FileExtension == goalFormat);
    }
}