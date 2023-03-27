using Reader.PPM;
using Writer.PPM;

namespace ImageFormatConverter.Tests.PpmTests;

[TestFixture]
public class PpmValidationTests
{
    private const string SourceDir = "../../../testPictures/sources/ppm/";
    private PpmFileReader _ppmFileReader;
    private PpmFileWriter _ppmWriter;
    private Color[,] _image;
    
    [SetUp]
    public void Setup()
    {
        _ppmFileReader = new PpmFileReader();
        _ppmWriter = new PpmFileWriter();
        _image = new Color[512, 512];
        for (int i = 0; i < 512; i++)
        {
            for (int j = 0; j < 512; j++)
            {
                _image[i, j] = new Color((byte)(j / 2), 0, 0);
            }
        }
    }

    [Test]
    public void PPM_ValidateGeneratedPicture_TrueReturned()
    {
        var bytes = _ppmWriter.WriteToFile(_image);
        var structureValidationResult = _ppmFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.True);
    }
    
    [TestCaseSource(nameof(_validPpms))]
    public void PPM_ValidateCorrectSample_TrueReturned(string filename)
    {
        var bytes = File.ReadAllBytes(SourceDir+filename);
        var structureValidationResult = _ppmFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.True);
    }
    
    [TestCaseSource(nameof(_invalidPpms))]
    public void PPM_ValidateIncorrectSample_FalseReturned(string filename)
    {
        var bytes = File.ReadAllBytes(SourceDir+filename);
        var structureValidationResult = _ppmFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.False);
    }
    
    private static object[] _validPpms =
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
    
    private static object[] _invalidPpms =
    {
        new object[] { "incorrect/incorrect_header_data.ppm" },
        new object[] { "incorrect/incorrect_header_structure.ppm" },
        new object[] { "incorrect/incorrect_symbol_in_pixels.ppm" },
        new object[] { "incorrect/too_big_pixel_data.ppm" },
        new object[] { "incorrect/too_long.ppm" },
        new object[] { "incorrect/too_short.ppm" },
    };
}