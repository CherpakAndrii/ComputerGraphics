namespace ImageFormatConverter.Tests.BasicTests;

[TestFixture]
public class BasicValidationTests
{
    private BasicValidator basicValidator;
    
    [SetUp]
    public void Setup()
    {
        basicValidator = new();
    }
    
    [Test]
    public void BasicValidation_CorrectExistingFile_TrueReturned()
    {
        bool existence = basicValidator.CheckFileExistence("testResPictures/sources/correct_sample.bmp");
        bool extension = basicValidator.CheckFileExtension("testResPictures/sources/correct_sample.bmp");
        Assert.Multiple(() =>
        {
            Assert.That(existence, Is.True);
            Assert.That(extension, Is.True);
        });
    }
    
    [Test]
    public void BasicValidation_AnotherCorrectExistingFile_TrueReturned()
    {
        bool existence = basicValidator.CheckFileExistence("testResPictures/sources/existing.correct.extension.bmp");
        bool extension = basicValidator.CheckFileExtension("testResPictures/sources/existing.correct.extension.bmp");
        Assert.Multiple(() =>
        {
            Assert.That(existence, Is.True);
            Assert.That(extension, Is.True);
        });
    }
    
    [Test]
    public void BasicValidation_CorrectNonExistentFile_FalseForExistenceReturned()
    {
        bool existence = basicValidator.CheckFileExistence("testResPictures/sources/non-existent.bmp");
        bool extension = basicValidator.CheckFileExtension("testResPictures/sources/non-existent.bmp");
        Assert.Multiple(() =>
        {
            Assert.That(existence, Is.False);
            Assert.That(extension, Is.True);
        });
    }
    
    [Test]
    public void BasicValidation_IncorrectExtensionOfExistentFile_FalseForExtensionReturned()
    {
        bool existence = basicValidator.CheckFileExistence("testResPictures/sources/incorrect_extension.txt");
        bool extension = basicValidator.CheckFileExtension("testResPictures/sources/incorrect_extension.txt");
        Assert.Multiple(() =>
        {
            Assert.That(existence, Is.True);
            Assert.That(extension, Is.False);
        });
    }
}