using Core.Lights;

namespace Writer.GIF;

public class GifPalette
{
    public Color[] BaseColors;
    private (Color, byte)[] _sortedReplacementMap;

    public GifPalette(Color[] baseColors, (Color, byte)[] sortedReplacementMap)
    {
        BaseColors = baseColors;
        _sortedReplacementMap = sortedReplacementMap;
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
    
    private byte? GetBaseColorIndexByOriginal(Color color)
    {
        int colorHash = color.GetNumericRepresentation();
        int lBound = 0, uBound = _sortedReplacementMap.Length, center, currentHash;

        while (lBound < uBound)
        {
            center = (lBound + uBound) / 2;
            currentHash = _sortedReplacementMap[center].Item1.GetNumericRepresentation();
            if (currentHash == colorHash) return _sortedReplacementMap[center].Item2;
            if (currentHash > colorHash) uBound = center;
            else lBound = center+1;
        }
        
        return null;
    }
}