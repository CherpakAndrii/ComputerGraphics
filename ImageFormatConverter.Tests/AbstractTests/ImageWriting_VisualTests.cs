using ImageFormatConverter.Abstractions.Interfaces;
using ImageFormatConverter.Console;

namespace ImageFormatConverter.Tests.AbstractTests;

[TestFixture]
public abstract class ImageWriting_VisualTests
{
    protected static string TestPicturesDir = "../../../testPictures/";
    protected string Destination;
    protected string DefaultExtension;
    protected IImageWriter imgWriter;
    private static readonly ImageGenerator ImageGenerator = new();
    
    [TestCaseSource(nameof(_generated))]
    public void CreateImageFile(Func<Color[,]> generatingFunction, string name)
    {
        Color[,] picture = generatingFunction();
        byte[] filedata = imgWriter.WriteToFile(picture);
        File.WriteAllBytes(Destination+name+DefaultExtension, filedata);
        Assert.Pass();
    }

    private static object[] _generated =
    {
        new object[] { () => ImageGenerator.CreateRedGradientLeftToRightImage(), "red_gradient" },
        new object[] { () => ImageGenerator.CreateBlueGradientUpToDownImage(), "blue_gradient" },
        new object[] { () => ImageGenerator.CreateBlueGradientUpToDownAndRedGradientLeftToRightImage(), "blue_red_gradient" },
        new object[] { () => ImageGenerator.CreatePtnPnhImage(), "ptn_pnh" }
    };
}