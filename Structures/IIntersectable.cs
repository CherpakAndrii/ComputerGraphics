namespace Structures;

public interface IIntersectable
{
    public Vector GetNormalVector(Point point);
    public bool HasIntersectionWith(Ray ray);
    public Point? GetIntersectionWith(Ray ray);
}