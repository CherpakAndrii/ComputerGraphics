using Reader.BMP;
using Writer.PNG;

namespace ImageFormatConverter.Tests.InterformatTests;

[TestFixture]
public class BmpToPng : InterformatConversionTests
{
    [SetUp]
    public void Setup()
    {
        SourceDir = TestPicturesDir + "sources/bmp/correct/";
        newExtension = "png";
        oldExtension = "bmp";
        DestinationDir = TestPicturesDir+"interformat/"+oldExtension+"_"+newExtension+'/';
        if (!Directory.Exists(DestinationDir)) Directory.CreateDirectory(DestinationDir);
        _origImgFileReader = new BmpFileReader();
        _convertedImgWriter = new PngFileWriter();
    }

    [TestCaseSource(nameof(_validFiles))]
    public void ConvertBmpToPng(string filename)
    {
        ConvertPictureWithoutValidation(filename);
    }
    
    protected static object[] _validFiles = new Object[]{
        new object[] { "correct_sample.bmp" },
        new object[] { "long_header_sample.bmp" },
        new object[] { "red_gradient.bmp" },
        new object[] { "blue_gradient.bmp" },
        new object[] { "red_blue_gradient.bmp" },
        new object[] { "pnh.bmp" }
    };
}