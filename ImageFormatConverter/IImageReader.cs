using Core.Lights;

namespace ImageFormatConverter;

public interface IImageReader
{
    public bool ValidateFileFormat(string filename);
    public Color[,] ImageToPixels(string filename);
}