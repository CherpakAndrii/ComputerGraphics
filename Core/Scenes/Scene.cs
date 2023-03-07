﻿using Structures.Interfaces;
using Core.Cameras;
using Core.Lights;

namespace Core.Scenes;

public class Scene
{
    public List<ILightSource> LightSources { get; } = new();
    public List<IIntersectable> Figures { get; } = new();
    public required Camera Camera { get; set; }
}