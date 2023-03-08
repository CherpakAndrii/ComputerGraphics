using Core.Scenes;
using Core.Lights;
using Core.Cameras;
using Core.Calculators;
using Structures.IntersectableFigures;
using Structures.BaseGeometricalStructures;

namespace Core.Tests;

public class LightCalculatorTests
{
    private Camera camera = new()
    {
        ProjectionPlaneHeightInPixels = 60,
        ProjectionPlaneWidthInPixels = 60,
        DistanceToProjectionPlane = 1,
        FieldOfView = 90,
        Direction = new(1, 0, 0),
        Position = new(0, 0, 0)
    };

    [Fact]
    public void CalculateLight_SphereShadow_ReturnDarkPixel()
    {
        Scene scene = new() { Camera = camera };
        Sphere sphere = new(new(10, 0, 0), 5);
        Plane plane = new(new(1, 0, 0), new(1, 0, 0));
        LightPoint lightPoint = new(new(0, 0, 0), new(255, 255, 255));
        scene.LightSources.Add(lightPoint);
        scene.Figures.Add(sphere);
        scene.Figures.Add(plane);
        Ray cameraRay = new(camera.Position, camera.Direction);
        Color pixel = new();

        Point intersection = (Point)sphere.GetIntersectionWith(cameraRay)!;
        LightCalculator.CalculateLight(scene, ref pixel, intersection, sphere);

        Assert.Equal(pixel, new(0, 0, 0));
    }

    [Fact]
    public void CalculateLight_SphereShadow_ReturnWhitePixel()
    {
        Scene scene = new() { Camera = camera };
        Sphere sphere = new(new(10, 0, 0), 5);
        LightPoint lightPoint = new(new(0, 0, 0), new(255, 255, 255));
        scene.LightSources.Add(lightPoint);
        scene.Figures.Add(sphere);
        Ray cameraRay = new(camera.Position, camera.Direction);
        Color pixel = new();

        Point intersection = (Point)sphere.GetIntersectionWith(cameraRay)!;
        LightCalculator.CalculateLight(scene, ref pixel, intersection, sphere);

        Assert.Equal(pixel, new(255, 255, 255));
    }
}
