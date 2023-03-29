using ImageFormatConverter.Tests.AbstractTests;
using Writer.PNG;

namespace ImageFormatConverter.Tests.PpmTests;

public class PngWritingTests : ImageWriting_VisualTests
{
    [SetUp]
    public void Setup()
    {
        Destination = TestPicturesDir + "createdPpms/"; 
        DefaultExtension = ".ppm";
        imgWriter = new PngFileWriter();
    }
}