using System.Runtime.InteropServices.ComTypes;
using ImageFormatConverter.Console;
using Writer.BMP;
using Writer.GIF;

namespace ImageFormatConverter.Tests.GifTests;

[TestFixture]
public class GiffPaletteTests
{
    private static string createdGifsDir = "../../../testPictures/CreatedGifs/";
    private static ImageGenerator ImageGenerator = new ();
    
    [TestCaseSource(nameof(_validPalettes))]
    public void GIF_ValidatePaletteSelector(Color[,] pict, Color[] baseColors, byte[,] indexes)
    {
        var palette = GifPaletteSelector.GetPalette(pict);
        var baseColorIndexes = palette.GetColorIndexes(pict);

        Color[,] expectedNewPic = GetPicFromPalette(baseColors, indexes);
        Color[,] pictureGot = GetPicFromPalette(palette.BaseColors, baseColorIndexes);
        
        
        CollectionAssert.AreEqual(expectedNewPic, pictureGot);
    }

    [TestCaseSource(nameof(_generatedPics))]
    public void GIF_VisualPaletteSelectorTesting(Func<Color[,]> generatingFunction, string name)
    {
        var pic = generatingFunction();
        var palette = GifPaletteSelector.GetPalette(pic);
        VisualizePalette(palette, name);
        var baseColorIndexes = palette.GetColorIndexes(pic);
        Color[,] pictureGot = GetPicFromPalette(palette.BaseColors, baseColorIndexes);
        File.WriteAllBytes(createdGifsDir+name+'('+palette.BaseColors.Length+')'+".bmp", new BmpFileWriter().WriteToFile(pictureGot));
    }
    
    private static void VisualizePalette(GifPalette palette, string origPicName)
    {
        (int n, int m) = GetOptimalSize(palette);
        Color[,] plt = new Color[n*10, m*10];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m && i*m+j < palette.BaseColors.Length; j++)
            {
                Color clr = palette.BaseColors[i * m + j];
                for (int k = 0; k < 10; k++)
                {
                    for (int l = 0; l < 10; l++)
                    {
                        plt[i * 10 + k, j * 10 + l] = clr;
                    }
                }
            }
        }

        string name = createdGifsDir + origPicName + "_palette.bmp";
        var pic = new BmpFileWriter().WriteToFile(plt);
        File.WriteAllBytes(name, pic);
    }

    private static (int, int) GetOptimalSize(GifPalette palette)
    {
        int colorArraySize = palette.BaseColors.Length, n = 1, m = 1;
        for (int i = 2; i <= 16; i++)
        {
            if (i * i >= colorArraySize)
            {
                n = m = i;
                break;
            }
        }

        while (n * m >= colorArraySize)
            n--;

        return (n, m);
    }

    private static object[] _generatedPics = {
        new object[] { () => ImageGenerator.CreateRedGradientLeftToRightImage(), "red_gradient" },
        new object[] { () => ImageGenerator.CreateBlueGradientUpToDownImage(), "blue_gradient" },
        new object[] { () => ImageGenerator.CreateBlueGradientUpToDownAndRedGradientLeftToRightImage(), "blue_red_gradient" },
        new object[] { () => ImageGenerator.CreatePtnPnhImage(), "ptn_pnh" }
    };

    
    
    
    private static object[] _validPalettes = {
        new object[] { new Color[,]{{new(0, 0, 0), new (0, 255, 0)}, {new (0, 255, 0), new(255, 255, 255)}}, 
            new Color[] {new(0, 0, 0), new (0, 255, 0), new (255, 255, 255)}, 
            new byte[,] {{0, 1}, {1, 2}} }
    };
    
    private Color[,] GetPicFromPalette(Color[] baseColors, byte[,] indexes)
    {
        Color[,] newPic = new Color[indexes.GetLength(0), indexes.GetLength(1)];
        for (int i = 0; i < indexes.GetLength(0); i++)
        {
            for (int j = 0; j < indexes.GetLength(1); j++)
            {
                newPic[i, j] = baseColors[indexes[i, j]];
            }
        }

        return newPic;
    }
}