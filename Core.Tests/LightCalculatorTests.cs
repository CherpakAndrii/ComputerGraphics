using Core.Scenes;
using Core.Lights;
using Core.Cameras;
using Core.Calculators;
using Structures.IntersectableFigures;
using Structures.BaseGeometricalStructures;

namespace Core.Tests;

public class LightCalculatorTests
{
    private readonly Camera _camera = new
    (
        new Point(0, 0, 0),
        new Vector(1, 0, 0),
        90,
        1
    );

    private readonly Scene _scene;
    private readonly Ray _cameraRay;
    private Color _pixel;
    private readonly Sphere _sphere = new(new Point(10, 0, 0), 5);

    public LightCalculatorTests()
    {
        _scene = new() { ProjectionPlane = new ProjectionPlane(_camera, 60, 60)};
        LightPoint lightPoint = new(new(0, 0, 0), new(255, 255, 255));
        _scene.Figures.Add(_sphere);
        _scene.LightSources.Add(lightPoint);
        _cameraRay = new Ray(_camera.Position, _camera.Direction);
    }

    [Fact]
    public void CalculateLight_SphereShadow_ReturnDarkPixel()
    {
        Plane plane = new(new Point(1, 0, 0), new Vector(1, 0, 0));
        _scene.Figures.Add(plane);

        var intersection = (Point)_sphere.GetIntersectionWith(_cameraRay)!;
        LightCalculator.CalculateLight(_scene, ref _pixel, intersection, _sphere);

        Assert.Equal(_pixel, new Color(0, 0, 0));
    }

    [Fact]
    public void CalculateLight_SphereLight_ReturnWhitePixel()
    {
        var intersection = (Point)_sphere.GetIntersectionWith(_cameraRay)!;
        LightCalculator.CalculateLight(_scene, ref _pixel, intersection, _sphere);

        Assert.Equal(_pixel, new Color(255, 255, 255));
    }
}
