using Reader.BMP;
using Reader.PPM;
using Writer.BMP;

namespace ImageFormatConverter.Tests.InterformatTests;

[TestFixture]
public class PpmToBmpTests
{
    private BmpFileWriter _bmpWriter;
    private PpmFileReader _ppmReader;
    private BmpFileReader _bmpReader;

    [SetUp]
    public void Setup()
    {
        _bmpWriter = new BmpFileWriter();
        _ppmReader = new PpmFileReader();
        _bmpReader = new BmpFileReader();
    }
    
    [TestCaseSource(nameof(_validPpms))]
    public void PpmToBmp_SamePictureGot(string inputPpmFile)
    {
        if (_ppmReader.ValidateFileStructure(inputPpmFile))
        {
            var picOrig = _ppmReader.ImageToPixels(inputPpmFile);
            var filename = Regex.Match(inputPpmFile, @".+/([^/]+)\.ppm$").Groups[1].Captures[0].Value;
            var newFilepath = "testResPictures/interformat/" + filename + ".bmp";
            _bmpWriter.WriteToFile(newFilepath, picOrig);
            
            if (_bmpReader.ValidateFileStructure(newFilepath))
            {
                var picCreated = _bmpReader.ImageToPixels(newFilepath);
                CollectionAssert.AreEqual(picOrig, picCreated);
            }
            else Assert.Fail("invalid created file structure");
        }
        else Assert.Fail("invalid source file structure");
    }


    private static object[] _validPpms =
    {
        new object[] { "testResPictures/createdPpms/red_gradient.ppm" }, 
        new object[] { "testResPictures/createdPpms/blue_gradient.ppm" }, 
        new object[] { "testResPictures/createdPpms/red_blue_gradient.ppm" },
        new object[] { "testResPictures/sources/test.ppm" },
        new object[] { "testResPictures/sources/correct_sample.ppm" },
        new object[] { "testResPictures/sources/incorrect_sample.ppm" }
    };
}