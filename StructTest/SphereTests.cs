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
}