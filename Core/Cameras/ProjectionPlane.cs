using Structures.BaseGeometricalStructures;

namespace Core.Cameras;

public struct ProjectionPlane
{
    public int Width { get; }
    
    public int Height { get; }
    
    public Camera Camera { get; }
    
    public Point[,] Matrix { get; }

    public ProjectionPlane(Camera camera, int width, int height)
    {
        Width = width;
        Height = height;
        Camera = camera;
        Matrix = Create();
    }
    
    private Point[,] Create()
    {
        var (rightProjectionPlaneDirection, upProjectionPlaneDirection) = GetProjectionPlaneDirections();

        var projectionPlane = new Point[Width, Height];
        float projectionPlaneAspectRatio
            = (float)Width / Height;

        float alpha = Camera.FieldOfView / 2;
            
        float leftProjectionPlaneOffset = (float)Math.Tan((Math.PI / 180) * alpha) * Camera.DistanceToProjectionPlane;
        float bottomProjectionPlaneOffset = leftProjectionPlaneOffset * projectionPlaneAspectRatio;
            
        float horizontalDistanceBetweenProjectionPixels
            = leftProjectionPlaneOffset / Width * 2;
        float verticalDistanceBetweenProjectionPixels
            = bottomProjectionPlaneOffset / Height * 2;


        var leftBottomCornerOfProjectionPlane =
            Camera.Position
            - rightProjectionPlaneDirection * leftProjectionPlaneOffset
            - upProjectionPlaneDirection * bottomProjectionPlaneOffset;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                projectionPlane[x, y] =
                    leftBottomCornerOfProjectionPlane
                    + Camera.Direction * Camera.DistanceToProjectionPlane
                    + rightProjectionPlaneDirection * x * horizontalDistanceBetweenProjectionPixels
                    + upProjectionPlaneDirection * y * verticalDistanceBetweenProjectionPixels;
            }
        }
        return projectionPlane;
    }

    private (Vector rightDirection, Vector upDirection) GetProjectionPlaneDirections()
    {
        var rightDirection = new Vector(0, 0, 1).CrossProductWith(Camera.Direction);
        if (rightDirection == Vector.Zero)
        {
            rightDirection
                = new Vector(0, 1, 0).CrossProductWith(Camera.Direction);
        }
        var upDirection = Camera.Direction.CrossProductWith(rightDirection);
        return (rightDirection, upDirection);
    }
}