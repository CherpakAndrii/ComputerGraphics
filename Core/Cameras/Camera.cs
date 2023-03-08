using Structures.BaseGeometricalStructures;

namespace Core.Cameras;

public class Camera
{
    public Camera
    (
        Point position,
        Vector direction,
        float fieldOfView
    )
    {
        Position = position;
        Direction = direction.Normalized();

        if (fieldOfView is < 0 or > 180)
        {
            throw new Exception("Field of view has wrong value");
        }

        FieldOfView = fieldOfView;
    }
    public Point Position { get; }
    public Vector Direction { get; }

    private float _distanceToProjectionPlane;

    public float DistanceToProjectionPlane
    {
        get => _distanceToProjectionPlane;
        set
        {
            if (value < 0)
                throw new Exception("Distance cannot be negative");
            _distanceToProjectionPlane = value;
        }
    }
    public float FieldOfView { get; }
    
    public int ProjectionPlaneWidthInPixels { get; set; }
    
    public int ProjectionPlaneHeightInPixels { get; set; }

    public Point[,] ProjectionPlane
    {
        get
        {
            (
                var rightProjectionPlaneDirection,
                var upProjectionPlaneDirection
            ) = GetProjectionPlaneDirections();

            var projectionPlane = new Point[ProjectionPlaneWidthInPixels, ProjectionPlaneHeightInPixels];
            float projectionPlaneAspectRatio
                = (float)ProjectionPlaneWidthInPixels / ProjectionPlaneHeightInPixels;

            float alpha = FieldOfView / 2;
            
            float leftProjectionPlaneOffset = (float)Math.Tan((Math.PI / 180) * alpha) * _distanceToProjectionPlane;
            float bottomProjectionPlaneOffset = leftProjectionPlaneOffset * projectionPlaneAspectRatio;
            
            float horizontalDistanceBetweenProjectionPixels
                = leftProjectionPlaneOffset / ProjectionPlaneWidthInPixels * 2;
            float verticalDistanceBetweenProjectionPixels
                = bottomProjectionPlaneOffset / ProjectionPlaneHeightInPixels * 2;


            var leftBottomCornerOfProjectionPlane =
                Position
                - rightProjectionPlaneDirection * leftProjectionPlaneOffset
                - upProjectionPlaneDirection * bottomProjectionPlaneOffset;

            for (int x = 0; x < ProjectionPlaneWidthInPixels; x++)
            {
                for (int y = 0; y < ProjectionPlaneHeightInPixels; y++)
                {
                    projectionPlane[x, y] =
                        leftBottomCornerOfProjectionPlane
                        + Direction * _distanceToProjectionPlane
                        + rightProjectionPlaneDirection * x * horizontalDistanceBetweenProjectionPixels
                        + upProjectionPlaneDirection * y * verticalDistanceBetweenProjectionPixels;
                }
            }
            return projectionPlane;
        }
    }

    private (Vector rightDirection, Vector upDirection) GetProjectionPlaneDirections()
    {
        var rightDirection = new Vector(0, 0, 1).CrossProductWith(Direction);
        if (rightDirection == Vector.Zero)
        {
            rightDirection
                = new Vector(0, 1, 0).CrossProductWith(Direction);
        }
        var upDirection = Direction.CrossProductWith(rightDirection);
        return (rightDirection, upDirection);
    }
}