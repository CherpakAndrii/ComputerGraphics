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

    [Test]
    public void BMP_ValidateCorrectSample_TrueReturned()
    {
        bool structureValidationResult = bmpValidator.ValidateFileStructure("testResPictures/sources/correct_sample.bmp");
        Assert.That(structureValidationResult, Is.True);
    }
    
    [Test]
    public void BMP_ValidateIncorrectSample_FalseReturned()
    {
        bool structureValidationResult = bmpValidator.ValidateFileStructure("testResPictures/sources/incorrect_sample.bmp");
        Assert.That(structureValidationResult, Is.False);
    }
    
    [Test]
    public void BMP_ValidateOwnPicture_TrueReturned()
    {
        bool res = bmpValidator.ValidateFileStructure("testResPictures/createdBmps/red_gradient.bmp");
        Assert.That(res, Is.True);
    }
}