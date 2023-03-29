using NUnit.Framework;
using Reader.PPM;
using Writer.PNG;

namespace ImageFormatConverter.Tests.InterformatTests;

[TestFixture]
public class PpmToPng : InterformatConversionTests
{
    [SetUp]
    public void Setup()
    {
        SourceDir = TestPicturesDir + "sources/ppm/correct/";
        newExtension = "png";
        oldExtension = "ppm";
        DestinationDir = TestPicturesDir + "interformat/" + oldExtension + "_" + newExtension + '/';
        if (!Directory.Exists(DestinationDir)) Directory.CreateDirectory(DestinationDir);
        _origImgFileReader = new PpmFileReader();
        _convertedImgWriter = new PngFileWriter();
    }

    [TestCaseSource(nameof(_validFiles))]
    public void ConvertPpmToPng(string filename)
    {
        ConvertPictureWithoutValidation(filename);
    }

    protected static object[] _validFiles = new Object[]
    {
        new object[] { "blue_gradient.ppm" },
        new object[] { "correct_sample_P6.ppm" },
        new object[] { "more_blank_symbols.ppm" },
        new object[] { "one_line.ppm" },
        new object[] { "pretty_sample_P6.ppm" },
        new object[] { "red_blue_gradient.ppm" },
        new object[] { "red_gradient.ppm" },
        new object[] { "test.ppm" }
    };
}