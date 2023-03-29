namespace ImageFormatConverter.Tests.AbstractTests;

[TestFixture]
public abstract class InterformatConversionTests
{
    protected const string TestPicturesDir = "../../../testPictures/";
    protected string SourceDir;
    protected string DestinationDir;
    protected string newExtension;
    protected string oldExtension;
    protected IImageReader _origImgFileReader;
    protected IImageWriter _convertedImgWriter;
    protected IImageReader _convertedImgReader;
    
    public void ConvertPicture(string filename)
    {
        var originalBytes = File.ReadAllBytes(SourceDir+filename);
        var origPicture = _origImgFileReader.ImageToPixels(originalBytes);
        var convertedBytes = _convertedImgWriter.WriteToFile(origPicture);

        var validationResult = _convertedImgReader.ValidateFileStructure(convertedBytes);
        var convertedPicture = _convertedImgReader.ImageToPixels(convertedBytes);
        
        File.WriteAllBytes(DestinationDir+$"{filename}.{newExtension}", convertedBytes);
        Assert.Multiple(() => 
            {
                Assert.That(validationResult, Is.True);
                CollectionAssert.AreEqual(origPicture, convertedPicture);
            });
    }
    
    /// <summary>
    /// Used for formats without readers and validators
    /// </summary>
    /// <param name="filename"></param>
    public void ConvertPictureWithoutValidation(string filename)
    {
        var originalBytes = File.ReadAllBytes(SourceDir+filename);
        var origPicture = _origImgFileReader.ImageToPixels(originalBytes);
        var convertedBytes = _convertedImgWriter.WriteToFile(origPicture);
        
        File.WriteAllBytes(DestinationDir+$"{filename}.{newExtension}", convertedBytes);
        Assert.Pass();
    }
}