using Reader.BMP;
using Writer.BMP;

namespace ImageFormatConverter.Tests.BmpTests;

[TestFixture]
public class BmpReadAndWriteTests
{
    private BmpFileWriter _bmpWriter;
    private BmpFileReader _bmpReader;
    private Color[,] _image;

    [SetUp]
    public void Setup()
    {
        _bmpWriter = new BmpFileWriter();
        _bmpReader = new BmpFileReader();
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
    public void RewriteCorrectBmp_GotCopy()
    {
        var bytes = _bmpWriter.WriteToFile(_image);
        var picCreated = _bmpReader.ImageToPixels(bytes);

        CollectionAssert.AreEqual(_image, picCreated);
    }
}