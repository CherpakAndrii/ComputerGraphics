using Core.Lights;

namespace Writer.GIF;

public class GifPaletteSelector
{
    public static GifPalette GetPalette(Color[,] sourceImage)
    {
        Color[] pictureColors = To1DArray(sourceImage);
        List<PixelsRange> ranges = GetRanges(pictureColors);
        return GetPaletteFromRanges(ranges);
    }

    private static List<PixelsRange> GetRanges(Color[] colors)
    {
        List<PixelsRange> ranges = new List<PixelsRange> { new (colors, 0, colors.Length) };
        while (ranges.Count < 255)
        {
            PixelsRange withMaxDiff = ranges[0];
            for (int i = 1; i < ranges.Count; i++)
            {
                if (ranges[i].Delta > withMaxDiff.Delta) withMaxDiff = ranges[i];
            }
            
            if (withMaxDiff.Delta < 5) break;
            ranges.Add(withMaxDiff.Split());
        }

        return ranges;
    }

    private static GifPalette GetPaletteFromRanges(List<PixelsRange> ranges)
    {
        Color[] baseColors = new Color[ranges.Count];
        (ColorReplacementRange, byte)[] replacementMap = new (ColorReplacementRange, byte)[ranges.Count];

        for (int i = 0; i < baseColors.Length; i++)
        {
            baseColors[i] = ranges[i].GetAvgColor();
            replacementMap[i] = (ranges[i].ToColorReplacementRange(), (byte)i);
        }
        
        Array.Sort(replacementMap, (p1, p2) => p1.Item1.Hash.CompareTo(p2.Item1.Hash));

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
}