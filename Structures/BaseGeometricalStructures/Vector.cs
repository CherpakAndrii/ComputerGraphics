namespace Structures.BaseGeometricalStructures;

public struct Vector : IEquatable<Vector>
{
    public static Vector Zero => new (0, 0, 0); 
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Z { get; private set; }

    private const float Epsilon = 1e-6f;

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

    public Vector(Point start, Point end)
    {
        X = end.X - start.X;
        Y = end.Y - start.Y;
        Z = end.Z - start.Z;
    }
    
    public Vector(Vector original)
    {
        X = original.X;
        Y = original.Y;
        Z = original.Z;
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

    public static Vector operator *(Vector vector, float number)
    {
        return new Vector(vector.X * number, vector.Y * number, vector.Z * number);
    }

    public void Normalize()
    {
        float module = GetModule();
        X /= module;
        Y /= module;
        Z /= module;
    }
    
    public Vector Normalized()
    {
        Vector copy = new Vector(this);
        copy.Normalize();
        return copy;
    }

    public Vector CrossProductWith(Vector another)
    {
        float x = Y * another.Z - Z * another.Y;
        float y = Z * another.X - X * another.Z;
        float z = X * another.Y - Y * another.X;

        return new Vector(x, y, z);
    }

    public float DotProductWith(Vector another)
    {
        return X * another.X + Y * another.Y + Z * another.Z;
    }

    public bool IsCollinearTo(Vector vector)
    {
        float[] coefs = { X / vector.X, Y / vector.Y, Z / vector.Z };
        return Math.Abs(coefs[0] - coefs[1]) < Epsilon && Math.Abs(coefs[1] - coefs[2]) < Epsilon &&
               Math.Abs(coefs[2] - coefs[0]) < Epsilon;
    }
    
    public bool IsPerpendicularTo(Vector vector)
    {
        return DotProductWith(vector) == 0;
    }

    public float GetAngleWith(Vector another)
    {
        return (float)Math.Acos(DotProductWith(another) / (GetModule() * another.GetModule()));
    }

    // TODO: may not work correctly, needs to be fixed if this method is to be used.
    public bool Equals(Vector other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        // it should be more accurate, but may have a higher time complexity:
        // return Math.Abs(X - other.X) < epsilon && Math.Abs(Y - other.Y) < epsilon && Math.Abs(Z - other.Z) < epsilon;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public static bool operator ==(Vector first, Vector second)
        => first.Equals(second);

    public static bool operator !=(Vector first, Vector second)
        => !first.Equals(second);
}