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
        
        var compressedData = compressor.Сompress(new byte[] { 1, 2, 3, 4, 5}, 7);

        var decompressedData = decompressor.Decompress(compressedData, 7);

        for (int i = 0; i < decompressedData.Length; i++)
        {
        }
    }
    
}