using Core.Lights;

namespace Writer.GIF;

public class GifPalette
{
    public Color[] BaseColors;
    private (ColorReplacementRange, byte)[] _sortedReplacementMap;

    public GifPalette(Color[] baseColors, (ColorReplacementRange, byte)[] sortedReplacementMap)
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
        long colorHash = color.GetNumericRepresentation();
        (ColorReplacementRange, byte) targetRange = GetTargetRange(colorHash);
        return targetRange.Item1.Contains(color) ? targetRange.Item2 : null;
    }

    private (ColorReplacementRange, byte) GetTargetRange(long colorHash)
    {
        int hInd = _sortedReplacementMap.Length, lInd = 0, cInd;
        while (hInd - lInd > 2)
        {
            cInd = (hInd + lInd) / 2;
            if (_sortedReplacementMap[cInd].Item1.Hash < colorHash) lInd = cInd;
            else hInd = cInd + 1;
        }

        return hInd - lInd == 2 && _sortedReplacementMap[lInd + 1].Item1.Hash <= colorHash ?
            _sortedReplacementMap[lInd + 1] : _sortedReplacementMap[lInd];
    }
}