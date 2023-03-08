using Structures.BaseGeometricalStructures;

namespace Core.Cameras;

public struct Camera
{
    public Camera
    (
        Point position,
        Vector direction,
        float fieldOfView,
        float distanceToProjectionPlane
    )
    {
        Position = position;
        Direction = direction.Normalized();

        if (fieldOfView is < 0 or > 180)
        {
            throw new Exception("Field of view has wrong value");
        }
        FieldOfView = fieldOfView;
        
        if (distanceToProjectionPlane < 0)
            throw new Exception("Distance cannot be negative");
        DistanceToProjectionPlane = distanceToProjectionPlane;
    }
    public Point Position { get; }
    public Vector Direction { get; }

    public float DistanceToProjectionPlane { get; }
    public float FieldOfView { get; }
}