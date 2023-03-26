using Core.Lights;

namespace ImageFormatConverter.Abstractions.Interfaces;

public interface IImageReader
{
    public Color[,] ImageToPixels(byte[] fileData);

    public bool ValidateFileStructure(byte[] fileData);

    public string FileExtension { get; }
}