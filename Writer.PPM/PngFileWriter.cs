using Core.Lights;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ImageFormatConverter.Abstractions.Interfaces;
using System.Text;

namespace Writer.PNG;

public class PngFileWriter : IImageWriter
{
    public string FileExtension => "png";

    public byte[] WriteToFile(Color[,] pixels)
    {
        byte[] pngSignature = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

        byte[] ihdrChunkLength = BitConverter.GetBytes(13);
        byte[] ihdrChunkName = Encoding.ASCII.GetBytes("IHDR");
        byte[] imageWidth = BitConverter.GetBytes(pixels.GetLength(1));
        byte[] imageHeight = BitConverter.GetBytes(pixels.GetLength(0));
        byte bitDepth = 8;
        byte colorType = 2;


        throw new NotImplementedException();
    }

}