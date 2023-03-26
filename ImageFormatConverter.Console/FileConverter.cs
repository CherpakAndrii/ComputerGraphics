using ImageFormatConverter.Abstractions.Interfaces;

namespace ImageFormatConverter.Console;

public class FileConverter
{
    private IImageWriter _imageWriter;
    private IImageReader _imageReader;
    
    public FileConverter(IImageReader imageReader, IImageWriter imageWriter)
    {
        _imageReader = imageReader;
        _imageWriter = imageWriter;
    }

    public byte[] ConvertImage(byte[] imageData)
    {
        var image = _imageReader.ImageToPixels(imageData);
        return _imageWriter.WriteToFile(image);
    }
}