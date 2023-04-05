using Core.Lights;
using Core.Cameras;
using Core.Scenes.IntersectableContainers;
using Structures.Interfaces;

namespace Core.Scenes;

public class Scene
{
    public List<ILightSource> LightSources { get; } = new();
    public IIntersectableContainer Figures { get; } = new IntersectableList();
    public required ProjectionPlane ProjectionPlane { get; set; }
}