using ImageFormatConverter.Console;

namespace ImageFormatConverter.Tests.AbstractTests;

[TestFixture]
public abstract class ImageReadingTests
{
    protected IImageWriter _imgWriter;
    protected IImageReader _imgReader;
    private static readonly ImageGenerator ImageGenerator = new();

    [TestCaseSource(nameof(_generated))]
    public void WriteAndReadImage_GotCopy(Color[,] image)
    {
        var bytes = _imgWriter.WriteToFile(image);
        var picCreated = _imgReader.ImageToPixels(bytes);

        CollectionAssert.AreEqual(image, picCreated);
    }
    
    private static object[] _generated =
    {
        new object[] { ImageGenerator.CreateRedGradientLeftToRightImage() },
        new object[] { ImageGenerator.CreateBlueGradientUpToDownImage() },
        new object[] { ImageGenerator.CreateBlueGradientUpToDownAndRedGradientLeftToRightImage() },
        new object[] { ImageGenerator.CreatePtnPnhImage() }
    };
}
