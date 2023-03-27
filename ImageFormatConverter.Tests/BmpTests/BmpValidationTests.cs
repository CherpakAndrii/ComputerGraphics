using Reader.BMP;
using Writer.BMP;

namespace ImageFormatConverter.Tests.BmpTests;

[TestFixture]
public class BmpValidationTests
{
    private const string SourceDir = "../../../testPictures/sources/bmp/";
    private BmpFileReader _bmpFileReader;
    private BmpFileWriter _bmpWriter;
    private Color[,] _image;
    
    [SetUp]
    public void Setup()
    {
        _bmpFileReader = new BmpFileReader();
        _bmpWriter = new BmpFileWriter();
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
    public void BMP_ValidateGeneratedPicture_TrueReturned()
    {
        var bytes = _bmpWriter.WriteToFile(_image);
        var structureValidationResult = _bmpFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.True);
    }
    
    [TestCaseSource(nameof(_validBmps))]
    public void BMP_ValidateCorrectSample_TrueReturned(string filename)
    {
        var bytes = File.ReadAllBytes(SourceDir+filename);
        var structureValidationResult = _bmpFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.True);
    }
    
    [TestCaseSource(nameof(_invalidBmps))]
    public void BMP_ValidateIncorrectSample_FalseReturned(string filename)
    {
        var bytes = File.ReadAllBytes(SourceDir+filename);
        var structureValidationResult = _bmpFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.False);
    }
    
    private static object[] _validBmps =
    {
        new object[] { "correct/correct_sample.bmp" },
        new object[] { "correct/long_header_sample.bmp" },
        new object[] { "correct/red_gradient.bmp" },
        new object[] { "correct/blue_gradient.bmp" },
        new object[] { "correct/red_blue_gradient.bmp" },
        new object[] { "correct/pnh.bmp" }
    };
    
    private static object[] _invalidBmps =
    {
        new object[] { "incorrect/empty.bmp" },
        new object[] { "incorrect/ppm.bmp" }
    };
}