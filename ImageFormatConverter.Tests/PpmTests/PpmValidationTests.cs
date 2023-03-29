using Reader.PPM;
using Writer.PPM;

namespace ImageFormatConverter.Tests.PpmTests;

[TestFixture]
public class PpmValidationTests : ImageValidationTests
{
    [SetUp]
    public void Setup()
    {
        SourceDir = TestPicturesDir + "ppm/";
        _imgFileReader = new PpmFileReader();
        _imgWriter = new PpmFileWriter();
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
    
    private static object[] _validFiles =
    {
        new object[] { "correct/blue_gradient.ppm" },
        new object[] { "correct/correct_sample_P6.ppm" },
        new object[] { "correct/more_blank_symbols.ppm" },
        new object[] { "correct/one_line.ppm" },
        new object[] { "correct/pretty_sample_P6.ppm" },
        new object[] { "correct/red_blue_gradient.ppm" },
        new object[] { "correct/red_gradient.ppm" },
        new object[] { "correct/test.ppm" }
    };
    
    private static object[] _invalidFiles =
    {
        new object[] { "incorrect/incorrect_header_data.ppm" },
        new object[] { "incorrect/incorrect_header_structure.ppm" },
        new object[] { "incorrect/incorrect_symbol_in_pixels.ppm" },
        new object[] { "incorrect/too_big_pixel_data.ppm" },
        new object[] { "incorrect/too_long.ppm" },
        new object[] { "incorrect/too_short.ppm" },
    };
}