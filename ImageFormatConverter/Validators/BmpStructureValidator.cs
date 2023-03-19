using ImageFormatConverter.Interfaces;

namespace ImageFormatConverter.Validators;

public class BmpStructureValidator : IImageFileStructureValidator
{
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
}