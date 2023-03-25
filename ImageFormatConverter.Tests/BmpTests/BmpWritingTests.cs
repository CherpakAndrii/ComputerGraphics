using Writer.BMP;

namespace ImageFormatConverter.Tests.BmpTests;

[TestFixture]
public class BmpWritingTests
{
    private BmpFileWriter bmpWriter;
    [SetUp]
    public void Setup()
    {
        bmpWriter = new();
    }

    [Test]
    public void CreateBmpWithRedGradientLeftToRight()
    {
        Color[,] image = new Color[512, 512];
        for (int i = 0; i < 512; i++)
        {
            for (int j = 0; j < 512; j++)
            {
                image[i, j] = new Color((byte)(j / 2), 0, 0);
            }
        }

        bmpWriter.WriteToFile("testResPictures/createdBmps/red_gradient.bmp", image);
        Assert.Pass();
    }
    
    [Test]
    public void CreateBmpWithBlueGradientUpToDown()
    {
        Color[,] image = new Color[512, 512];
        for (int i = 0; i < 512; i++)
        {
            for (int j = 0; j < 512; j++)
            {
                image[i, j] = new Color(0, 0, (byte)(i / 2));
            }
        }

        bmpWriter.WriteToFile("testResPictures/createdBmps/blue_gradient.bmp", image);
        Assert.Pass();
    }
    
    [Test]
    public void CreateBmpWithBlueGradientUpToDownAndRedGradientLeftToRight()
    {
        Color[,] image = new Color[1024, 1024];
        for (int i = 0; i < 1024; i++)
        {
            for (int j = 0; j < 1024; j++)
            {
                image[i, j] = new Color((byte)(j / 4), 0, (byte)(i / 4));
            }
        }

        bmpWriter.WriteToFile("testResPictures/createdBmps/red_blue_gradient.bmp", image);
        Assert.Pass();
    }
}