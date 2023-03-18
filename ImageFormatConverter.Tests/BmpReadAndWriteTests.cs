using Core.Lights;
using ImageFormatConverter.Readers;
using ImageFormatConverter.Validators;
using ImageFormatConverter.Writers;

namespace ImageFormatConverter.Tests;

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
            bmpWriter.WriteToFile(origFileName + ".copy.bmp", image);

            byte[] originalData = File.ReadAllBytes(origFileName);
            byte[] copyData = File.ReadAllBytes(origFileName + ".copy.bmp");
            
            CollectionAssert.AreEqual(originalData, copyData);
        }
        else Assert.Fail();
    }
    
    private static object[] correctFiles =
    {
        new object[] { "testResPictures/red_gradient.bmp" }, new object[] { "testResPictures/blue_gradient.bmp" }, new object[] { "testResPictures/red_blue_gradient.bmp" }
    };
}