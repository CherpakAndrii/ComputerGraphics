using Core.Lights;
using ImageFormatConverter.Interfaces;

namespace ImageFormatConverter.Readers;

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
}