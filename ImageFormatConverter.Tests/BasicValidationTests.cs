using Core.Lights;
using ImageFormatConverter.Validators;

namespace ImageFormatConverter.Tests;

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
        bool existence = basicValidator.CheckFileExistence("testResPictures/correct_sample.bmp");
        bool extension = basicValidator.CheckFileExtension("testResPictures/correct_sample.bmp");
        Assert.Multiple(() =>
        {
            Assert.That(existence, Is.True);
            Assert.That(extension, Is.True);
        });
    }
    
    [Test]
    public void BasicValidation_AnotherCorrectExistingFile_TrueReturned()
    {
        bool existence = basicValidator.CheckFileExistence("testResPictures/existing.correct.extension.bmp");
        bool extension = basicValidator.CheckFileExtension("testResPictures/existing.correct.extension.bmp");
        Assert.Multiple(() =>
        {
            Assert.That(existence, Is.True);
            Assert.That(extension, Is.True);
        });
    }
    
    [Test]
    public void BasicValidation_CorrectNonExistentFile_FalseForExistenceReturned()
    {
        bool existence = basicValidator.CheckFileExistence("testResPictures/non-existent.bmp");
        bool extension = basicValidator.CheckFileExtension("testResPictures/non-existent.bmp");
        Assert.Multiple(() =>
        {
            Assert.That(existence, Is.False);
            Assert.That(extension, Is.True);
        });
    }
    
    [Test]
    public void BasicValidation_IncorrectExtensionOfExistentFile_FalseForExtensionReturned()
    {
        bool existence = basicValidator.CheckFileExistence("testResPictures/incorrect_extension.txt");
        bool extension = basicValidator.CheckFileExtension("testResPictures/incorrect_extension.txt");
        Assert.Multiple(() =>
        {
            Assert.That(existence, Is.True);
            Assert.That(extension, Is.False);
        });
    }
}