using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;

namespace Reader.BMP;

public class BmpFileReader : IImageReader
{
    public Color[,] ImageToPixels(byte[] fileData)
    {
        int imageWidth = BitConverter.ToInt32(fileData.AsSpan()[18..22]);
        int imageHeight = BitConverter.ToInt32(fileData.AsSpan()[22..26]);
        int metadataLength = BitConverter.ToInt32(fileData.AsSpan()[10..14]);
        int rowLength = imageWidth * 3;
        int numberOfZeroBytes = 0;

        while (rowLength%4 != 0)
        {
            rowLength++;
            numberOfZeroBytes++;
        }

        Color[,] picture = new Color[imageHeight, imageWidth];
        int pointer = metadataLength;

        for (int i = imageHeight-1; i >= 0; i--)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                picture[i, j] = new Color(fileData[pointer + 2], fileData[pointer + 1], fileData[pointer]);
                pointer += 3;
            }

            pointer += numberOfZeroBytes;
        }

        return picture;
    }
    
    public bool ValidateFileStructure(byte[] fileData)
    {
        if (fileData.Length < 58 || 
            fileData[0] != 'B' || 
            fileData[1] != 'M' ||
            fileData[26] != 1)
            return false;
        
        
        int filesize = BitConverter.ToInt32(fileData.AsSpan()[2..6]);
        int imageWidth = BitConverter.ToInt32(fileData.AsSpan()[18..22]);
        int imageHeight = BitConverter.ToInt32(fileData.AsSpan()[22..26]);
        int metadataLength = BitConverter.ToInt32(fileData.AsSpan()[10..14]);
        byte bps = fileData[28];
        if (bps != 24 && bps != 32) return false;

        bps = (byte)(bps >> 3);

        int rowLength = imageWidth * bps;
        int numberOfZeroBytes = 0;

        while (rowLength%4 != 0)
        {
            rowLength++;
            numberOfZeroBytes++;
        }
        
        int expectedFileSize = metadataLength + imageHeight * rowLength;

        return (filesize >= expectedFileSize - numberOfZeroBytes && expectedFileSize <= fileData.Length);
    }

    public string FileExtension => "bmp";
}