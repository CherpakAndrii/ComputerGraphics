using Core.Lights;

namespace Writer.GIF;

public partial class GifPaletteSelector
{
    private static (Color, byte)[] GetReplacementMap((Color, ushort)[] pictureColors, ushort[] clusterIndexes)
    {
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

        return unique.ToArray();
    }
    
    private static byte?[,,] GetReplacementMap((Color, ushort)[] pictureColors, ushort[] clusterIndexes, Compression compression)
    {
        int compressionCoefficient = (int)compression;
        int tableSize = (int)Math.Ceiling(256.0 / compressionCoefficient);
        byte?[,,] table = new byte?[tableSize, tableSize, tableSize];


        for (int i = 0; i < clusterIndexes.Length; i++)
        {
            Color color = pictureColors[i].Item1;
            table[color.R, color.G, color.B] = (byte)clusterIndexes[i];
        }

        return table;
    }
    
    private static (Color, ushort)[] GetUniqueColorsWithLowMemory(Color[] colors)
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
    
    private static (Color, ushort)[] GetUniqueColorsFaster(Color[] colors, Compression compression)
    {
        int compressionCoefficient = (int)compression;
        int tableSize = (int)Math.Ceiling(256.0 / compressionCoefficient);
        ushort[,,] table = new ushort[tableSize, tableSize, tableSize];

        foreach (var color in colors)
        {
            table[color.R/compressionCoefficient, color.G/compressionCoefficient, color.B/compressionCoefficient] += 1;
        }
        
        List<(Color, ushort)> unique = new List<(Color, ushort)>();

        for (ushort r = 0; r < tableSize; r++)
        for (ushort g = 0; g < tableSize; g++)
        for (ushort b = 0; b < tableSize; b++)
        {
            ushort amount = table[r, g, b];
            if (amount > 0)
                unique.Add((new Color((byte)r, (byte)g, (byte)b), amount));
        }

        return unique.ToArray();
    }
    
    delegate byte TransformationFunction(double component, int pixels);
    private static Color[] GetBaseColorsFromClusters((Color, ushort)[] pictureColors, ushort[] clusterIndexes, int numberOfClusters, Compression compression)
    {
        TransformationFunction transform = compression == Compression.None
            ? (component, pixels) => (byte)(component / pixels)
            : (component, pixels) => (byte)((byte)(component / pixels) * (int)compression);
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
                transform(componentSums.Item1, pixelsInCluster),
                transform(componentSums.Item2, pixelsInCluster),
                transform(componentSums.Item3, pixelsInCluster));
        }

        return baseColors;
    }
    
    private static GifPalette GetPaletteFromUniqueColorsWithCompression((Color, ushort)[] uniqueColorsWithAmounts, Compression compression)
    {
        int compressionCoefficient = (int)compression;
        int tableSize = (int)Math.Ceiling(256.0 / compressionCoefficient);
        byte?[,,] table = new byte?[tableSize, tableSize, tableSize];
        Color[] baseColors = new Color[uniqueColorsWithAmounts.Length];

        for (int i = 0; i < uniqueColorsWithAmounts.Length; i++)
        {
            Color color = uniqueColorsWithAmounts[i].Item1;
            baseColors[i] = uniqueColorsWithAmounts[i].Item1;
            table[color.R, color.G, color.B] = (byte)i;
        }
        
        return new GifPalette(baseColors, table, compression);
    }
    
    private static GifPalette GetPaletteFromUniqueColorsWithoutCompression((Color, ushort)[] uniqueColorsWithAmounts)
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
}