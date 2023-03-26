using Reader.PPM;
using Writer.PPM;

namespace ImageFormatConverter.Tests.PpmTests;

[TestFixture]
public class PpmReadAndWriteTests
{
    private PpmFileWriter _ppmWriter;
    private PpmFileReader _ppmReader;
    private Color[,] _image;

    [SetUp]
    public void Setup()
    {
        _ppmWriter = new PpmFileWriter();
        _ppmReader = new PpmFileReader();
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
    public void RewriteCorrectPpm_GotCopy()
    {
        var bytes = _ppmWriter.WriteToFile(_image);
        var picCreated = _ppmReader.ImageToPixels(bytes);

        CollectionAssert.AreEqual(_image, picCreated);
    }
}