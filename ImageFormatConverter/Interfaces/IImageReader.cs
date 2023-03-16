using Core.Lights;

namespace ImageFormatConverter.Interfaces;

public interface IImageReader
{
    public Color[,] ImageToPixels(string filename);
}