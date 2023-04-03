using Core.Lights;
using System.Text;
using ImageFormatConverter.Abstractions.Interfaces;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

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
        byte[] ihdrChunkLength = BitConverter.GetBytes(13);
        byte[] ihdrChunkName = Encoding.ASCII.GetBytes("IHDR");
        byte[] imageWidth = BitConverter.GetBytes(pixels.GetLength(1));
        byte[] imageHeight = BitConverter.GetBytes(pixels.GetLength(0));
        byte bitDepth = 8;
        byte colorType = 2;
        byte compressType = 0;
        byte filtrationMethod = 0;
        byte interlaceMethod = 0;
        byte[] ihdrData = imageWidth.Reverse()
                                    .Concat(imageHeight.Reverse())
                                    .Concat(new byte[] { bitDepth, colorType, compressType, filtrationMethod, interlaceMethod })
                                    .ToArray();
        byte[] ihdrCrc = Crc32.Hash(ihdrChunkName.Concat(ihdrData).ToArray());
        return ihdrChunkLength.Reverse().Concat(ihdrChunkName)
                                        .Concat(ihdrData)
                                        .Concat(ihdrCrc.Reverse())
                                        .ToArray();
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
        using MemoryStream outputMemoryStream = new();
        using (DeflaterOutputStream deflateStream = new(outputMemoryStream))
        {
            deflateStream.Write(uncompressedData, 0, uncompressedData.Length);
        }
        byte[] compressedData = outputMemoryStream.ToArray();
        byte[] idatLength = BitConverter.GetBytes(compressedData.Length);
        byte[] idatChunkName = Encoding.ASCII.GetBytes("IDAT");
        byte[] idatCrc = Crc32.Hash(idatChunkName.Concat(compressedData).ToArray());
        return idatLength.Reverse().Concat(idatChunkName)
                                   .Concat(compressedData)
                                   .Concat(idatCrc.Reverse())
                                   .ToArray();
    }

    private static byte[] CreateDefaultIENDChunk()
    {
        byte[] iendChunkLength = BitConverter.GetBytes(0);
        byte[] iendChunkName = Encoding.ASCII.GetBytes("IEND");
        byte[] iendCrc = Crc32.Hash(iendChunkName).ToArray();
        return iendChunkLength.Concat(iendChunkName).Concat(iendCrc.Reverse()).ToArray();
    }
}