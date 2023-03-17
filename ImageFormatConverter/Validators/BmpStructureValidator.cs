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
            BitConverter.ToInt32(filedata[10..14]) != 54 ||
            filedata[14] != 40 ||
            filedata[26] != 1 ||
            filedata[28] != 24)
                return false;
        
        int filesize = BitConverter.ToInt32(filedata[2..6]);
        int imageWidth = BitConverter.ToInt32(filedata[18..22]);
        int imageHeight = BitConverter.ToInt32(filedata[22..26]);
        
        int rowLength = imageWidth * 3;
        int numberOfZeroBytes = 0;

        while (rowLength%4 != 0)
        {
            rowLength++;
            numberOfZeroBytes++;
        }
        
        int expectedFileSize = 54 + imageHeight * rowLength;

        return (filesize >= expectedFileSize - numberOfZeroBytes && expectedFileSize <= filedata.Length);
    }
}