using ImageFormatConverter.Tests.AbstractTests;
using Reader.PNG;
using Writer.PNG;

namespace ImageFormatConverter.Tests.PpmTests;

[TestFixture]
public class PngReadAndWriteTests : ImageReadingTests
{

    [SetUp]
    public void Setup()
    {
        _imgWriter = new PngFileWriter();
        _imgReader = new PngFileReader();
    }
}