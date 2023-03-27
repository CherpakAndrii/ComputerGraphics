using ImageFormatConverter.Tests.AbstractTests;
using ImageFormatConverter.Tests.BmpTests;
using Writer.PPM;

namespace ImageFormatConverter.Tests.PpmTests;

public class PpmWritingTests : ImageWriting_VisualTests
{
    [SetUp]
    public void Setup()
    {
        Destination = TestPicturesDir + "createdPpms/"; 
        DefaultExtension = ".ppm";
        imgWriter = new PpmFileWriter();
    }
}