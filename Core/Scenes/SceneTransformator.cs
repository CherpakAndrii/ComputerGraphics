using Structures.BaseGeometricalStructures;
using Structures.IntersectableFigures;

namespace Core.Scenes;

public class SceneTransformator
{
    private double[,] _transformationMatrix = new double[4, 4]
    {
        { 1, 0, 0, 0 },
        { 0, 1, 0, 0 },
        { 0, 0, 1, 0 },
        { 0, 0, 0, 1 }
    };

    public double AngleRadX { get; private set; } = 0;
    public double AngleDegreeX { get { return AngleRadX * 180.0 / Math.PI; } }
    public double SinX { get; private set; } = 0;
    public double CosX { get; private set; } = 1;

    public double AngleRadY { get; private set; } = 0;
    public double AngleDegreeY { get { return AngleRadY * 180.0 / Math.PI; } }
    public double SinY { get; private set; } = 0;
    public double CosY { get; private set; } = 1;

    public double AngleRadZ { get; private set; } = 0;
    public double AngleDegreeZ { get { return AngleRadZ * 180.0 / Math.PI; } }
    public double SinZ { get; private set; } = 0;
    public double CosZ { get; private set; } = 1;

    public double ShiftX { get; private set; } = 0;
    public double ShiftY { get; private set; } = 0;
    public double ShiftZ { get; private set; } = 0;

    public double ScaleX { get; private set; } = 1;
    public double ScaleY { get; private set; } = 1;
    public double ScaleZ { get; private set; } = 1;

    public void RotateDegreeX(double angleDegree) => RotateX(angleDegree * Math.PI / 180.0, true);
    public void RotateRadX(double angleRad) => RotateX(angleRad, true);
    private void RotateX(double angleRad, bool update)
    {
        AngleRadX += angleRad;
        SinX = Math.Sin(AngleRadX);
        CosX = Math.Cos(AngleRadX);
        if (update) UpdateTransformationMatrix();
    }

    public void RotateDegreeY(double angleDegree) => RotateY(angleDegree * Math.PI / 180.0, true);
    public void RotateRadY(double angleRad) => RotateY(angleRad, true);
    private void RotateY(double angleRad, bool update)
    {
        AngleRadY += angleRad;
        SinY = Math.Sin(AngleRadY);
        CosY = Math.Cos(AngleRadY);
        if (update) UpdateTransformationMatrix();
    }

    public void RotateDegreeZ(double angleDegree) => RotateZ(angleDegree * Math.PI / 180.0, true);
    public void RotateRadZ(double angleRad) => RotateZ(angleRad, true);
    private void RotateZ(double angleRad, bool update)
    {
        AngleRadZ += angleRad;
        SinZ = Math.Sin(AngleRadZ);
        CosZ = Math.Cos(AngleRadZ);
        if (update) UpdateTransformationMatrix();
    }

    public void Rotate(Vector angles)
    {
        RotateX(angles.X, false);
        RotateY(angles.Y, false);
        RotateZ(angles.Z, true);
    }

    public void MoveX(double directionX)
    {
        ShiftX += directionX;
        UpdateTransformationMatrix();
    }

    public void MoveY(double directionY)
    {
        ShiftY += directionY;
        UpdateTransformationMatrix();
    }

    public void MoveZ(double directionZ)
    {
        ShiftZ += directionZ;
        UpdateTransformationMatrix();
    }

    public void Move(Vector direction)
    {
        ShiftX += direction.X;
        ShiftY += direction.Y;
        ShiftZ += direction.Z;
        UpdateTransformationMatrix();
    }

    public void ToScaleX(double scaleX)
    {
        ScaleX *= scaleX;
        UpdateTransformationMatrix();
    }

    public void ToScaleY(double scaleY)
    {
        ScaleY *= scaleY;
        UpdateTransformationMatrix();
    }

    public void ToScaleZ(double scaleZ)
    {
        ScaleZ *= scaleZ;
        UpdateTransformationMatrix();
    }

    public void ToScale(Vector scale)
    {
        ScaleX *= scale.X;
        ScaleY *= scale.Y;
        ScaleZ *= scale.Z;
        UpdateTransformationMatrix();
    }

    private void UpdateTransformationMatrix()
    {
        _transformationMatrix = new double[4, 4]
        {
            { ScaleX * CosY * CosZ, -SinZ * CosY, SinY, ShiftX  },
            { SinX * SinY * CosZ + SinZ * CosX, ScaleY * (-SinX * SinY * SinZ + CosX * CosZ), -SinX * CosY, ShiftY },
            { SinX * SinZ - SinY * CosX * CosZ, SinX * CosZ + SinY * SinZ * CosX, ScaleZ * CosX * CosY, ShiftZ },
            { 0, 0, 0, 1 }
        };
    }

    public void ResetTransformation()
    {
        ShiftX = 0;
        ShiftY = 0;
        ShiftZ = 0;
        ScaleX = 1;
        ScaleY = 1;
        ScaleY = 1;
        RotateX(-AngleRadX, false);
        RotateY(-AngleRadY, false);
        RotateZ(-AngleRadZ, true);
    }

    public Point Apply(Point point)
    {
        float[] pointArray = { point.X, point.Y, point.Z, 1 };
        float[] transformedPoint = { 0, 0, 0, 0 };
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                transformedPoint[i] +=(float)_transformationMatrix[i, j] * pointArray[j];
            }
        }

        return new Point
        (
            transformedPoint[0],
            transformedPoint[1],
            transformedPoint[2]
        );
    }

    public Triangle Apply(Triangle triangle)
    {
        return new Triangle
        (
            Apply(triangle.A),
            Apply(triangle.B),
            Apply(triangle.C)
        );
    }
    
    public Vector Apply(Vector vector)
    {
        float[] vectorArray = { vector.X, vector.Y, vector.Z };
        float[] transformedVector = { 0, 0, 0 };
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                transformedVector[i] +=(float)_transformationMatrix[i, j] * vectorArray[j];
            }
        }

        return new Vector
        (
            transformedVector[0],
            transformedVector[1],
            transformedVector[2]
        );
    }
}
