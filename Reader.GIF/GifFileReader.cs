using Core.Lights;
using System.Collections;
using ImageFormatConverter.Common;
using ImageFormatConverter.Abstractions.Interfaces;

namespace Reader.GIF;

public class GifFileReader : IImageReader
{
    public string FileExtension => "gif";

    public Color[,] ImageToPixels(byte[] fileData)
    {
        ParseLogicalScreenPacked(fileData[10], out int globalBitPerPixel, out _, out bool isGlobalPalette);
        int globalColorAmount = (int)Math.Pow(2, globalBitPerPixel);
        int backgroundColorIndex = fileData[11];
        int cursor = 13;
        Color[] palette = isGlobalPalette ? ParseColorTable(fileData, globalColorAmount, ref cursor) : Array.Empty<Color>();
        Color backgroundColor = isGlobalPalette ? palette[backgroundColorIndex] : new(0, 0, 0);
        SkipExtensions(fileData, ref cursor);
        ParseImageDescription(fileData, ref cursor, out _, out _, out int width, out int height);
        ParseLocalLogicalScreenPacked(fileData[cursor + 9], out int localBitPerpixel, out _, out _, out bool islocalPalette);
        int localColorAmount = (int)Math.Pow(2, localBitPerpixel + 1);
        cursor += 10;
        if (islocalPalette)
            palette = ParseColorTable(fileData, localColorAmount, ref cursor);
        int minLzwCode = fileData[cursor++];
        byte[] compressedData = GetCompressedData(fileData, ref cursor);
        return GetPixels(palette, compressedData, minLzwCode, height, width, islocalPalette ? localColorAmount : globalColorAmount);
    }
    
    public byte[] CompressedData { get; set; }
    
    public int LzwCode { get; set; }

    public bool ValidateFileStructure(byte[] fileData)
    {
        if (fileData.Length <= 15) return false;
        if (fileData[0] != 'G' ||
            fileData[1] != 'I' ||
            fileData[2] != 'F' ||
            fileData[3] != '8' ||
            !(fileData[4] == '7' || fileData[4] == '9') ||
            fileData[5] != 'a')
            return false;
        if (!ValidateLogicalScreen(fileData, out int globalColorAmount, out bool isGlobalPalette)) 
            return false;
        int cursor = isGlobalPalette? 13 + globalColorAmount * 3 : 13;
        if (fileData.Length <= cursor) 
            return false;
        if (!SkipExtensions(fileData, ref cursor)) 
            return false;
        if (fileData.Length <= cursor + 10) 
            return false;
        ParseImageDescription(fileData, ref cursor, out _, out _, out int width, out int height);
        if (width < 0 || height < 0) 
            return false;
        if(!ValidateLocalLogicalScreen(fileData, ref cursor, out bool isLocalPalette))
            return false;
        if (!isLocalPalette && !isGlobalPalette) 
            return false;
        return true;
    }

    private static bool ValidateLogicalScreen(byte[] fileData, out int colorAmount, out bool isGlobalPalette)
    {
        ParseLogicalScreenPacked(fileData[10], out int pixel, out bool sort, out isGlobalPalette);
        colorAmount = (int)Math.Pow(2, pixel);
        return !((isGlobalPalette && fileData[11] >= colorAmount)
                || pixel is < 0 or > 7
                || fileData[12] != 0
                || sort);
    }

    private static void ParseLogicalScreenPacked(byte data, out int pixel, out bool sort, out bool globalPalette)
    {
        BitArray logicalScreenData = new(new byte[] { data });
        pixel = Helper.IntFromBitArray(logicalScreenData, 0, 3) + 1;
        sort = logicalScreenData[3];
        globalPalette = logicalScreenData[7];
    }

    private static bool ValidateLocalLogicalScreen(byte[] fileData, ref int cursor, out bool islocalPalette)
    {
        ParseLocalLogicalScreenPacked(fileData[cursor + 9], out int pixel, out bool sort, out bool interlancing, out islocalPalette);
        return !(pixel is < 0 or > 7
                || fileData[12] != 0
                || interlancing
                || sort);
    }

    private static void ParseLocalLogicalScreenPacked(byte data, out int pixel, out bool sort, out bool interlancing, out bool isLocalPalette)
    {
        BitArray logicalScreenData = new(new byte[] { data });
        pixel = Helper.IntFromBitArray(logicalScreenData, 0, 3) + 1;
        sort = logicalScreenData[5];
        interlancing = logicalScreenData[6];
        isLocalPalette = logicalScreenData[7];
    }

    private static bool SkipExtensions(byte[] fileData, ref int cursor)
    {
        while (fileData[cursor] != 44)
        {
            if (fileData[cursor] == 33)
            {
                cursor += 2;
                if (fileData.Length <= cursor) 
                    return false;
                do
                {
                    cursor += fileData[cursor] + 1;
                    if (fileData.Length <= cursor) 
                        return false;
                } while (fileData[cursor] != 0);
                cursor++;
            }
            else return false;
        }
        return true;
    }

    private static Color[] ParseColorTable(byte[] fileData, int amount, ref int cursor)
    {
        Color[] globalPalette = new Color[amount];
        for (int i = 0; i < amount; i++)
        {
            globalPalette[i] = new Color(fileData[cursor], fileData[cursor + 1], fileData[cursor + 2]);
            cursor += 3;
        }
        return globalPalette;
    }

    private static void ParseImageDescription(byte[] fileData, ref int cursor, out int left, out int top, out int width, out int height)
    {
        left = BitConverter.ToUInt16(fileData.AsSpan()[(cursor + 1)..(cursor + 3)]);
        top = BitConverter.ToUInt16(fileData.AsSpan()[(cursor + 3)..(cursor + 5)]);
        width = BitConverter.ToUInt16(fileData.AsSpan()[(cursor + 5)..(cursor + 7)]);
        height = BitConverter.ToUInt16(fileData.AsSpan()[(cursor + 7)..(cursor + 9)]);
    }

    private static byte[] GetCompressedData(byte[] fileData, ref int cursor) 
    {
        byte[] compressedData = Array.Empty<byte>();
        do
        {
            int compressedDataLength = fileData[cursor];
            compressedData = compressedData.Concat(fileData[(cursor + 1)..(cursor + 1 + compressedDataLength)]).ToArray();
            cursor += 1 + compressedDataLength;
        } while (fileData[cursor] != 0);
        return compressedData;
    }

    private Color[,] GetPixels(Color[] palette, byte[] compressedData, int minLzwCode, int height, int width, int colorAmount)
    {
        Lzw lzw = new();

        this.CompressedData = compressedData;
        this.LzwCode = minLzwCode;
        
        byte[] decompressedData = lzw.Decompress(compressedData, minLzwCode);
        Color[,] result = new Color[height, width];
        int k = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (decompressedData[k] > colorAmount)
                    result[i, j] = palette[colorAmount - 1];
                else result[i, j] = palette[decompressedData[k]];
                k++;
            }
        }
        return result;
    }
}