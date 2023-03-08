using Core.Cameras;
using Core.Scenes;
using Core.Calculators;
using Structures.IntersectableFigures;
using Structures.BaseGeometricalStructures;

namespace Core.Tests;

public class IntersectionCalculatorTests
{
    private Camera camera = new()
    {
        ProjectionPlaneHeightInPixels = 60,
        ProjectionPlaneWidthInPixels = 60,
        DistanceToProjectionPlane = 1,
        FieldOfView = 90,
        Direction = new (1, 0, 0),
        Position = new (0, 0, 0)
    };

    [Fact]
    public void FindClosestIntersection_EmptyScene_ReturnsFalse()
    {
        Scene scene = new() { Camera = camera };
        Ray cameraRay = new(camera.Position, camera.Direction);

        bool result = IntersectionCalculator.FindClosestIntersection(scene, cameraRay, out var _, out var figure);

        Assert.Multiple(() =>
        {
            Assert.False(result);
            Assert.Null(figure);
        });
    }

    [Fact]
    public void FindClosestIntersection_NoIntersection_ReturnsFalse()
    {
        Scene scene = new() { Camera = camera };
        Ray cameraRay = new(camera.Position, camera.Direction);
        Sphere sphere = new(new(100, 100, 100), 10);
        scene.Figures.Add(sphere);

        bool result = IntersectionCalculator.FindClosestIntersection(scene, cameraRay, out var _, out var figure);

        Assert.Multiple(() =>
        {
            Assert.Null(figure);
            Assert.False(result);
        });
    }

    [Fact]
    public void FindClosestIntersection_Intersection_ReturnsTrue()
    {
        Scene scene = new() { Camera = camera };
        Ray cameraRay = new(camera.Position, camera.Direction);
        Sphere sphere = new(new(5, 0, 0), 4);
        scene.Figures.Add(sphere);

        bool result = IntersectionCalculator.FindClosestIntersection(scene, cameraRay, out var _, out var figure);

        Assert.Multiple(() =>
        {
            Assert.Equal(sphere, figure);
            Assert.True(result);
        });
    }
}