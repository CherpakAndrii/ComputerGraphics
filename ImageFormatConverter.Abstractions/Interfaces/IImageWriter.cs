using Core.Lights;

namespace ImageFormatConverter.Abstractions.Interfaces;

public interface IImageWriter
{
    public byte[] WriteToFile(Color[,] pixels);

    public string FileExtension { get; }
}