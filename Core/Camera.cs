using Structures;

namespace Core;

public class Camera
{
    public Point Position { get; set; }
    public Point ProjectionPlanePosition { get; set; }
    public float FieldOfView { get; set; }

    public Point[,] GetProjectionPlane()
    {   
        var pixelPosition = new Point();
        var projectionPlane = new Point[20, 20];
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                projectionPlane[i, j] = pixelPosition;
                pixelPosition += new Vector(5, 0, 0);
            }
            pixelPosition += new Vector(0, 5, 0);
        }

        return projectionPlane;
    }
    
}