using Writer.BMP;

namespace ImageFormatConverter.Tests.BmpTests;

public class BmpWritingTests : ImageWriting_VisualTests
{
    [SetUp]
    public void Setup()
    {
        Destination = TestPicturesDir + "createdBmps/";
        if (!Directory.Exists(Destination)) Directory.CreateDirectory(Destination);
        DefaultExtension = ".bmp";
        imgWriter = new BmpFileWriter();
    }
}