using Structures.BaseGeometricalStructures;

namespace Core.Scenes;

public class SceneTransformator
{
    private double[,] TransformationMatrix = new double[4, 4]
    {
        { 1, 0, 0, 0 },
        { 0, 1, 0, 0 },
        { 0, 0, 1, 0 },
        { 0, 0, 0, 1 }
    };

    public double AngleX { get; private set; } = 0;
    public double SinX { get; private set; } = 0;
    public double CosX { get; private set; } = 1;

    public double AngleY { get; private set; } = 0;
    public double SinY { get; private set; } = 0;
    public double CosY { get; private set; } = 1;

    public double AngleZ { get; private set; } = 0;
    public double SinZ { get; private set; } = 0;
    public double CosZ { get; private set; } = 1;

    public double ShiftX { get; private set; } = 0;
    public double ShiftY { get; private set; } = 0;
    public double ShiftZ { get; private set; } = 0;

    public double ScaleX { get; private set; } = 1;
    public double ScaleY { get; private set; } = 1;
    public double ScaleZ { get; private set; } = 1;

    public void RotateX(double angle) => RotateX(angle, true);
    private void RotateX(double angle, bool update)
    {
        AngleX += angle;
        SinX = Math.Sin(AngleX);
        CosX = Math.Cos(AngleX);
        if (update) UpdateTransformationMatrix();
    }

    public void RotateY(double angle) => RotateY(angle, true);
    private void RotateY(double angle, bool update)
    {
        AngleX += angle;
        SinX = Math.Sin(AngleX);
        CosX = Math.Cos(AngleX);
        if (update) UpdateTransformationMatrix();
    }

    public void RotateZ(double angle) => RotateZ(angle, true);
    private void RotateZ(double angle, bool update)
    {
        AngleX += angle;
        SinX = Math.Sin(AngleX);
        CosX = Math.Cos(AngleX);
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
        TransformationMatrix = new double[4, 4]
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
        RotateX(-AngleX, false);
        RotateY(-AngleY, false);
        RotateZ(-AngleZ, true);
    }
}
