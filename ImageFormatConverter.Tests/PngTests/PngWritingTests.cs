using Writer.PNG;

namespace ImageFormatConverter.Tests.PngTests;

public class PngWritingTests : ImageWriting_VisualTests
{
    [SetUp]
    public void Setup()
    {
        Destination = TestPicturesDir + "createdPngs/";
        if (!Directory.Exists(Destination)) Directory.CreateDirectory(Destination);
        DefaultExtension = ".png";
        imgWriter = new PngFileWriter();
    }
}