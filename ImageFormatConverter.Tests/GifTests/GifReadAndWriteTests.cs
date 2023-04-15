using Reader.GIF;
using Writer.GIF;

namespace ImageFormatConverter.Tests.GifTests;

[TestFixture]
public class GifReadAndWriteTests : ImageReadingTests
{

    [SetUp]
    public void Setup()
    {
        _imgWriter = new GifFileWriter();
        _imgReader = new GifFileReader();
    }
}