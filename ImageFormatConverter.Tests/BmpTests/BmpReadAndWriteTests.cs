namespace ImageFormatConverter.Tests.BmpTests;

[TestFixture]
public class BmpReadAndWriteTests
{
    private BmpFileWriter bmpWriter;
    private BmpFileReader bmpReader;
    private BmpStructureValidator bmpValidator;
    private BasicValidator basicValidator;

    [SetUp]
    public void Setup()
    {
        bmpWriter = new();
        bmpReader = new();
        bmpValidator = new();
        basicValidator = new ();
    }

    [TestCaseSource(nameof(correctFiles))]
    public void RewriteCorrectBmp_GotCopy(string origFileName)
    {
        if (basicValidator.CheckFileExistence(origFileName) &&
            basicValidator.CheckFileExtension(origFileName) &&
            bmpValidator.ValidateFileStructure(origFileName))
        {
            Color[,] image = bmpReader.ImageToPixels(origFileName);
            string filename = Regex.Match(origFileName, @".+/([^/]+)\.bmp$").Groups[1].Captures[0].Value;
            string newFilepath = "testResPictures/createdBmps/" + filename + ".copy.bmp";
            bmpWriter.WriteToFile(newFilepath, image);

            if (bmpValidator.ValidateFileStructure(newFilepath))
            {
                var pic_created = bmpReader.ImageToPixels(newFilepath);
                CollectionAssert.AreEqual(image, pic_created);
            }
            else Assert.Fail("invalid created file structure");
        }
        else Assert.Fail();
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