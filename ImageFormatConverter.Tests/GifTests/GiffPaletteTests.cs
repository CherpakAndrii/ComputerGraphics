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
        var baseColorIndexes = palette.GetColorIndexes(pic);
        Color[,] pictureGot = GetPicFromPalette(palette.BaseColors, baseColorIndexes);
        File.WriteAllBytes(createdGifsDir+name+'('+palette.BaseColors.Length+')'+".bmp", new BmpFileWriter().WriteToFile(pictureGot));
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