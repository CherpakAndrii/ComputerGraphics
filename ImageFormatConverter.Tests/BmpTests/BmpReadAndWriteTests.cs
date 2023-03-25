namespace ImageFormatConverter.Tests.BmpTests;

[TestFixture]
public class BmpReadAndWriteTests
{
    private BmpFileWriter _bmpWriter;
    private BmpFileReader _bmpReader;

    [SetUp]
    public void Setup()
    {
        _bmpWriter = new BmpFileWriter();
        _bmpReader = new BmpFileReader();
    }

    [TestCaseSource(nameof(correctFiles))]
    public void RewriteCorrectBmp_GotCopy(string origFileName)
    {
        if (_bmpReader.ValidateFileStructure(origFileName))
        {
            var image = _bmpReader.ImageToPixels(origFileName);
            var filename = Regex.Match(origFileName, @".+/([^/]+)\.bmp$").Groups[1].Captures[0].Value;
            var newFilepath = "testResPictures/createdBmps/" + filename + ".copy.bmp";
            _bmpWriter.WriteToFile(newFilepath, image);

            if (_bmpReader.ValidateFileStructure(newFilepath))
            {
                var picCreated = _bmpReader.ImageToPixels(newFilepath);
                CollectionAssert.AreEqual(image, picCreated);
            }
            else
            {
                Assert.Fail("invalid created file structure");
            }
        }
        else
        {
            Assert.Fail();
        }
    }
    
    private static object[] correctFiles =
    {
        new object[] { "testResPictures/sources/correct_sample.bmp" },
        new object[] { "testResPictures/sources/incorrect_sample.bmp" },
        new object[] { "testResPictures/createdBmps/red_gradient.bmp" },
        new object[] { "testResPictures/createdBmps/blue_gradient.bmp" },
        new object[] { "testResPictures/createdBmps/red_blue_gradient.bmp" }
    };
}