using Core.Lights;
using System.Collections;
using ImageFormatConverter.Common;
using ImageFormatConverter.Abstractions.Interfaces;

namespace Writer.GIF;

public class GifFileWriter : IImageWriter
{
    public string FileExtension => "gif";

    public byte[] WriteToFile(Color[,] pixels)
    {
        GifPalette palette = GifPaletteSelector.GetPalette(pixels);
        byte[] gifSignature = "GIF89a"u8.ToArray();
        byte[] logicalScreen = CreateLogicalScreenDescriptor(pixels, palette);
        byte[] imageDescriptor = CreateImageDescriptor(pixels, palette);
        byte[] localColorTable = CreateLocalColorTable(palette);
        byte[] dataSubBlocks = CreateDataSubBlocks(pixels, palette);
        return gifSignature.Concat(logicalScreen)
                           .Concat(imageDescriptor)
                           .Concat(localColorTable)
                           .Concat(dataSubBlocks)
                           .Concat(new byte[] { 59 })
                           .ToArray();
    }

    private static byte[] CreateLogicalScreenDescriptor(Color[,] pixels, GifPalette palette)
    {
        byte[] logicalWidth = BitConverter.GetBytes(pixels.GetLength(1))[0..2];
        byte[] logicalHeight = BitConverter.GetBytes(pixels.GetLength(0))[0..2];
        int globalPalettePower = (int)Math.Ceiling(Math.Log2(palette.BaseColors.Length));
        BitArray globalBitPerPixel = new(new int[] { globalPalettePower - 1 });
        BitArray colorResolution = new(new int[] { 7 });
        byte packedFields = BitConverter.GetBytes(Helper.IntFromBitArray(globalBitPerPixel, 0, 3) + Helper.IntFromBitArray(colorResolution, 4, 7))[0];
        byte[] lastBytes = new byte[]{ packedFields, 0, 0 };
        return logicalHeight.Reverse().Concat(logicalWidth.Reverse())
                                      .Concat(lastBytes).ToArray();
    }

    private static byte[] CreateImageDescriptor(Color[,] pixels, GifPalette palette)
    {
        byte[] descriptorSeparator = { 44 };
        byte[] imageLeft = BitConverter.GetBytes(0)[0..2];
        byte[] imageTop = BitConverter.GetBytes(0)[0..2];
        byte[] imageWidth = BitConverter.GetBytes(pixels.GetLength(1))[0..2];
        byte[] imageHeight = BitConverter.GetBytes(pixels.GetLength(0))[0..2];
        int locallPalettePower = (int)Math.Ceiling(Math.Log2(palette.BaseColors.Length));
        BitArray localBitPerPixel = new(new int[] { locallPalettePower - 1 });
        byte[] packedFields = { BitConverter.GetBytes(Helper.IntFromBitArray(localBitPerPixel, 0, 3) + 128)[0] };
        return descriptorSeparator.Concat(imageLeft.Reverse())
                                  .Concat(imageTop.Reverse())
                                  .Concat(imageWidth.Reverse())
                                  .Concat(imageHeight.Reverse())
                                  .Concat(packedFields).ToArray();
    }

    private static byte[] CreateLocalColorTable(GifPalette palette)
    {
        int ColorNumbers = (int)Math.Pow(2, Math.Ceiling(Math.Log2(palette.BaseColors.Length)));
        byte[] localColorTable = new byte[ColorNumbers * 3];
        int j = 0;
        for (int i = 0; i < palette.BaseColors.Length; i++)
        {
            localColorTable[j] = BitConverter.GetBytes(palette.BaseColors[i].R)[0];
            localColorTable[j + 1] = BitConverter.GetBytes(palette.BaseColors[i].G)[0];
            localColorTable[j + 2] = BitConverter.GetBytes(palette.BaseColors[i].B)[0];
            j += 3;
        }
        for (int i = palette.BaseColors.Length; i < ColorNumbers * 3; i++)
        {
            localColorTable[i] = 0;
        }
        return localColorTable;
    }

    private static byte[] CreateDataSubBlocks(Color[,] pixels, GifPalette palette)
    {
        byte[] result = { BitConverter.GetBytes(Math.Ceiling(Math.Log2(palette.BaseColors.Length)))[0] };
        byte[] compressedData = LzwCompress(pixels, palette);
        int counter = 0;
        while (compressedData.Length / 256 > counter)
        {
            result = result.Concat(new byte[] { 255 })
                           .Concat(compressedData[(256 * counter)..(256 * (counter + 1))])
                           .ToArray();
            counter++;
        }
        int rest = compressedData.Length % 256;
        return result.Concat(new byte[] { BitConverter.GetBytes(rest)[0] })
                     .Concat(compressedData[(256 * counter)..compressedData.Length])
                     .Concat(new byte[] { BitConverter.GetBytes(0)[0] }).ToArray();
    }

    private static byte[] LzwCompress(Color[,] pixels, GifPalette palette)
    {
        var lzw = new Lzw();
        var paletteIndexes = palette.GetColorIndexes(pixels);

        List<byte> decompressedData = new();

        for (int i = 0; i < paletteIndexes.GetLength(0); i++)
        {
            for (int j = 0; j < paletteIndexes.GetLength(1); j++)
            {
                decompressedData.Add(paletteIndexes[i, j]);
            }
        }

        lzw.Сompress(decompressedData, palette.BaseColors.Length);
        
        return Array.Empty<byte>();
    }
}
