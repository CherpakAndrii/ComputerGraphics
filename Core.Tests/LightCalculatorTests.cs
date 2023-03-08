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

    private Scene scene;
    private Ray cameraRay;
    private Color pixel = new();
    private Sphere sphere = new(new(10, 0, 0), 5);

    public LightCalculatorTests()
    {
        scene = new() { Camera = camera };
        LightPoint lightPoint = new(new(0, 0, 0), new(255, 255, 255));
        scene.Figures.Add(sphere);
        scene.LightSources.Add(lightPoint);
        cameraRay = new(camera.Position, camera.Direction);
    }

    [Fact]
    public void CalculateLight_SphereShadow_ReturnDarkPixel()
    {
        Plane plane = new(new(1, 0, 0), new(1, 0, 0));
        scene.Figures.Add(plane);

        Point intersection = (Point)sphere.GetIntersectionWith(cameraRay)!;
        LightCalculator.CalculateLight(scene, ref pixel, intersection, sphere);

        Assert.Equal(pixel, new(0, 0, 0));
    }

    [Fact]
    public void CalculateLight_SphereShadow_ReturnWhitePixel()
    {
        Point intersection = (Point)sphere.GetIntersectionWith(cameraRay)!;
        LightCalculator.CalculateLight(scene, ref pixel, intersection, sphere);

        Assert.Equal(pixel, new(255, 255, 255));
    }
}
