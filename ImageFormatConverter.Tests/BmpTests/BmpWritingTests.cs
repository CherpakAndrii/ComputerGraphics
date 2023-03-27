using ImageFormatConverter.Tests.AbstractTests;
using Writer.BMP;

namespace ImageFormatConverter.Tests.BmpTests;

public class BmpWritingTests : ImageWriting_VisualTests
{
    [SetUp]
    public void Setup()
    {
        Destination = TestPicturesDir + "createdBmps/";
        DefaultExtension = ".bmp";
        imgWriter = new BmpFileWriter();
    }
}