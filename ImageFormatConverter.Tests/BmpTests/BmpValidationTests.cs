using Reader.BMP;

namespace ImageFormatConverter.Tests.BmpTests;

[TestFixture]
public class BmpValidationTests
{
    private BmpFileReader _bmpValidator;
    
    [SetUp]
    public void Setup()
    {
        _bmpValidator = new BmpFileReader();
    }

    [TestCaseSource(nameof(_validBmps))]
    public void BMP_ValidateCorrectSample_TrueReturned(string correctFilename)
    {
        var structureValidationResult = _bmpValidator.ValidateFileStructure(correctFilename);
        Assert.That(structureValidationResult, Is.True);
    }

    private static object[] _validBmps =
    {
        new object[] { "testResPictures/sources/correct_sample.bmp" },
        new object[] { "testResPictures/sources/incorrect_sample.bmp" },
        new object[] { "testResPictures/createdBmps/red_gradient.bmp" },
        new object[] { "testResPictures/createdBmps/blue_gradient.bmp" },
        new object[] { "testResPictures/createdBmps/red_blue_gradient.bmp" }
    };
}