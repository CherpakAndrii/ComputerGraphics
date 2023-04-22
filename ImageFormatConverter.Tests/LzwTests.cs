using System.ComponentModel.DataAnnotations;
using ImageFormatConverter.Console;
using Reader.GIF;
using Writer.GIF;
using Lzw = Reader.GIF.Lzw;

namespace ImageFormatConverter.Tests;

public class LzwTests
{
    [Test]
    public void LzwTest()
    {

        var decompressor = new Reader.GIF.Lzw();
        var compressor = new Writer.GIF.Lzw();

        var gifReader = new GifFileReader();
        
        var fileData = FileReader.ReadFile("C:\\Users\\Acer\\Downloads\\test\\screenshot.gif");
        var image = gifReader.ImageToPixels(fileData);

        var compressedData = gifReader.CompressedData;
        var code = gifReader.LzwCode;

        var decompressedData = decompressor.Decompress(compressedData, code);

        var compressedActual = compressor.Сompress(decompressedData, code);

        Assert.That(compressedData, Is.EquivalentTo(compressedActual));
    }
    
}