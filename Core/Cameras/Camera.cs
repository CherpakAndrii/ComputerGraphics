using Structures.BaseGeometricalStructures;

namespace Core.Cameras;

public class Camera
{
    public Point Position { get; set; }
    
    private Vector _directionNormalized;
    public Vector Direction
    {
        get => _directionNormalized;
        set => _directionNormalized = value.Normalized();
    }

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
    
    public int ProjectionPlaneWidthInPixels { get; set; }
    
    public int ProjectionPlaneHeightInPixels { get; set; }

    public Point[,] ProjectionPlane
    {
        get
        {
            Vector rightProjectionPlaneDirection
                = new Vector(0, 0, 1).CrossProductWith(_directionNormalized);
            if (rightProjectionPlaneDirection == Vector.Zero)
            {
                rightProjectionPlaneDirection
                    = new Vector(0, 1, 0).CrossProductWith(_directionNormalized);
            }
            Vector upProjectionPlaneDirection =
                _directionNormalized.CrossProductWith(rightProjectionPlaneDirection);
            
            var projectionPlane = new Point[ProjectionPlaneWidthInPixels, ProjectionPlaneHeightInPixels];
            float projectionPlaneAspectRatio
                = (float)ProjectionPlaneWidthInPixels / ProjectionPlaneHeightInPixels;

            float alpha = _fieldOfView / 2;
            
            float leftProjectionPlaneOffset = (float)Math.Tan(alpha) * _distanceToProjectionPlane;
            float bottomProjectionPlaneOffset = leftProjectionPlaneOffset * projectionPlaneAspectRatio;
            
            float horizontalDistanceBetweenProjectionPixels
                = leftProjectionPlaneOffset / ProjectionPlaneWidthInPixels / 2;
            float verticalDistanceBetweenProjectionPixels
                = bottomProjectionPlaneOffset / ProjectionPlaneHeightInPixels / 2;


            Point leftBottomCornerOfProjectionPlane =
                Position
                + rightProjectionPlaneDirection * horizontalDistanceBetweenProjectionPixels
                + upProjectionPlaneDirection * verticalDistanceBetweenProjectionPixels;

            for (int x = 0; x < ProjectionPlaneWidthInPixels; x++)
            {
                for (int y = 0; y < ProjectionPlaneHeightInPixels; y++)
                {
                    projectionPlane[x, y] =
                        leftBottomCornerOfProjectionPlane
                        + rightProjectionPlaneDirection * x
                        + upProjectionPlaneDirection * y;
                }
            }
            return projectionPlane;
        }
    }
    
}