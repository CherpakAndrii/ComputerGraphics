namespace Structures;

public interface ISurface
{
    public Vector GetNormalVector(Point point);
    public bool HasIntersectionWith(Beam beam);
    public Point? GetIntersectionWith(Beam beam);
}