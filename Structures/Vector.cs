namespace Structures;

public struct Vector : IEquatable<Vector>
{
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Z { get; private set; }

    public Vector(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector((float, float, float) coordinates)
    {
        X = coordinates.Item1;
        Y = coordinates.Item2;
        Z = coordinates.Item3;
    }

    public float GetModule()
    {
        return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
    }
    
    public static Vector operator +(Vector vector1, Vector vector2)
    {
        return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z);
    }

    public static Vector operator -(Vector vector)
    {
        return new Vector(-vector.X, -vector.Y, -vector.Z);
    }
    
    public static Vector operator -(Vector vector1, Vector vector2)
    {
        return new Vector(vector1.X-vector2.X, vector1.Y-vector2.Y, vector1.Z-vector2.Z);
    }

    public void Normalize()
    {
        float module = GetModule();
        X /= module;
        Y /= module;
        Z /= module;
    }

    public bool Equals(Vector other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}