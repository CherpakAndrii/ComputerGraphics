using Core.Lights;

namespace Writer.GIF;

public class GifPaletteSelector
{
    public static GifPalette GetPalette(Color[,] sourceImage, ushort maxColors = 256, byte iterations = 50)
    {
        Color[] pictureColors = To1DArray(sourceImage);
        (Color, ushort)[] uniqueColorsWithAmount = GetUniqueColors(pictureColors);
        if (uniqueColorsWithAmount.Length <= 256) 
            return GetPaletteFromUniqueColors(uniqueColorsWithAmount);

        var normalizedData = Normalization.Normalize(uniqueColorsWithAmount);
        ushort numberOfClusters = maxColors;
        var clusterIndexes = ColorClustering.Clusterize(normalizedData, ref numberOfClusters, pictureColors.Length, iterations);
        return GetPaletteFromClusters(uniqueColorsWithAmount, clusterIndexes, numberOfClusters);
    }

    private static GifPalette GetPaletteFromClusters((Color, ushort)[] pictureColors, ushort[] clusterIndexes, int numberOfClusters)
    {
        Color[] baseColors = GetBaseColorsFromClusters(pictureColors, clusterIndexes, numberOfClusters);
        (Color, byte)[] set = new (Color, byte)[clusterIndexes.Length];
        
        for (int i = 0; i < clusterIndexes.Length; i++)
            set[i] = (pictureColors[i].Item1, (byte)clusterIndexes[i]);
        
        Array.Sort(set, (p1, p2) => 
            p1.Item1.GetNumericRepresentation().CompareTo(p2.Item1.GetNumericRepresentation()));
        
        List<(Color, byte)> unique = new();
        var previous = set[0];
        unique.Add(previous);

        for (int i = 1; i < set.Length; i++)
        {
            if (!set[i].Item1.Equals(previous.Item1))
            {
                previous = set[i];
                unique.Add(previous);
            }
        }

        (Color, byte)[] replacementMap = unique.ToArray();
        return new GifPalette(baseColors, replacementMap);
    }
    
    private static GifPalette GetPaletteFromUniqueColors((Color, ushort)[] uniqueColorsWithAmounts)
    {
        (Color, byte)[] replacementMap = new (Color, byte)[uniqueColorsWithAmounts.Length];
        Color[] baseColors = new Color[uniqueColorsWithAmounts.Length];

        for (int i = 0; i < uniqueColorsWithAmounts.Length; i++)
        {
            baseColors[i] = uniqueColorsWithAmounts[i].Item1;
            replacementMap[i] = (baseColors[i], (byte)i);
        }
        
        Array.Sort(replacementMap, (p1, p2) => p1.Item1.GetNumericRepresentation().CompareTo(p2.Item1.GetNumericRepresentation()));
        return new GifPalette(baseColors, replacementMap);
    }

    private static Color[] To1DArray(Color[,] picture)
    {
        Color[] arr1D = new Color[picture.GetLength(0) * picture.GetLength(1)];
        int ctr = 0, n = picture.GetLength(0), m = picture.GetLength(1);
        
        for (int i = 0; i < n; i++)
        for (int j = 0; j < m; j++)
            arr1D[ctr++] = picture[i, j];

        Array.Sort(arr1D, (c1, c2) => c1.GetNumericRepresentation().CompareTo(c2.GetNumericRepresentation()));
        return arr1D;
    }
    
    private static (Color, ushort)[] GetUniqueColors(Color[] colors)
    {
        Color[] copy = new Color[colors.Length];
        colors.CopyTo(copy, 0);
        Array.Sort(copy, (c1, c2) => c1.GetNumericRepresentation().CompareTo(c2.GetNumericRepresentation()));
        List<(Color, ushort)> unique = new List<(Color, ushort)>();
        Color previous = copy[0];
        ushort colorCounter = 1;

        for (int i = 1; i < copy.Length; i++)
        {
            if (copy[i].Equals(previous))
            {
                colorCounter++;
            }
            else
            {
                unique.Add((previous, colorCounter));
                previous = copy[i];
                colorCounter = 1;
            }
        }
        
        unique.Add((previous, colorCounter));
        
        return unique.ToArray();
    }

    private static Color[] GetBaseColorsFromClusters((Color, ushort)[] pictureColors, ushort[] clusterIndexes, int numberOfClusters)
    {
        (double, double, double)[] clusterPixColComponentsSum = new (double, double, double)[numberOfClusters];
        int[] clusterCounters = new int[numberOfClusters];

        for (int i = 0; i < pictureColors.Length; i++)
        {
            var (color, amountOfSuchPixels) = pictureColors[i];
            int clusterIndex = clusterIndexes[i];
            clusterCounters[clusterIndex] += amountOfSuchPixels;
            clusterPixColComponentsSum[clusterIndex].Item1 += color.R*amountOfSuchPixels;
            clusterPixColComponentsSum[clusterIndex].Item2 += color.G*amountOfSuchPixels;
            clusterPixColComponentsSum[clusterIndex].Item3 += color.B*amountOfSuchPixels;
        }

        Color[] baseColors = new Color[numberOfClusters];

        for (int i = 0; i < numberOfClusters; i++)
        {
            int pixelsInCluster = clusterCounters[i];
            var componentSums = clusterPixColComponentsSum[i];
            baseColors[i] = new Color(
            (byte)(componentSums.Item1 / pixelsInCluster),
            (byte)(componentSums.Item2 / clusterCounters[i]),
            (byte)(componentSums.Item3 / clusterCounters[i]));
        }

        return baseColors;
    } 
}