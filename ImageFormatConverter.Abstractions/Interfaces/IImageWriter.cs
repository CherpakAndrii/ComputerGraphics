using Core.Lights;

namespace ImageFormatConverter.Abstractions.Interfaces;

public interface IImageWriter
{
    public void WriteToFile(string outputFileName, Color[,] pixels);

    public string FileExtension { get; }
}