using ImageFormatConverter.Tests.AbstractTests;
using Reader.BMP;
using Writer.BMP;

namespace ImageFormatConverter.Tests.BmpTests;

[TestFixture]
public class BmpValidationTests : ImageValidationTests
{
    [SetUp]
    public void Setup()
    {
        SourceDir = TestPicturesDir + "bmp/";
        _imgFileReader = new BmpFileReader();
        _imgWriter = new BmpFileWriter();
    }
    
    
    [TestCaseSource(nameof(_validFiles))]
    public void BMP_ValidateCorrectSample_TrueReturned(string filename)
    {
        ValidateCorrectSample_TrueReturned(filename);
    }
    
    [TestCaseSource(nameof(_invalidFiles))]
    public void BMP_ValidateIncorrectSample_FalseReturned(string filename)
    {
        ValidateIncorrectSample_FalseReturned(filename);
    }
    
    protected static object[]_validFiles = new []{
        new object[] { "correct/correct_sample.bmp" },
        new object[] { "correct/long_header_sample.bmp" },
        new object[] { "correct/red_gradient.bmp" },
        new object[] { "correct/blue_gradient.bmp" },
        new object[] { "correct/red_blue_gradient.bmp" },
        new object[] { "correct/pnh.bmp" }
    };

    protected static object[]_invalidFiles = new[]
    {
        new object[] { "incorrect/empty.bmp" },
        new object[] { "incorrect/ppm.bmp" }
    };
}