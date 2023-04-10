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

    public void RotateX(double angle, bool update = true)
    {
        AngleX += angle;
        SinX = Math.Sin(AngleX);
        CosX = Math.Cos(AngleX);
        if (update) UpdateTransformationMatrix();
    }

    public void RotateY(double angle, bool update = true)
    {
        AngleX += angle;
        SinX = Math.Sin(AngleX);
        CosX = Math.Cos(AngleX);
        if (update) UpdateTransformationMatrix();
    }

    public void RotateZ(double angle, bool update = true)
    {
        AngleX += angle;
        SinX = Math.Sin(AngleX);
        CosX = Math.Cos(AngleX);
        if (update) UpdateTransformationMatrix();
    }

    public void Rotate(Vector angles, bool update = true)
    {
        RotateX(angles.X, false);
        RotateY(angles.Y, false);
        RotateZ(angles.Z, update);
    }

    public void MoveX(double directionX, bool update = true)
    {
        ShiftX += directionX;
        if (update) UpdateTransformationMatrix();
    }

    public void MoveY(double directionY, bool update = true)
    {
        ShiftY += directionY;
        if (update) UpdateTransformationMatrix();
    }

    public void MoveZ(double directionZ, bool update = true)
    {
        ShiftZ += directionZ;
        if (update) UpdateTransformationMatrix();
    }

    public void Move(Vector direction, bool update = true)
    {
        ShiftX += direction.X;
        ShiftY += direction.Y;
        ShiftZ += direction.Z;
        if (update) UpdateTransformationMatrix();
    }

    public void ToScaleX(double scaleX, bool update = true)
    {
        ScaleX *= scaleX;
        if (update) UpdateTransformationMatrix();
    }

    public void ToScaleY(double scaleY, bool update = true)
    {
        ScaleY *= scaleY;
        if (update) UpdateTransformationMatrix();
    }

    public void ToScaleZ(double scaleZ, bool update = true)
    {
        ScaleZ *= scaleZ;
        if (update) UpdateTransformationMatrix();
    }

    public void ToScale(Vector scale, bool update = true)
    {
        ScaleX *= scale.X;
        ScaleY *= scale.Y;
        ScaleZ *= scale.Z;
        if (update) UpdateTransformationMatrix();
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
        RotateX(-AngleX, false);
        RotateY(-AngleY, false);
        RotateZ(-AngleZ, false);
        ShiftX = 0;
        ShiftY = 0;
        ShiftZ = 0;
        ScaleX = 1;
        ScaleY = 1;
        ScaleY = 1;
        UpdateTransformationMatrix();
    }
}
