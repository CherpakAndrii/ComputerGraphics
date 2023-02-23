namespace Structures;

public struct Point : IEquatable<Point>
{
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Z { get; private set; }

    public Point(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Point((float, float, float) coordinates)
    {
        X = coordinates.Item1;
        Y = coordinates.Item2;
        Z = coordinates.Item3;
    }
    
    public Point(Point original)
    {
        X = original.X;
        Y = original.Y;
        Z = original.Z;
    }

    public static Point operator +(Point point, Vector vector)
    {
        return new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
    }

    public static Point operator -(Point point, Vector vector)
    {
        return point + (-vector);
    }

    public bool Equals(Point other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        return obj is Point other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}