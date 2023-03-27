using Core.Lights;

namespace ImageFormatConverter.Console;

public class ImageGenerator
{
    public Color[,] CreateRedGradientLeftToRightImage()
    {
        Color[,] image = new Color[512, 512];
        for (int i = 0; i < 512; i++)
        {
            for (int j = 0; j < 512; j++)
            {
                image[i, j] = new Color((byte)(j / 2), 0, 0);
            }
        }

        return image;
    }
    
    public Color[,] CreateBlueGradientUpToDownImage()
    {
        Color[,] image = new Color[512, 512];
        for (int i = 0; i < 512; i++)
        {
            for (int j = 0; j < 512; j++)
            {
                image[i, j] = new Color(0, 0, (byte)(i / 2));
            }
        }

        return image;
    }
    
    public Color[,] CreateBlueGradientUpToDownAndRedGradientLeftToRightImage()
    {
        Color[,] image = new Color[1024, 1024];
        for (int i = 0; i < 1024; i++)
        {
            for (int j = 0; j < 1024; j++)
            {
                image[i, j] = new Color((byte)(j / 4), 0, (byte)(i / 4));
            }
        }

        return image;
    }

    public Color[,] CreatePtnPnhImage()
    {
        (int, int)[] blackVerticalLineCoors = new[] { (20, 60), (20, 90), (20, 130), (20, 170), (20, 200) };
        (int, int)[] redVerticalLineCoors = new[] { (110, 60), (110, 90), (110, 110), (110, 140) };
        (int, int)[] blackHorizontalLineCoors = new[] { (20, 70), (20, 110), (20, 140), (40, 180) };
        (int, int)[] redHorizontalLineCoors = new[] { (110, 70), (130, 120) };

        Color red = new Color(255, 0, 0);
        Color black = new Color(0, 0, 0);


        Color[,] image = new Color[180, 270];
        
        for (int i = 0; i < 90; i++)
        for (int j = 0; j < 270; j++)
            image[i, j] = red;

        foreach (var (y, x) in blackVerticalLineCoors)
            for (int i = y; i < y + 50; i++)
            for (int j = x; j < x + 10; j++)
                image[i, j] = black;

        foreach (var (y, x) in blackHorizontalLineCoors)
            for (int i = y; i < y + 10; i++)
            for (int j = x; j < x + 20; j++)
                image[i, j] = black;

        foreach (var (y, x) in redVerticalLineCoors)
            for (int i = y; i < y + 50; i++)
            for (int j = x; j < x + 10; j++)
                image[i, j] = red;

        foreach (var (y, x) in redHorizontalLineCoors)
            for (int i = y; i < y + 10; i++)
            for (int j = x; j < x + 20; j++)
                image[i, j] = red;

        for (int i = 110; i < 160; i++)
        for (int j = i * 4 / 5; j < (i * 4 / 5) + 10; j++)
        {
            image[i, 160 + j] = red;
            image[i, 209 - j] = red;
        }

        return image;
    }
}