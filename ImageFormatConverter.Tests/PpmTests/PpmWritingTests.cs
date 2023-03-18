namespace ImageFormatConverter.Tests.PpmTests;

[TestFixture]
public class PpmWritingTests
{
    private PpmFileWriter ppmFileWriter;
    [SetUp]
    public void Setup()
    {
        ppmFileWriter = new();
    }

    [Test]
    public void CreatePpmWithRedGradientLeftToRight()
    {
        Color[,] image = new Color[512, 512];
        for (int i = 0; i < 512; i++)
        {
            for (int j = 0; j < 512; j++)
            {
                image[i, j] = new Color((byte)(j / 2), 0, 0);
            }
        }

        ppmFileWriter.WriteToFile("testResPictures/createdPpms/red_gradient.ppm", image);
        Assert.Pass();
    }
    
    [Test]
    public void CreatePpmWithBlueGradientUpToDown()
    {
        Color[,] image = new Color[512, 512];
        for (int i = 0; i < 512; i++)
        {
            for (int j = 0; j < 512; j++)
            {
                image[i, j] = new Color(0, 0, (byte)(i / 2));
            }
        }

        ppmFileWriter.WriteToFile("testResPictures/createdPpms/blue_gradient.ppm", image);
        Assert.Pass();
    }
    
    [Test]
    public void CreatePpmWithBlueGradientUpToDownAndRedGradientLeftToRight()
    {
        Color[,] image = new Color[1024, 1024];
        for (int i = 0; i < 1024; i++)
        {
            for (int j = 0; j < 1024; j++)
            {
                image[i, j] = new Color((byte)(j / 4), 0, (byte)(i / 4));
            }
        }

        ppmFileWriter.WriteToFile("testResPictures/createdPpms/red_blue_gradient.ppm", image);
        Assert.Pass();
    }
}