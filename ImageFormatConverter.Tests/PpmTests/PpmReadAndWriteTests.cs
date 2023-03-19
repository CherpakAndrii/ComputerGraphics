namespace ImageFormatConverter.Tests.PpmTests;

[TestFixture]
public class PpmReadAndWriteTests
{
    private PpmFileWriter ppmWriter;
    private PpmFileReader ppmReader;
    private PpmStructureValidator ppmValidator;
    private BasicValidator basicValidator;

    [SetUp]
    public void Setup()
    {
        ppmWriter = new();
        ppmReader = new();
        ppmValidator = new();
        basicValidator = new ();
    }

    [TestCaseSource(nameof(correctFiles))]
    public void RewriteCorrectPpm_GotCopy(string origFileName)
    {
        if (basicValidator.CheckFileExistence(origFileName) &&
            basicValidator.CheckFileExtension(origFileName) &&
            ppmValidator.ValidateFileStructure(origFileName))
        {
            Color[,] image = ppmReader.ImageToPixels(origFileName);
            string filename = Regex.Match(origFileName, @".+/([^/]+)\.ppm").Groups[1].Captures[0].Value;
            string newFilepath = "testResPictures/createdPpms/" + filename + ".copy.ppm";
            ppmWriter.WriteToFile(newFilepath, image);

            if (ppmValidator.ValidateFileStructure(newFilepath))
            {
                var pic_created = ppmReader.ImageToPixels(newFilepath);
                CollectionAssert.AreEqual(image, pic_created);
            }
            else Assert.Fail("invalid created file structure");
        }
        else Assert.Fail();
    }
    
    private static object[] correctFiles =
    {
        new object[] { "testResPictures/createdPpms/red_gradient.ppm" }, 
        new object[] { "testResPictures/createdPpms/blue_gradient.ppm" }, 
        new object[] { "testResPictures/createdPpms/red_blue_gradient.ppm" }, 
//        new object[] { "testResPictures/sources/correct_sample.ppm" }
    };
}