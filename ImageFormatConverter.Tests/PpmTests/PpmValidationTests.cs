using Reader.PPM;
using Writer.PPM;

namespace ImageFormatConverter.Tests.PpmTests;

[TestFixture]
public class PpmValidationTests
{
    private PpmFileReader _ppmFileReader;
    private PpmFileWriter _ppmFileWriter;
    private Color[,] _image;
    
    [SetUp]
    public void Setup()
    {
        _ppmFileReader = new PpmFileReader();
        _ppmFileWriter = new PpmFileWriter();
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
    public void PPM_ValidateCorrectSample_TrueReturned()
    {
        var bytes = _ppmFileWriter.WriteToFile(_image);
        var structureValidationResult = _ppmFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.True);
    }
}