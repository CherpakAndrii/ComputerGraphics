using Core.Lights;
using System.Text;
using System.IO.Hashing;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ImageFormatConverter.Abstractions.Interfaces;

namespace Writer.PNG;

public class PngFileWriter : IImageWriter
{
    public string FileExtension => "png";

    public byte[] WriteToFile(Color[,] pixels)
    {
        byte[] pngSignature = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
        byte[] ihdrChunk = CreateDefaultIHDRChunk(pixels);
        byte[] idatChunk = CreateDefaultIDATChunk(pixels);
        byte[] iendChunk = CreateDefaultIENDChunk();
        return pngSignature.Concat(ihdrChunk).Concat(idatChunk).Concat(iendChunk).ToArray();
    }

    private static byte[] CreateDefaultIHDRChunk(Color[,] pixels)
    {
        byte[] ihdrChunkLength = BitConverter.GetBytes(13).Reverse().ToArray();
        byte[] ihdrChunkName = Encoding.ASCII.GetBytes("IHDR");
        byte[] imageWidth = BitConverter.GetBytes(pixels.GetLength(1)).Reverse().ToArray();
        byte[] imageHeight = BitConverter.GetBytes(pixels.GetLength(0)).Reverse().ToArray();
        byte bitDepth = 8;
        byte colorType = 2;
        byte compressType = 0;
        byte filtrationMethod = 0;
        byte interlaceMethod = 0;
        byte[] ihdrData = imageWidth.Concat(imageHeight)
                                    .Concat(new byte[] { bitDepth, colorType, compressType, filtrationMethod, interlaceMethod })
                                    .ToArray();
        byte[] ihdrCrc = Crc32.Hash(ihdrChunkName.Concat(ihdrData).ToArray());

        return ihdrChunkLength.Concat(ihdrChunkName).Concat(ihdrData).Concat(ihdrCrc).ToArray();
    }

    private static byte[] CreateDefaultIDATChunk(Color[,] pixels)
    {
        byte[] uncompressedData = new byte[pixels.GetLength(0) * pixels.GetLength(1) * 3 + pixels.GetLength(0)];
        int k = 0;
        for (int i = 0; i < pixels.GetLength(0); i++)
        {
            uncompressedData[k] = 0;
            k++;
            for (int j = 0; j < pixels.GetLength(1); j++)
            {
                uncompressedData[k] = BitConverter.GetBytes(pixels[i, j].R)[0];
                uncompressedData[k + 1] = BitConverter.GetBytes(pixels[i, j].G)[0];
                uncompressedData[k + 2] = BitConverter.GetBytes(pixels[i, j].B)[0];
                k += 3;
            }
        }
        Deflater deflate = new();
        deflate.SetInput(uncompressedData);
        byte[] compressedData = new byte[uncompressedData.Length + 6];
        int bytesAmount = deflate.Deflate(compressedData);
        byte[] idatLength = BitConverter.GetBytes(bytesAmount).Reverse().ToArray();
        byte[] idatChunkName = Encoding.ASCII.GetBytes("IDAT");
        byte[] idatCrc = Crc32.Hash(idatChunkName.Concat(compressedData[0..bytesAmount]).ToArray());
        return idatLength.Concat(idatChunkName).Concat(compressedData[0..bytesAmount]).Concat(idatCrc).ToArray();
    }

    private static byte[] CreateDefaultIENDChunk()
    {
        byte[] iendChunkLength = BitConverter.GetBytes(0);
        byte[] iendChunkName = Encoding.ASCII.GetBytes("IEND");
        byte[] iendCrc = Crc32.Hash(iendChunkName);
        return iendChunkLength.Concat(iendChunkName).Concat(iendCrc).ToArray();
    }
}