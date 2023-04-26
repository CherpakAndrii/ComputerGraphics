using Writer.BMP;

namespace ImageFormatConverter.Tests.GifTests;

public class GifWritingTests : ImageWriting_VisualTests
{
    [SetUp]
    public void Setup()
    {
        Destination = TestPicturesDir + "createdGifs/";
        if (!Directory.Exists(Destination)) Directory.CreateDirectory(Destination);
        DefaultExtension = ".gif";
        imgWriter = new BmpFileWriter();
    }
}