using Core.Lights;

namespace Writer.GIF;

public class GifPalette
{
    public Color[] BaseColors;
    private object _sortedReplacementMap;

    private delegate byte? ColorTransformationFunction(Color c);

    private ColorTransformationFunction GetBaseColorIndexByOriginal;

    public GifPalette(Color[] baseColors, (Color, byte)[] sortedReplacementMap)
    {
        BaseColors = baseColors;
        _sortedReplacementMap = sortedReplacementMap;
        GetBaseColorIndexByOriginal = GetBaseColorIndexByOriginalWithoutCompression;
    }
    
    public GifPalette(Color[] baseColors, byte?[,,] sortedReplacementMap, Compression compression)
    {
        BaseColors = baseColors;
        _sortedReplacementMap = sortedReplacementMap;
        GetBaseColorIndexByOriginal = GetBaseColorIndexByOriginalWithCompression;
    }

    public byte[,] GetColorIndexes(Color[,] originalPicture)
    {
        int n = originalPicture.GetLength(0), m = originalPicture.GetLength(1);
        byte[,] indexes = new byte[n, m];
        
        byte? ind;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                ind = GetBaseColorIndexByOriginal(originalPicture[i, j]);
                if (ind is null)
                    throw new IndexOutOfRangeException("The palette does not match the picture");
                indexes[i, j] = ind.Value;
            }
        }

        return indexes;
    }
    
    private byte? GetBaseColorIndexByOriginalWithoutCompression(Color color)
    {
        (Color, byte)[] map = ((Color, byte)[])_sortedReplacementMap;
        var transform = (int component) => (byte)component;
        color = new Color(transform(color.R), transform(color.G), transform(color.B));
        int colorHash = color.GetNumericRepresentation();
        int lBound = 0, uBound = map.Length, center, currentHash;

        while (lBound < uBound)
        {
            center = (lBound + uBound) / 2;
            currentHash = map[center].Item1.GetNumericRepresentation();
            if (currentHash == colorHash) return map[center].Item2;
            if (currentHash > colorHash) uBound = center;
            else lBound = center+1;
        }
        
        return null;
    }
    
    private byte? GetBaseColorIndexByOriginalWithCompression(Color color)
    {
        var transform = (int component) => (byte)(component / 3);
        byte?[,,] map = (byte?[,,])_sortedReplacementMap;
        return map[transform(color.R), transform(color.G), transform(color.B)];
    }
}