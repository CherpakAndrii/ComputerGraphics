using Reader.GIF;
using Writer.GIF;

namespace ImageFormatConverter.Tests.GifTests;

[TestFixture]
public class GifValidationTests : ImageValidationTests
{
    [SetUp]
    public void Setup()
    {
        SourceDir = TestPicturesDir + "gif/";
        _imgFileReader = new GifFileReader();
        _imgWriter = new GifFileWriter();
    }
    
    
    [TestCaseSource(nameof(_validFiles))]
    public void GIF_ValidateCorrectSample_TrueReturned(string filename)
    {
        ValidateCorrectSample_TrueReturned(filename);
    }
    
    [TestCaseSource(nameof(_invalidFiles))]
    public void GIF_ValidateIncorrectSample_FalseReturned(string filename)
    {
        ValidateIncorrectSample_FalseReturned(filename);
    }
    
    protected static object[]_validFiles = new object[]{
        //new object[] { "correct/correct_sample.gif" }
    };

    protected static object[]_invalidFiles = new object[]
    {
        //new object[] { "incorrect/empty.gif" }
    };
}