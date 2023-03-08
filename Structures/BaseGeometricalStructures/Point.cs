namespace Structures.BaseGeometricalStructures;

public struct Point : IEquatable<Point>
{
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Z { get; private set; }
    
    private const float ε = 1e-2f;

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

    public float GetDistance(Point point2)
    {
        float dx = X - point2.X, dy = Y - point2.Y, dz = Z - point2.Z;
        return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public static Point operator +(Point point, Vector vector)
    {
        return new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
    }

    public static Point operator -(Point point, Vector vector)
    {
        return point + (-vector);
    }

    // TODO: may not work correctly, needs to be fixed if this method is to be used.
    public bool Equals(Point other)
    {
        return Math.Abs(X - other.X) < ε && Math.Abs(Y - other.Y) < ε && Math.Abs(Z - other.Z) < ε;
    }

    public override bool Equals(object? obj)
    {
        return obj is Point other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public static bool operator ==(Point left, Point right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Point left, Point right)
    {
        return !(left == right);
    }
}