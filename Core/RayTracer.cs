using Core.Lights;
using Core.Scenes;
using Core.Calculators;
using Structures.BaseGeometricalStructures;

namespace Core;

public class RayTracer
{
    public Scene Scene { get; }

    private IRenderOutput _renderOutput;

    public RayTracer(Scene scene, IRenderOutput renderOutput)
    {
        Scene = scene;
        _renderOutput = renderOutput;
    }

    public void TraceRays()
    {
        Point[,] projectionPlane = Scene.ProjectionPlane.Matrix;
        Color[,] pixels = new Color[projectionPlane.GetLength(0), projectionPlane.GetLength(1)];
        for (int i = 0; i < projectionPlane.GetLength(0); i++)
        {
            for (int j = 0; j < projectionPlane.GetLength(1); j++)
            {
                pixels[i, j] = new Color();
                Ray ray = new(Scene.ProjectionPlane.Camera.Position, projectionPlane[i, j]);
                if (IntersectionCalculator.FindClosestIntersection(Scene, ray, out var intersectionPoint, out var figure))
                    LightCalculator.CalculateLight(Scene, ref pixels[i, j], intersectionPoint, figure);
            }
        }
        
        _renderOutput.CreateRenderResult(pixels);
    }
}