using Reader.PNG;
using Writer.PNG;

namespace ImageFormatConverter.Tests.PngTests;

[TestFixture]
public class PngValidationTests
{
    private const string SourceDir = "../../../testPictures/sources/png/";
    private PngFileReader _pngFileReader;
    private PngFileWriter _pngWriter;
    private Color[,] _image;
    
    [SetUp]
    public void Setup()
    {
        _pngFileReader = new PngFileReader();
        _pngWriter = new PngFileWriter();
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
    public void PNG_ValidateGeneratedPicture_TrueReturned()
    {
        var bytes = _pngWriter.WriteToFile(_image);
        var structureValidationResult = _pngFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.True);
    }
    
    [TestCaseSource(nameof(_validPngs))]
    public void PNG_ValidateCorrectSample_TrueReturned(string filename)
    {
        var bytes = File.ReadAllBytes(SourceDir+filename);
        var structureValidationResult = _pngFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.True);
    }
    
    [TestCaseSource(nameof(_invalidPngs))]
    public void PNG_ValidateIncorrectSample_FalseReturned(string filename)
    {
        var bytes = File.ReadAllBytes(SourceDir+filename);
        var structureValidationResult = _pngFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.False);
    }
    
    private static object[] _validPngs =
    {
        /*new object[] { "correct/blue_gradient.ppm" },*/
    };
    
    private static object[] _invalidPngs =
    {
        /*new object[] { "incorrect/incorrect_header_data.ppm" },*/
    };
}