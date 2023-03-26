using Core.Lights;
using System.IO.Hashing;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ImageFormatConverter.Abstractions.Interfaces;

namespace Reader.PNG;

public class PngFileReader : IImageReader
{
    public string FileExtension => "png";

    public Color[,] ImageToPixels(byte[] fileData)
    {

        throw new NotImplementedException();
    }

    public bool ValidateFileStructure(byte[] fileData)
    {
        if (fileData.Length < 50) return false;

        //The first eight bytes of a PNG file always contain the same values:
        if (fileData[0] != 137 ||
            fileData[1] != 'P' ||
            fileData[2] != 'N' ||
            fileData[3] != 'G' ||
            fileData[4] != 13 ||
            fileData[5] != 10 ||
            fileData[6] != 26 ||
            fileData[7] != 10)
            return false;

        //then we always have IHDR chunk

        //first 4 bytes is length of chunks data (for IHDR it shoud be always 13)
        int ihdrLength = BitConverter.ToInt32(fileData.AsSpan()[8..12]);
        if (ihdrLength != 13) return false;

        //following 4 bytes is chunk name. fisrt chunk always should be IHDR
        string ihdrName = BitConverter.ToString(fileData[12..16]);
        if (ihdrName != "IHDR") return false;

        //then start IHDR data section

        //first 8 bytes contaings image width and height
        int imageWidth = BitConverter.ToInt32(fileData.AsSpan()[16..20]);
        if (imageWidth < 0) return false;
        int imageHeight = BitConverter.ToInt32(fileData.AsSpan()[20..24]);
        if (imageHeight < 0) return false;

        //then 1 byte of bit depth (possible values: 1, 2, 4, 8 and 16)
        byte bitDepth = fileData[24];
        if (bitDepth != 1 ||
            bitDepth != 2 ||
            bitDepth != 4 ||
            bitDepth != 8 ||
            bitDepth != 16)
            return false;

        //then 1 byte of color type (possible values: 0, 2, 3, 4, 6)
        byte colorType = fileData[25];
        if (colorType != 0 ||
            colorType != 2 ||
            colorType != 3 ||
            colorType != 4 ||
            colorType != 6)
            return false;

        //not allowed combination of colour type and a bit depth
        if ((colorType == 3 && bitDepth == 16) ||
            ((bitDepth == 1 || bitDepth == 2 || bitDepth == 4) &&
            (colorType == 2 || colorType == 4 || colorType == 6)))
            return false;
        
        //then 1 byte of compression type (for now there is only 1 possible compression: DEFLATE (0))
        byte compressType = fileData[26];
        if (compressType != 0) return false;

        //then 1 byte of filtration method (for now there is only 1 possible filtration: (0))
        byte filtrationMethod = fileData[27];
        if (filtrationMethod != 0) return false;

        //then 1 byte of interlace method (for now there are only 2 possible methods: no interlace (0) and Adam7 (1))
        byte interlaceMethod = fileData[28];
        if (interlaceMethod != 0 ||
            interlaceMethod != 1) return false;

        //last 4 bytes of every chunkc is crc that indicates data safety

        //checking crc of ihdr file
        byte[] crc = Crc32.Hash(fileData[16..29]);
        if (crc.Length < 4 && crc[0..4] != fileData[29..32]) return false;

        //int secondChunkLength = BitConverter.ToInt32(fileData.AsSpan()[]);

        throw new NotImplementedException();
    }
}