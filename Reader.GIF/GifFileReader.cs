using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;
using System.Collections;

namespace Reader.GIF;

public class GifFileReader : IImageReader
{
    public string FileExtension => "gif";

    //TEMPORARY HARDCODE
    private static Color[,] result = new Color[0,0];
    public Color[,] ImageToPixels(byte[] fileData)
    {
        return result;
    }

    public bool ValidateFileStructure(byte[] fileData)
    {
        //not yet calculated
        if (fileData.Length <= 15) return false;

        //The first six bytes of a GIF file always contain the same values:
        if (fileData[0] != 'G' ||
            fileData[1] != 'I' ||
            fileData[2] != 'F' ||
            fileData[3] != '8' ||
            !(fileData[4] == '7' || fileData[4] == '9') ||
            fileData[5] != 'a')
            return false;

        int logicalWidth = BitConverter.ToInt32(fileData[6..8].Concat(new byte[2] { 0, 0 }).ToArray());
        int logicalHeight = BitConverter.ToInt32(fileData[8..10].Concat(new byte[2] {0, 0}).ToArray());

        BitArray logicalScreenData = new(new byte[] { fileData[10] });
        //pixel+1 = # bits/pixel in image
        int globalBitPerPixel = IntFromBitArray(logicalScreenData, 0, 3) + 1;
        int globalColorAmount = (int)Math.Pow(2, globalBitPerPixel);
        if (globalColorAmount < 2 || globalColorAmount > 256) return false;
        //sort flag
        if (logicalScreenData[3]) return false;
        //cr+1 = # bits of color resolution
        int cr = IntFromBitArray(logicalScreenData, 4, 7) + 1;
        //bool is there global palette or not
        bool isGlobalPalette = logicalScreenData[7];
        //index in global palette of background color (or default, if there is no global palette)
        int backgroundColorIndex = fileData[11];
        if (isGlobalPalette && backgroundColorIndex >= globalColorAmount) return false;
        //aspect ratio (0 - default 1:1)
        if (fileData[12] != 0) return false;

        //reading global palette
        Color[] globalPalette = new Color[globalColorAmount];
        int cursor = 13;
        if (isGlobalPalette)
        {
            for (int i = 0; i < globalColorAmount; i++)
            {
                globalPalette[i] = new Color(fileData[cursor], fileData[cursor + 1], fileData[cursor + 2]);
                cursor += 3;
                if (fileData.Length <= cursor) return false;
            }
        }

        Color backgroundColor = isGlobalPalette ? globalPalette[backgroundColorIndex] : new(0, 0, 0);

        //skipping all extensions
        while (fileData[cursor] != 44)
        {
            if (fileData[cursor] == 33)
            {
                cursor += 2;
                if (fileData.Length <= cursor) return false;
                do
                {
                    cursor += fileData[cursor] + 1;
                    if (fileData.Length <= cursor) return false;
                } while (fileData[cursor] != 0);
                cursor++;
            }
            else return false;
        }

        if (fileData.Length <= cursor + 10) return false;
        //image description
        int imageLeft = BitConverter.ToInt32(fileData[(cursor + 1)..(cursor + 3)].Concat(new byte[2] {0, 0}).ToArray());
        int imageTop = BitConverter.ToInt32(fileData[(cursor + 3)..(cursor + 5)].Concat(new byte[2] {0, 0}).ToArray());
        int imageWidth = BitConverter.ToInt32(fileData[(cursor + 5)..(cursor + 7)].Concat(new byte[2] {0, 0}).ToArray());
        int imageHeight = BitConverter.ToInt32(fileData[(cursor + 7)..(cursor + 9)].Concat(new byte[2] {0, 0}).ToArray());
        if (imageWidth < 0 || imageHeight < 0) return false;
        BitArray localData = new(new byte[] { fileData[cursor + 9] });
        //pixel+1 = # bits/pixel in image
        int localBitPerPixel = IntFromBitArray(logicalScreenData, 0, 3) + 1;
        int localColorAmount = (int)Math.Pow(2, globalBitPerPixel);
        if (localColorAmount < 2 || localColorAmount > 256) return false;
        //reserved (should be both 0)
        if (localData[3] || localData[4]) return false;
        bool sortFlag = localData[5];
        //Do not support interlacing
        if (localData[6]) return false;
        bool isLocalColorTable = localData[7];
        if (!isLocalColorTable && !isGlobalPalette) return false;

        //local color table
        cursor += 10;
        Color[] localPalette = new Color[localColorAmount];
        if (isLocalColorTable)
        {
            for (int i = 0; i < localColorAmount; i++)
            {
                localPalette[i] = new Color(fileData[cursor], fileData[cursor + 1], fileData[cursor + 2]);
                cursor += 3;
                if (fileData.Length <= cursor) return false;
            }
        }

        //table based image data
        int minLzwCode = fileData[cursor];
        byte[] compressedData = Array.Empty<byte>();
        do
        {
            int compressedDataLength = fileData[cursor + 1];
            compressedData = compressedData.Concat(fileData[(cursor + 2)..(cursor + 3 + compressedDataLength)]).ToArray();
            cursor += 3 + compressedDataLength;
        } while (fileData[cursor + 1] != 0);
        Lzw lzw = new();
        byte[] decompressedData = lzw.Decompress(compressedData);

        //reading decompressed data
        result = new Color[imageHeight, imageWidth];
        int k = 0;
        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                if (isLocalColorTable)
                {
                    if (localColorAmount <= decompressedData[k]) return false;
                    result[i, j] = localPalette[decompressedData[k]];
                }
                else
                {
                    if (globalColorAmount <= decompressedData[k]) return false;
                    result[i, j] = globalPalette[decompressedData[k]];
                }
                k++;
            }
        }
        return true;
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