using Writer.GIF;

namespace ImageFormatConverter.Tests.GifTests;

[TestFixture]
public class GiffPaletteTests
{
    [TestCaseSource(nameof(_validPalettes))]
    public void GIF_ValidatePaletteSelector(Color[,] pict, Color[] baseColors, byte[,] indexes)
    {
        var palette = GifPaletteSelector.GetPalette(pict);
        var baseColorIndexes = palette.GetColorIndexes(pict);
        
        Assert.Multiple(() =>
        {
            CollectionAssert.AreEqual(baseColors, palette.BaseColors, message: "colors are not equal");
            CollectionAssert.AreEqual(indexes, baseColorIndexes, message: "map is not equal");
        });
    }
    
    private static object[] _validPalettes = {
        new object[] { new Color[,]{{new(0, 0, 0), new (0, 255, 0)}, {new (0, 255, 0), new(255, 255, 255)}}, 
            new Color[] {new(0, 0, 0), new (0, 255, 0), new (255, 255, 255)}, 
            new byte[,] {{0, 1}, {1, 2}} }
    };
}