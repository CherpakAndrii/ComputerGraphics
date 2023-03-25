namespace ImageFormatConverter.Tests.PpmTests;

[TestFixture]
public class PpmValidationTests
{
    private PpmFileReader _ppmValidator;
    
    [SetUp]
    public void Setup()
    {
        _ppmValidator = new();
    }

    [TestCaseSource(nameof(correctFileNames))]
    public void PPM_ValidateCorrectSample_TrueReturned(string correctFileName)
    {
        bool structureValidationResult = _ppmValidator.ValidateFileStructure(correctFileName);
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
        new object[] { "testResPictures/createdPpms/red_gradient.ppm" }, 
        new object[] { "testResPictures/createdPpms/blue_gradient.ppm" }, 
        new object[] { "testResPictures/createdPpms/red_blue_gradient.ppm" }, 
        new object[] { "testResPictures/sources/test.ppm" }, 
        new object[] { "testResPictures/sources/correct_sample.ppm" }
    };
}