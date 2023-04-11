using Core.Lights;

namespace Writer.GIF;

public class GifPaletteSelector
{
    public static GifPalette GetPalette(Color[,] sourceImage)
    {
        Color[] pictureColors = To1DArray(sourceImage);
        Color[] uniqueColors = GetUniqueColors(pictureColors);
        if (uniqueColors.Length <= 256) 
            return GetPaletteFromUniqueColors(uniqueColors);

        var normalizedData = Normalization.Normalize(pictureColors);
        ushort numberOfClusters = 256;
        var clusterIndexes = ColorClusterization.Clusterize(normalizedData, ref numberOfClusters);
        return GetPaletteFromClusters(pictureColors, clusterIndexes, numberOfClusters);
    }

    private static GifPalette GetPaletteFromClusters(Color[] pictureColors, ushort[] clusterIndexes, int numberOfClusters)
    {
        Color[] baseColors = GetBaseColorsFromClusters(pictureColors, clusterIndexes, numberOfClusters);
        (Color, byte)[] set = new (Color, byte)[clusterIndexes.Length];
        
        for (int i = 0; i < clusterIndexes.Length; i++)
            set[i] = (pictureColors[i], (byte)clusterIndexes[i]);
        
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
    
    private static GifPalette GetPaletteFromUniqueColors(Color[] uniqueColors)
    {
        (Color, byte)[] replacementMap = new (Color, byte)[uniqueColors.Length];

        for (int i = 0; i < uniqueColors.Length; i++)
        {
            replacementMap[i] = (uniqueColors[i], (byte)i);
        }
        
        Array.Sort(replacementMap, (p1, p2) => p1.Item1.GetNumericRepresentation().CompareTo(p2.Item1.GetNumericRepresentation()));
        return new GifPalette(uniqueColors, replacementMap);
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
    
    private static Color[] GetUniqueColors(Color[] colors)
    {
        Color[] copy = new Color[colors.Length];
        colors.CopyTo(copy, 0);
        Array.Sort(copy, (c1, c2) => c1.GetNumericRepresentation().CompareTo(c2.GetNumericRepresentation()));
        List<Color> unique = new List<Color>();
        Color previous = copy[0];
        unique.Add(previous);

        for (int i = 1; i < copy.Length; i++)
        {
            if (!copy[i].Equals(previous))
            {
                previous = copy[i];
                unique.Add(previous);
            }
        }
        
        return unique.ToArray();
    }

    private static Color[] GetBaseColorsFromClusters(Color[] pictureColors, ushort[] clusterIndexes, int numberOfClusters)
    {
        (double, double, double)[] newCenters = new (double, double, double)[numberOfClusters];
        int[] clusterCounters = new int[numberOfClusters];

        for (int i = 0; i < pictureColors.Length; i++)
        {
            clusterCounters[clusterIndexes[i]]++;
            newCenters[clusterIndexes[i]].Item1 += pictureColors[i].R;
            newCenters[clusterIndexes[i]].Item2 += pictureColors[i].G;
            newCenters[clusterIndexes[i]].Item3 += pictureColors[i].B;
        }

        Color[] baseColors = new Color[numberOfClusters];

        for (int i = 0; i < numberOfClusters; i++)
        {
            baseColors[i] = new Color(
            (byte)(newCenters[clusterIndexes[i]].Item1 / clusterCounters[i]),
            (byte)(newCenters[clusterIndexes[i]].Item2 / clusterCounters[i]),
            (byte)(newCenters[clusterIndexes[i]].Item3 / clusterCounters[i]));
        }

        return baseColors;
    } 
}