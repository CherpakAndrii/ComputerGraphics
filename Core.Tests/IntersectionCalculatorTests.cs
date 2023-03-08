using Core.Cameras;
using Core.Scenes;
using Core.Calculators;
using Structures.IntersectableFigures;
using Structures.BaseGeometricalStructures;

namespace Core.Tests;

public class IntersectionCalculatorTests
{
    private readonly Camera _camera = new
    (
        new Point(0, 0, 0),
        new Vector(1, 0, 0),
        90,
        1
    );

    [Fact]
    public void FindClosestIntersection_EmptyScene_ReturnsFalse()
    {
        Scene scene = new() { ProjectionPlane = new ProjectionPlane(_camera, 60, 60)};
        Ray cameraRay = new(_camera.Position, _camera.Direction);

        bool result = IntersectionCalculator.FindClosestIntersection(scene, cameraRay, out _, out var figure);

        Assert.Multiple(() =>
        {
            Assert.False(result);
            Assert.Null(figure);
        });
    }

    [Fact]
    public void FindClosestIntersection_NoIntersection_ReturnsFalse()
    {
        Scene scene = new() { ProjectionPlane = new ProjectionPlane(_camera, 60, 60)};
        Ray cameraRay = new(_camera.Position, _camera.Direction);
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
        Scene scene = new() { ProjectionPlane = new ProjectionPlane(_camera, 60, 60)};
        Ray cameraRay = new(_camera.Position, _camera.Direction);
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