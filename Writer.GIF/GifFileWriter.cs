using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;
using System.Collections;

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
        byte[] localColorTable = CreateLocalColorTable(pixels, palette);
        throw new NotImplementedException();
    }

    private byte[] CreateLogicalScreenDescriptor(Color[,] pixels, GifPalette palette)
    {
        byte[] logicalWidth = BitConverter.GetBytes(pixels.GetLength(1))[0..2];
        byte[] logicalHeight = BitConverter.GetBytes(pixels.GetLength(0))[0..2];
        int globalPalettePower = (int)Math.Ceiling(Math.Log2(palette.BaseColors.Length));
        BitArray globalBitPerPixel = new(new int[] { globalPalettePower - 1 });
        BitArray colorResolution = new(new int[] { 7 });
        byte packedFields = BitConverter.GetBytes(IntFromBitArray(globalBitPerPixel, 0, 3) + IntFromBitArray(colorResolution, 4, 7))[0];
        byte[] lastBytes = new byte[]{ packedFields, 0, 0 };
        return logicalHeight.Reverse().Concat(logicalWidth.Reverse())
                                      .Concat(lastBytes).ToArray();
    }

    private byte[] CreateImageDescriptor(Color[,] pixels, GifPalette palette)
    {
        byte[] descriptorSeparator = { 44 };
        byte[] imageLeft = BitConverter.GetBytes(0)[0..2];
        byte[] imageTop = BitConverter.GetBytes(0)[0..2];
        byte[] imageWidth = BitConverter.GetBytes(pixels.GetLength(1))[0..2];
        byte[] imageHeight = BitConverter.GetBytes(pixels.GetLength(0))[0..2];
        int locallPalettePower = (int)Math.Ceiling(Math.Log2(palette.BaseColors.Length));
        BitArray localBitPerPixel = new(new int[] { locallPalettePower - 1 });
        byte[] packedFields = { BitConverter.GetBytes(IntFromBitArray(localBitPerPixel, 0, 3) + 128)[0] };
        return descriptorSeparator.Concat(imageLeft.Reverse())
                                  .Concat(imageTop.Reverse())
                                  .Concat(imageWidth.Reverse())
                                  .Concat(imageHeight.Reverse())
                                  .Concat(packedFields).ToArray();
    }

    private byte[] CreateLocalColorTable(Color[,] pixels, GifPalette palette )
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

    private static int IntFromBitArray(BitArray bitArray, int from, int to)
    {
        int value = 0;
        if (from >= 0 && to - from <= 32 && bitArray.Count >= to)
        {
            for (int i = from; i < to; i++)
            {
                if (bitArray[i])
                    value += Convert.ToInt16(Math.Pow(2, i - from));
            }
        }
        return value;
    }
}
