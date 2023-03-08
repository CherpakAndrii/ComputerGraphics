using Core.Cameras;
using Core.Lights;
using Core.Scenes;
using Structures.BaseGeometricalStructures;
using Structures.IntersectableFigures;

namespace RenderApp;

public static class ScenesSetup
{
    public static Scene CheburashkaScene()
    {
        Camera camera = new
        (
            new Point(0, 0, 0),
            new Vector(1, 0, 0),
            30,
            1
        );
        
        var projectionPlane = new ProjectionPlane(camera, 80, 80);

        Sphere sphere = new(new(6, -0.2f, -1f), 0.5f);
        Sphere sphere2 = new(new(6, -0.2f, 1f), 0.5f);
        Sphere sphere3 = new(new(6, 0, 0), 1f);
        Sphere sphere4 = new(new(5, -0.2f, -0.3f), 0.15f);
        Sphere sphere5 = new(new(5, -0.2f, 0.3f), 0.15f);

        LightPoint lightPoint = new(new(0, 0, 0), new(139, 69, 19));
        LightPoint lightPoint2 = new(new(-5, 0, 5), new(123, 45, 125));

        Scene scene = new() { ProjectionPlane = projectionPlane };
        scene.LightSources.Add(lightPoint);
        scene.LightSources.Add(lightPoint2);

        scene.Figures.Add(sphere);
        scene.Figures.Add(sphere2);
        scene.Figures.Add(sphere3);
        scene.Figures.Add(sphere4);
        scene.Figures.Add(sphere5);

        return scene;
    }

    public static Scene SceneWithAllFigures()
    {
        Camera camera = new
        (
            new Point(0, 0, 0),
            new Vector(1, 0, 0),
            90,
            1
        );

        var projectionPlane = new ProjectionPlane(camera, 60, 60);

        Sphere sphere = new(new Point(7, 0, 0), 4);
        var plane = new Plane(new Point(10, 10, 0), new Vector(-1, -1, 0));
        var disk = new Disk(new Point(2, 2, 2), new Vector(-1, 0, 0), 1);

        LightPoint lightPoint = new(new Point(0, 0, 0), new Color(0, 255, 255));
        LightPoint lightPoint2 = new(new Point(-3, 5, 5), new Color(255, 0, 255));

        Scene scene = new() { ProjectionPlane = projectionPlane };
        scene.LightSources.Add(lightPoint);
        scene.LightSources.Add(lightPoint2);
        scene.Figures.Add(sphere);
        scene.Figures.Add(plane);
        scene.Figures.Add(disk);

        return scene;
    }

    public static Scene SceneSphereAndDisk()
    {
        Camera camera = new
        (
            new Point(0, 0, 0),
            new Vector(1, 0, 0),
            90,
            1
        );

        var projectionPlane = new ProjectionPlane(camera, 90, 90);

        Sphere sphere = new(new Point(15, -2, 0), 4);
        var disk = new Disk(new Point(15, 3, 0), new Vector(0, -1, 0), 10);

        LightPoint lightPoint = new(new Point(15, -100, 0), new Color(150, 0, 150));
        LightPoint lightPoint2 = new(new Point(15, -30, -100), new Color(0, 150, 150));
        LightPoint lightPoint3 = new(new Point(15, -30, 100), new Color(150, 150, 0));

        Scene scene = new() { ProjectionPlane = projectionPlane };
        scene.LightSources.Add(lightPoint);
        scene.LightSources.Add(lightPoint2);
        scene.LightSources.Add(lightPoint3);
        scene.Figures.Add(sphere);
        scene.Figures.Add(disk);

        return scene;
    }

    public static Scene PlaneAndSphere()
    {
        Camera camera = new
        (
            new Point(0, 0, 0),
            new Vector(0, 1, 0),
            90,
            1
        );

        var projectionPlane = new ProjectionPlane(camera, 90, 90);

        Plane plane = new(new(0, 0, 5), new(0, 0, -1));
        Sphere sphere = new(new(0, 10, -4), 5);

        LightPoint lightPoint = new(new Point(-10, -10, -10), new Color(255, 255, 255));

        Scene scene = new() { ProjectionPlane = projectionPlane };
        scene.LightSources.Add(lightPoint);
        scene.Figures.Add(plane);
        scene.Figures.Add(sphere);

        return scene;
    }
}