using Reader.PPM;

namespace ImageFormatConverter.Tests.PpmTests;

[TestFixture]
public class PpmReadAndWriteTests
{
    private PpmFileWriter _ppmWriter;
    private PpmFileReader _ppmReader;

    [SetUp]
    public void Setup()
    {
        _ppmWriter = new PpmFileWriter();
        _ppmReader = new PpmFileReader();
    }

    [TestCaseSource(nameof(_correctFiles))]
    public void RewriteCorrectPpm_GotCopy(string origFileName)
    {
        if (_ppmReader.ValidateFileStructure(origFileName))
        {
            var image = _ppmReader.ImageToPixels(origFileName);
            var filename = Regex.Match(origFileName, @".+/([^/]+)\.ppm").Groups[1].Captures[0].Value;
            var newFilepath = "testResPictures/createdPpms/" + filename + ".copy.ppm";
            _ppmWriter.WriteToFile(newFilepath, image);

            if (_ppmReader.ValidateFileStructure(newFilepath))
            {
                var picCreated = _ppmReader.ImageToPixels(newFilepath);
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
    
    private static object[] _correctFiles =
    {
        new object[] { "testResPictures/createdPpms/red_gradient.ppm" }, 
        new object[] { "testResPictures/createdPpms/blue_gradient.ppm" }, 
        new object[] { "testResPictures/createdPpms/red_blue_gradient.ppm" }, 
//        new object[] { "testResPictures/sources/correct_sample.ppm" }
    };
}