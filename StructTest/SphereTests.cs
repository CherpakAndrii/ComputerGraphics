using Structures.BaseGeometricalStructures;
using Structures.IntersectableFigures;

namespace StructTest;

public class SphereTests
{
    [Test]
    public void Sphere_WhenCallNormalMethod_NormalVectorReceived()
    {
        Sphere sphere = new(new Point(0, 0, 0), 1);
        Vector expectedNormal = new(0, 0, 1);
        Point pointForNormal = new(0, 0, 1);

        var actualNormal = sphere.GetNormalVector(pointForNormal);
        
        Assert.That(actualNormal, Is.EqualTo(expectedNormal));
    }
    
    [Test]
    public void Sphere_WhenIntersectsWithRay_CorrectIntersectionPointReturned()
    {
        Sphere sphere = new(new Point(0, 0, 0), 1);
        Ray ray = new(new Point(0, 0, 5), new Vector(0, 0, -1));
        Point expectedIntersectionPoint = new(0, 0, 1);

        var hasIntersection = sphere.HasIntersectionWith(ray, out var actualIntersectionPoint);

        Assert.Multiple(() =>
        {
            Assert.That(hasIntersection, Is.True);
            Assert.That(actualIntersectionPoint, Is.EqualTo(expectedIntersectionPoint));
        });
    }
    
    [Test]
    public void Sphere_WhenRayInOppositeDirection_NoIntersection()
    {
        Sphere sphere = new(new Point(0, 0, 0), 1);
        Ray ray = new(new Point(0, 0, 5), new Vector(0, 0, 1));

        var hasIntersection = sphere.HasIntersectionWith(ray, out _);
        
        Assert.That(hasIntersection, Is.False);
    }
}