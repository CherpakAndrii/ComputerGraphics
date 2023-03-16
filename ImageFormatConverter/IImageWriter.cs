using Core.Lights;

namespace ImageFormatConverter;

public interface IImageWriter
{
    public void WriteToFile(string inputFileName, Color[,] pixels);
}