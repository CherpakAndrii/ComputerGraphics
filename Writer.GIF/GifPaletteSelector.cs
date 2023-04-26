using Core.Lights;

namespace Writer.GIF;

public partial class GifPaletteSelector
{
    public static GifPalette GetPalette(Color[,] sourceImage, ushort maxColors = 256, byte iterations = 50)
    {
        Color[] pictureColors = To1DArray(sourceImage);
        Compression compression = GetCompression(pictureColors.Length);
        (Color, ushort)[] uniqueColorsWithAmount = GetUniqueColors(pictureColors, compression);
        if (uniqueColorsWithAmount.Length <= maxColors)
            return GetPaletteFromUniqueColors(uniqueColorsWithAmount, compression);
        var normalizedData = Normalization.Normalize(uniqueColorsWithAmount);
        ushort numberOfClusters = maxColors;
        var clusterIndexes = ColorClustering.Clusterize(normalizedData, ref numberOfClusters, pictureColors.Length, iterations);
        return GetPaletteFromClusters(uniqueColorsWithAmount, clusterIndexes, numberOfClusters, compression);
    }
    
    private static Color[] To1DArray(Color[,] picture)
    {
        Color[] arr1D = new Color[picture.GetLength(0) * picture.GetLength(1)];
        int ctr = 0, n = picture.GetLength(0), m = picture.GetLength(1);
        
        for (int i = 0; i < n; i++)
        for (int j = 0; j < m; j++)
            arr1D[ctr++] = picture[i, j];

        return arr1D;
    }

    private static Compression GetCompression(int numOfColors)
    {
        return numOfColors switch
        {
            < 2_000_000 => Compression.None,
            < 10_000_000 => Compression.Low,
            < 15_000_000 => Compression.Medium,
            _ => Compression.High
        };
    }

    private static (Color, ushort)[] GetUniqueColors(Color[] colors, Compression compression)
    {
        return compression == Compression.None
            ? GetUniqueColorsWithLowMemory(colors)
            : GetUniqueColorsFaster(colors, compression);
    }
    
    private static GifPalette GetPaletteFromUniqueColors((Color, ushort)[] uniqueColorsWithAmounts, Compression compression)
    {
        return compression == Compression.None
            ? GetPaletteFromUniqueColorsWithoutCompression(uniqueColorsWithAmounts)
            : GetPaletteFromUniqueColorsWithCompression(uniqueColorsWithAmounts, compression);
    }
    
    private static GifPalette GetPaletteFromClusters((Color, ushort)[] pictureColors, ushort[] clusterIndexes, int numberOfClusters, Compression compression)
    {
        Color[] baseColors = GetBaseColorsFromClusters(pictureColors, clusterIndexes, numberOfClusters, compression);

        if (compression == Compression.None)
        {
            (Color, byte)[] replacementMap = GetReplacementMap(pictureColors, clusterIndexes);
            return new GifPalette(baseColors, replacementMap);
        }
        byte?[,,] replacemntMap = GetReplacementMap(pictureColors, clusterIndexes, compression);
        return new GifPalette(baseColors, replacemntMap, compression);
    }
}