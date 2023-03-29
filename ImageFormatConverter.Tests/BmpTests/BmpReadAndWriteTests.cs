using ImageFormatConverter.Tests.AbstractTests;
using Reader.BMP;
using Writer.BMP;

namespace ImageFormatConverter.Tests.BmpTests;

[TestFixture]
public class BmpReadAndWriteTests : ImageReadingTests
{

    [SetUp]
    public void Setup()
    {
        _imgWriter = new BmpFileWriter();
        _imgReader = new BmpFileReader();
    }
}