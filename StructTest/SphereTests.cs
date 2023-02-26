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
    
    [TestCaseSource(nameof(IntersectionCases))]
    public void Sphere_WhenIntersectsWithRay_CorrectIntersectionPointReturned
    (
        Sphere sphere,
        Ray ray,
        Point expectedIntersectionPoint)
    {
         var hasIntersection = sphere.HasIntersectionWith(ray, out var actualIntersectionPoint);

        Assert.Multiple(() =>
        {
            Assert.That(hasIntersection, Is.True);
            Assert.That(actualIntersectionPoint, Is.EqualTo(expectedIntersectionPoint));
        });
    }
    
    [TestCaseSource(nameof(NoIntersectionCases))]
    public void Sphere_WhenRayInOppositeDirection_NoIntersection(Sphere sphere, Ray ray)
    {
        var hasIntersection = sphere.HasIntersectionWith(ray, out _);
        
        Assert.That(hasIntersection, Is.False);
    }
    
    public static object[] IntersectionCases =
    {
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, 0, 5), new Vector(0, 0, -1)),
            new Point(0, 0, 1)
        },
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, 0, -5), new Vector(0, 0, 1)),
            new Point(0, 0, -1)
        },
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, 5, 0), new Vector(0, -1, 0)),
            new Point(0, 1, 0)
        }
    };
    
    public static object[] NoIntersectionCases =
    {
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, 0, 5), new Vector(0, 0, 1))
        },
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, 0, -5), new Vector(0, 0, -1)),
        },
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, -5, 0), new Vector(0, 1, 1))
        }
    };
}