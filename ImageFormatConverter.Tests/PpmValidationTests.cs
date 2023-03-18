using ImageFormatConverter.Validators;

namespace ImageFormatConverter.Tests;

[TestFixture]
public class PpmValidationTests
{
    private PpmStructureValidator ppmValidator;
    
    [SetUp]
    public void Setup()
    {
        ppmValidator = new();
    }

    [TestCaseSource(nameof(correctFileNames))]
    public void PPM_ValidateCorrectSample_TrueReturned(string correctFileName)
    {
        bool structureValidationResult = ppmValidator.ValidateFileStructure("testResPictures/correct_sample.ppm");
        Assert.That(structureValidationResult, Is.True);
    }
    
    /*[Test]
    public void PPM_ValidateIncorrectSample_FalseReturned()
    {
        bool structureValidationResult = ppmValidator.ValidateFileStructure("testResPictures/incorrect_sample.ppm");
        Assert.That(structureValidationResult, Is.False);
    }*/

    private static object[] correctFileNames =
    {
        new object[] { "testResPictures/red_gradient.ppm" }, 
        new object[] { "testResPictures/blue_gradient.ppm" }, 
        new object[] { "testResPictures/red_blue_gradient.ppm" }, 
        new object[] { "testResPictures/test.ppm" }, 
        new object[] { "testResPictures/correct_sample.ppm" }
    };
}