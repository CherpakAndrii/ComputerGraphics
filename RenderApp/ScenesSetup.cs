using Core.Cameras;
using Core.Lights;
using Core.Scenes;
using Structures.BaseGeometricalStructures;
using Structures.IntersectableFigures;

namespace RenderApp;

public class ScenesSetup
{
    public Scene CheburashkaScene()
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
}