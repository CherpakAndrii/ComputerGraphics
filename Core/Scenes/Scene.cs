using Structures.Interfaces;

namespace Core.Scenes;

public class Scene
{
    public List<ILightSource> LightSources { get; } = new();
    public List<IIntersectable> Figures { get; } = new();
    public required Cameras.Camera Camera { get; set; }
}