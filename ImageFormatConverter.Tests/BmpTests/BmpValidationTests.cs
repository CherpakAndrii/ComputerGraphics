using Reader.BMP;
using Writer.BMP;

namespace ImageFormatConverter.Tests.BmpTests;

[TestFixture]
public class BmpValidationTests
{
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
    public void BMP_ValidateCorrectSample_TrueReturned()
    {
        var bytes = _bmpWriter.WriteToFile(_image);
        var structureValidationResult = _bmpFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.True);
    }
}