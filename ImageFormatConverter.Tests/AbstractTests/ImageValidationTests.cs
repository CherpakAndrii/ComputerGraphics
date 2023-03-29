using ImageFormatConverter.Console;

namespace ImageFormatConverter.Tests.AbstractTests;

[TestFixture]
public abstract class ImageValidationTests
{
    protected const string TestPicturesDir = "../../../testPictures/sources/";
    protected string SourceDir;
    protected IImageReader _imgFileReader;
    protected IImageWriter _imgWriter;


    [Test]
    public void ValidateGeneratedPicture_TrueReturned()
    {
        Color[,] image = new ImageGenerator().CreateBlueGradientUpToDownAndRedGradientLeftToRightImage();
        var bytes = _imgWriter.WriteToFile(image);
        
        var structureValidationResult = _imgFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.True);
    }
    
    public void ValidateCorrectSample_TrueReturned(string filename)
    {
        var bytes = File.ReadAllBytes(SourceDir+filename);
        var structureValidationResult = _imgFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.True);
    }
    
    public void ValidateIncorrectSample_FalseReturned(string filename)
    {
        var bytes = File.ReadAllBytes(SourceDir+filename);
        var structureValidationResult = _imgFileReader.ValidateFileStructure(bytes);
        
        Assert.That(structureValidationResult, Is.False);
    }
}