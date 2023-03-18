namespace ImageFormatConverter.Tests.InterformatTests;

[TestFixture]
public class PpmToBmpTests
{
    private BmpFileWriter bmpWriter;
    private PpmFileReader ppmReader;
    private BmpFileReader bmpReader;
    private PpmStructureValidator ppmValidator;
    private BmpStructureValidator bmpValidator;
    private BasicValidator basicValidator;

    [SetUp]
    public void Setup()
    {
        bmpWriter = new();
        ppmReader = new();
        bmpReader = new();
        ppmValidator = new();
        bmpValidator = new();
        basicValidator = new ();
    }
    
    [TestCaseSource(nameof(validPpms))]
    public void PpmToBmp_SamePictureGot(string inputPpmFile)
    {
        if (!basicValidator.CheckFileExistence(inputPpmFile) || !basicValidator.CheckFileExtension(inputPpmFile)) 
            Assert.Fail("file not found");
        
        else if (ppmValidator.ValidateFileStructure(inputPpmFile))
        {
            var pic_orig = ppmReader.ImageToPixels(inputPpmFile);
            string filename = Regex.Match(inputPpmFile, @".+/([^/]+)\.ppm$").Groups[1].Captures[0].Value;
            string newFilepath = "testResPictures/interformat/" + filename + ".bmp";
            bmpWriter.WriteToFile(newFilepath, pic_orig);
            
            if (bmpValidator.ValidateFileStructure(newFilepath))
            {
                var pic_created = bmpReader.ImageToPixels(newFilepath);
                CollectionAssert.AreEqual(pic_orig, pic_created);
            }
            else Assert.Fail("invalid created file structure");
        }
        else Assert.Fail("invalid source file structure");
    }


    private static object[] validPpms =
    {
        new object[] { "testResPictures/createdPpms/red_gradient.ppm" }, 
        new object[] { "testResPictures/createdPpms/blue_gradient.ppm" }, 
        new object[] { "testResPictures/createdPpms/red_blue_gradient.ppm" },
        new object[] { "testResPictures/sources/test.ppm" },
        new object[] { "testResPictures/sources/correct_sample.ppm" },
        new object[] { "testResPictures/sources/incorrect_sample.ppm" }
    };
}