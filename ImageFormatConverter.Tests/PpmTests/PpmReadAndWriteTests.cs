using Reader.PPM;
using Writer.PPM;

namespace ImageFormatConverter.Tests.PpmTests;

[TestFixture]
public class PpmReadAndWriteTests : ImageReadingTests
{

    [SetUp]
    public void Setup()
    {
        _imgWriter = new PpmFileWriter();
        _imgReader = new PpmFileReader();
    }
}