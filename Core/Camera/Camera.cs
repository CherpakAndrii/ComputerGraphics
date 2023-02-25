using Structures;
using Structures.BaseGeometricalStructures;

namespace Core;

public class Camera
{
    public Point Position { get; set; }
    public Vector Direction { get; set; }
    private float _fieldOfView;

    public float FieldOfView
    {
        get => _fieldOfView;
        set
        {
            if (value is < 0 or > 180)
            {
                throw new Exception("Field of view has wrong value");
            }

            _fieldOfView = value;
        }
    }
    private readonly int _xSize, _ySize;

    public Camera(int xSize, int ySize)
    {
        _xSize = xSize;
        _ySize = ySize;
    }

    public Point[,] ProjectionPlane
    {
        get
        {
            var pixelPosition = new Point();
            var projectionPlane = new Point[20, 20];
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _ySize; j++)
                {
                    projectionPlane[i, j] = pixelPosition;
                    pixelPosition += new Vector(5, 0, 0);
                }
                pixelPosition += new Vector(0, 5, 0);
            }

            return projectionPlane;
        }
    }
    
}