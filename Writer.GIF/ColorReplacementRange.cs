using Core.Lights;

namespace Writer.GIF;

public readonly struct ColorReplacementRange
{
    public ColorReplacementRange(byte rMin, byte rMax, byte gMin, byte gMax, byte bMin, byte bMax)
    {
        _rMin = rMin;
        _rMax = rMax;
        _gMin = gMin;
        _gMax = gMax;
        _bMin = bMin;
        _bMax = bMax;
        
        Hash = (_rMin << 16) + (_gMin << 8) + _bMin;
    }

    private readonly byte _rMin;
    private readonly byte _rMax;
    private readonly byte _gMin;
    private readonly byte _gMax;
    private readonly byte _bMin;
    private readonly byte _bMax;

    public bool Contains(Color c) =>
        c.R >= _rMin && c.R <= _rMax && c.G >= _gMin && c.G <= _gMax && c.B >= _bMin && c.B <= _bMax;
    
    public int Hash { get; }
}