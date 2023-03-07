﻿using Structures.BaseGeometricalStructures;
using Structures.Interfaces;

namespace Structures.IntersectableFigures;

public class Plane : IIntersectable
{
    private Point Point { get; set; }
    private Vector Normal { get; set; }
    
    public Vector GetNormalVector(Point point)
    {
        return Normal.Normalized();
    }

    public Point? GetIntersectionWith(Ray ray)
    {
        throw new NotImplementedException();
    }

    public bool HasIntersectionWith(Ray ray)
    {
        return GetIntersectionWith(ray) is not null;
    }
}