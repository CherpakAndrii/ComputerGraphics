using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;

namespace Reader.BMP;

public class BmpFileReader : IImageReader
{
    public Color[,] ImageToPixels(string filename)
    {
        byte[] filedata = File.ReadAllBytes(filename);
        
        int imageWidth = BitConverter.ToInt32(filedata[18..22]);
        int imageHeight = BitConverter.ToInt32(filedata[22..26]);
        int metadataLength = BitConverter.ToInt32(filedata[10..14]);
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
                picture[i, j] = new Color(filedata[pointer + 2], filedata[pointer + 1], filedata[pointer]);
                pointer += 3;
            }

            pointer += numberOfZeroBytes;
        }

        return picture;
    }
    
    public bool ValidateFileStructure(string filename)
    {
        byte[] filedata = File.ReadAllBytes(filename);
        if (filedata.Length < 58 || 
            filedata[0] != 'B' || 
            filedata[1] != 'M' ||
            filedata[26] != 1)
            return false;
        
        
        int filesize = BitConverter.ToInt32(filedata[2..6]);
        int imageWidth = BitConverter.ToInt32(filedata[18..22]);
        int imageHeight = BitConverter.ToInt32(filedata[22..26]);
        int metadataLength = BitConverter.ToInt32(filedata[10..14]);
        byte bps = filedata[28];
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

        return (filesize >= expectedFileSize - numberOfZeroBytes && expectedFileSize <= filedata.Length);
    }

    public string FileExtension => "bmp";
}