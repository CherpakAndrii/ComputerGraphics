using Structures;
using Structures.Interfaces;

namespace Core.Scene;

public class Scene
{
    public List<ILightSource> LightSources { get; } = new();
    public List<IIntersectable> Figures { get; } = new();
    public required Camera Camera { get; set; }
}