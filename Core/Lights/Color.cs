namespace Core.Lights;

public struct Color
{
    private byte r;
    public int R 
    { 
        get => r;
        set => r = (byte)Math.Clamp(value, 0, 255);
    }

    private byte g;
    public int G 
    { 
        get => g;
        set => g = (byte)Math.Clamp(value, 0, 255);
    }

    private byte b;
    public int B 
    { 
        get => b;
        set => b = (byte)Math.Clamp(value, 0, 255);
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
}