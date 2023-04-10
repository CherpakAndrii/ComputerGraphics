using Core.Lights;

namespace Writer.GIF;

internal struct PixelsRange
{
    public int StartInd { get; }
    public int EndInd { get; private set; }
    private readonly Color[] _colors;
    private byte maxR, minR, maxG, minG, maxB, minB;
    public int Delta;

    public PixelsRange(Color[] colors, int startInd, int endInd)
    {
        _colors = colors;
        StartInd = startInd;
        EndInd = endInd;
        UpdateStats();
    }

    public Color GetAvgColor()
    {
        return new Color(
            (byte)((maxR - minR) / 2 + minR), 
            (byte)((maxG - minG) / 2 + minG), 
            (byte)((maxB - minB) / 2 + minB));
    }
    
    public ColorReplacementRange ToColorReplacementRange()
    {
        return new ColorReplacementRange(minR, maxR, minG, maxG, minB, maxB);
    }
    
    public PixelsRange Split()
    {
        byte rDiff = (byte)(maxR - minR), gDiff = (byte)(maxG - minG), bDiff = (byte)(maxB - minB);
        Func<Color, byte> lambda;
        if (rDiff >= gDiff && rDiff >= bDiff) lambda = (Color c) => (byte)c.R;
        else if (gDiff > bDiff) lambda = (Color c) => (byte)c.G;
        else lambda = (Color c) => (byte)c.B;
        
        int centralElementIndex = OrderArr(lambda);
        PixelsRange newPixRange = new PixelsRange(_colors, centralElementIndex, EndInd);
        EndInd = centralElementIndex;
        UpdateStats();
        return newPixRange;
    }

    private void UpdateStats()
    {
        maxR = minR = (byte)_colors[StartInd].R;
        maxG = minG = (byte)_colors[StartInd].G;
        maxB = minB = (byte)_colors[StartInd].B;

        for (int i = StartInd+1; i < EndInd; i++)
        {
            Color current = _colors[i];
            
            if (current.R > maxR) maxR = (byte)current.R;
            else if (current.R < minR) minR = (byte)current.R;
            
            if (current.G > maxG) maxG = (byte)current.G;
            else if (current.G < minG) minG = (byte)current.G;
            
            if (current.B > maxB) maxB = (byte)current.B;
            else if (current.B < minB) minB = (byte)current.B;
        }

        Delta = maxR + maxG + maxB - (minR + minG + minB);
    }

    private int OrderArr(Func<Color, byte> lambda)
    {
        int pivotInd = GetPivotIndex(lambda);
        (_colors[pivotInd], _colors[EndInd - 1]) = (_colors[EndInd - 1], _colors[pivotInd]);
        pivotInd = EndInd - 1;
        byte pivotVal = lambda(_colors[pivotInd]);

        int i = StartInd - 1;
        for (int j = StartInd; j <= pivotInd; j++)
        {
            if (lambda(_colors[j]) <= pivotVal){
                i++;
                (_colors[i], _colors[j]) = (_colors[j], _colors[i]);
            }
        }
        
        (_colors[i], _colors[pivotInd]) = (_colors[pivotInd], _colors[i]);
        return i;
    }

    private int GetPivotIndex(Func<Color, byte> lambda)
    {
        byte pivotValue = lambda(GetAvgColor());
        int pivotIndex = StartInd, minDiff = 256, currDiff;
        for (int i = StartInd; i < EndInd; i++)
        {
            currDiff = Math.Abs(lambda(_colors[i]) - pivotValue);
            if (currDiff < minDiff)
            {
                minDiff = currDiff;
                pivotIndex = i;
            }
        }

        return pivotIndex;
    }
}