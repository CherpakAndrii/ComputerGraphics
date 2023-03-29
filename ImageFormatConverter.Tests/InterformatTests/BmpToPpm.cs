using Reader.BMP;
using Reader.PPM;
using Writer.PPM;

namespace ImageFormatConverter.Tests.InterformatTests;

[TestFixture]
public class BmpToPpm : InterformatConversionTests
{
    [SetUp]
    public void Setup()
    {
        SourceDir = TestPicturesDir + "sources/bmp/correct/";
        newExtension = "ppm";
        oldExtension = "bmp";
        DestinationDir = TestPicturesDir+"interformat/"+oldExtension+"_"+newExtension+'/';
        if (!Directory.Exists(DestinationDir)) Directory.CreateDirectory(DestinationDir);
        _origImgFileReader = new BmpFileReader();
        _convertedImgWriter = new PpmFileWriter();
        _convertedImgReader = new PpmFileReader();
    }

    [TestCaseSource(nameof(_validFiles))]
    public void ConvertBmpToPpm(string filename)
    {
        ConvertPicture(filename);
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