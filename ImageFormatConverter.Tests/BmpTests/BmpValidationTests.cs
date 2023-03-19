namespace ImageFormatConverter.Tests.BmpTests;

[TestFixture]
public class BmpValidationTests
{
    private BmpStructureValidator bmpValidator;
    
    [SetUp]
    public void Setup()
    {
        bmpValidator = new();
    }

    [TestCaseSource(nameof(validBmps))]
    public void BMP_ValidateCorrectSample_TrueReturned(string correctFilename)
    {
        bool structureValidationResult = bmpValidator.ValidateFileStructure(correctFilename);
        Assert.That(structureValidationResult, Is.True);
    }

    private static object[] validBmps =
    {
        new object[] { "testResPictures/sources/correct_sample.bmp" },
        new object[] { "testResPictures/sources/incorrect_sample.bmp" },
        new object[] { "testResPictures/createdBmps/red_gradient.bmp" },
        new object[] { "testResPictures/createdBmps/blue_gradient.bmp" },
        new object[] { "testResPictures/createdBmps/red_blue_gradient.bmp" }
    };
}