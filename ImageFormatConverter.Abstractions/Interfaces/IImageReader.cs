using Core.Lights;

namespace ImageFormatConverter.Abstractions.Interfaces;

public interface IImageReader
{
    public Color[,] ImageToPixels(string filename);

    public bool ValidateFileStructure(string filename);
}