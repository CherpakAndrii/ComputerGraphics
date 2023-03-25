using Reader.BMP;
using Reader.PPM;

namespace ImageFormatConverter.Tests.InterformatTests;

[TestFixture]
public class BmpToPpmTests
{
    private PpmFileWriter _ppmWriter;
    private BmpFileReader _bmpReader;
    private PpmFileReader _ppmReader;

    [SetUp]
    public void Setup()
    {
        _ppmWriter = new PpmFileWriter();
        _bmpReader = new BmpFileReader();
        _ppmReader = new PpmFileReader();
    }
    
    [TestCaseSource(nameof(_validBmps))]
    public void BmpToPpm_SamePictureGot(string inputBmpFile)
    {
        if (_bmpReader.ValidateFileStructure(inputBmpFile))
        {
            var picOrig = _bmpReader.ImageToPixels(inputBmpFile);
            string filename = Regex.Match(inputBmpFile, @".+/([^/]+)\.bmp$").Groups[1].Captures[0].Value;
            string newFilepath = "testResPictures/interformat/" + filename + ".ppm";
            _ppmWriter.WriteToFile(newFilepath, picOrig);
            
            if (_ppmReader.ValidateFileStructure(newFilepath))
            {
                var picCreated = _ppmReader.ImageToPixels(newFilepath);
                CollectionAssert.AreEqual(picOrig, picCreated);
            }
            else Assert.Fail("invalid created file structure");
        }
        else Assert.Fail("invalid source file structure");
    }


    private static object[] _validBmps =
    {
        new object[] { "testResPictures/createdBmps/red_gradient.bmp" }, 
        new object[] { "testResPictures/createdBmps/blue_gradient.bmp" }, 
        new object[] { "testResPictures/createdBmps/red_blue_gradient.bmp" }, 
        new object[] { "testResPictures/sources/correct_sample.bmp" },
        new object[] { "testResPictures/sources/incorrect_sample.bmp" }
    };
}