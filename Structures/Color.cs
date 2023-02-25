namespace Structures;

public struct Color
{
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }

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
}