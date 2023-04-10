namespace Core.Lights;

public struct Color
{
    private byte _r;
    public int R 
    { 
        get => _r;
        set => _r = (byte)Math.Clamp(value, 0, 255);
    }

    private byte _g;
    public int G 
    { 
        get => _g;
        set => _g = (byte)Math.Clamp(value, 0, 255);
    }

    private byte _b;
    public int B 
    { 
        get => _b;
        set => _b = (byte)Math.Clamp(value, 0, 255);
    }

    public Color(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
    
    public Color((byte, byte, byte) color)
    {
        (R, G, B) = color;
    }
    
    public Color(Color original)
    {
        R = original.R;
        G = original.G;
        B = original.B;
    }

    public long GetNumericRepresentation() => _r << 16 + _g << 8 + _b;
}