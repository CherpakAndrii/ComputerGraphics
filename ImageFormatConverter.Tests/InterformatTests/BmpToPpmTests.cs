namespace ImageFormatConverter.Tests.InterformatTests;

[TestFixture]
public class BmpToPpmTests
{
    private PpmFileWriter ppmWriter;
    private BmpFileReader bmpReader;
    private PpmFileReader ppmReader;
    private BmpStructureValidator bmpValidator;
    private PpmStructureValidator ppmValidator;
    private BasicValidator basicValidator;

    [SetUp]
    public void Setup()
    {
        ppmWriter = new();
        bmpReader = new();
        ppmReader = new();
        bmpValidator = new();
        ppmValidator = new();
        basicValidator = new ();
    }
    
    [TestCaseSource(nameof(validBmps))]
    public void BmpToPpm_SamePictureGot(string inputBmpFile)
    {
        if (basicValidator.CheckFileExistence(inputBmpFile) && basicValidator.CheckFileExtension(inputBmpFile) &&
            bmpValidator.ValidateFileStructure(inputBmpFile))
        {
            var pic_orig = bmpReader.ImageToPixels(inputBmpFile);
            string filename = Regex.Match(inputBmpFile, @".+/([^/]+)\.bmp$").Groups[1].Captures[0].Value;
            string newFilepath = "testResPictures/interformat/" + filename + ".ppm";
            ppmWriter.WriteToFile(newFilepath, pic_orig);
            
            if (ppmValidator.ValidateFileStructure(newFilepath))
            {
                var pic_created = ppmReader.ImageToPixels(newFilepath);
                CollectionAssert.AreEqual(pic_orig, pic_created);
            }
            else Assert.Fail("invalid created file structure");
        }
        else Assert.Fail("invalid source file structure");
    }


    private static object[] validBmps =
    {
        new object[] { "testResPictures/createdBmps/red_gradient.bmp" }, 
        new object[] { "testResPictures/createdBmps/blue_gradient.bmp" }, 
        new object[] { "testResPictures/createdBmps/red_blue_gradient.bmp" }, 
        new object[] { "testResPictures/sources/correct_sample.bmp" }
    };
}