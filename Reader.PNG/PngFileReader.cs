using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;
using System.IO.Hashing;

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
        if (fileData[0] != 137 &&
            fileData[1] != 'P' &&
            fileData[2] != 'N' &&
            fileData[3] != 'G' &&
            fileData[4] != 13 &&
            fileData[5] != 10 &&
            fileData[6] != 26 &&
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
        if (bitDepth != 1 &&
            bitDepth != 2 &&
            bitDepth != 4 &&
            bitDepth != 8 &&
            bitDepth != 16)
            return false;

        //then 1 byte of color type (possible values: 0, 2, 3, 4, 6)
        byte colorType = fileData[25];
        if (colorType != 0 &&
            colorType != 2 &&
            colorType != 3 &&
            colorType != 4 &&
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
        if (interlaceMethod != 0 &&
            interlaceMethod != 1) return false;

        //it is not supported for now (and maybe for later too)
        if (interlaceMethod == 1) return false;

        //last 4 bytes of every chunkc is crc that indicates data safety

        //checking crc of ihdr file
        if (!CrcCheck(fileData[16..29], fileData[29..32], 4)) return false;

        //checking second chunk.
        int secondChunkLength = BitConverter.ToInt32(fileData.AsSpan()[32..36]);
        if (secondChunkLength < 0) return false;

        //if color type is 3, then next chunk must be PLTE chunk
        //if color type is 2 or 6, it can be PLTE
        //if color type is 0 or 4 PLTE chunk must not appear
        string secondChunkName = BitConverter.ToString(fileData[36..40]);
        if (colorType == 3 && secondChunkName != "PLTE") return false;
        if ((colorType == 0 || colorType == 4) && secondChunkName == "PLTE") return false;

        //The PLTE chunk contains from 1 to 256 palette entries, each a three-byte series of the form
        //The number of entries is determined from the chunk length. A chunk length not divisible by 3 is an error.
        if (secondChunkName == "PLTE" &&
            (secondChunkLength < 3 ||
            secondChunkLength > 768 ||
            secondChunkLength % 3 != 0))
            return false;


        throw new NotImplementedException();
    }

    private bool CrcCheck(byte[] toCrc, byte[] expectedCrc, int length)
    {
        byte[] crc = Crc32.Hash(toCrc);
        if (crc.Length != 4 && crc != expectedCrc) return false;
        return true;
    }
}