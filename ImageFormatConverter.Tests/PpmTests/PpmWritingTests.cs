using Writer.PPM;

namespace ImageFormatConverter.Tests.PpmTests;

public class PpmWritingTests : ImageWriting_VisualTests
{
    [SetUp]
    public void Setup()
    {
        Destination = TestPicturesDir + "createdPpms/"; 
        if (!Directory.Exists(Destination)) Directory.CreateDirectory(Destination);
        DefaultExtension = ".ppm";
        imgWriter = new PpmFileWriter();
    }
}