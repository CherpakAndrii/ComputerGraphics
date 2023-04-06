using NUnit.Framework;

namespace StructTest;

[TestFixture]
public class TriangleTests
{
    [TestCaseSource(nameof(_intersectionCases))]
    public void Triangle_WhenIntersectsWithRay_CorrectIntersectionPointReturned
    (
        Triangle triangle,
        Ray ray,
        Point expectedIntersectionPoint
    )
    {
        var actualIntersectionPoint = triangle.GetIntersectionWith(ray);

        Assert.Multiple(() =>
        {
            Assert.That(actualIntersectionPoint is not null, Is.True);
            Assert.That(actualIntersectionPoint, Is.EqualTo(expectedIntersectionPoint));
        });
    }
    
    [TestCaseSource(nameof(_intersectionCases))]
    public void Triangle_WhenIntersectsWithRay_TrueReturned
    (
        Triangle triangle,
        Ray ray,
        Point expectedIntersectionPoint
    )
    {
        var hasIntersection = triangle.HasIntersectionWith(ray);

        Assert.That(hasIntersection, Is.True);
    }
    
    [TestCaseSource(nameof(_noIntersectionCases))]
    public void Triangle_WhenNoIntersectionWithRay_NullReturned(Triangle triangle, Ray ray)
    {
        var intersection = triangle.GetIntersectionWith(ray);

        Assert.That(intersection, Is.Null);
    }
    
    [TestCaseSource(nameof(_ambiguousCases))]
    public void CheckAmbiguousCases_WhenRayIsParallelToTriangle(Triangle triangle, Ray ray)
    {
        var intersection = triangle.GetIntersectionWith(ray);

        Assert.That(intersection, Is.Null);
    }
    
    private static object[] _intersectionCases =
    {
        new object[]
        {
            new Triangle(new (4, 6, 2), new (2, 6, 2), new (2, 3, 2)),
            new Ray(new (3, 5, 7), new Vector(0, 0, -1)),
            new Point(3, 5, 2)
        },
        new object[]
        {
            new Triangle(new (3, -1, 8), new (1, -1, 8), new (1, -1, 6)),
            new Ray(new (2, 4, 7), new Vector(0, -1, 0)),
            new Point(2, -1, 7)
        },
        new object[]
        {
            new Triangle(new (-5.43f, 1.56f, 3), new (-4.88f,3.5f,-1), new (-2.55f,0.58f,0)),
            new Ray(new (-1.2f,3.25f,-1.5f), new Vector(-6.82f, -2.7f, 5.3f)),
            new Point(-4.61f, 1.9f, 1.15f)
        },
        new object[]
        {
            new Triangle(new (0, 0, 0), new (0, 1, 0), new (1, 0, 0)),
            new Ray(new (-1, -1, 1), new Vector(0.5f, 0.5f, -0.5f)),
            new Point(0, 0, 0)
        },
        new object[]
        {
            new Triangle(new (0, 0, 0), new (0, 1, 0), new (1, 0, 0)),
            new Ray(new (0.5f, 0.5f, 1), new Vector(0, 0, -1)),
            new Point(0.5f, 0.5f, 0)
        },
        new object[]
        {
            new Triangle(new (-1, -1, 0), new (1, -1, 0), new (0, 1, 0)),
            new Ray(new (0, 0, 1), new Vector(0, 0, -1)),
            new Point(0, 0, 0)
        },
        new object[]
        {
            new Triangle(new (11, 11, 5), new (5, 11, 11), new (11, 5, 11)),
            new Ray(new (4, 4, 4), new Vector(1, 1, 1)),
            new Point(9, 9, 9)
        },
        new object[]
        {
            new Triangle(new (0, 0, 0), new (0, 1, 0), new (1, 0, 0)),
            new Ray(new (1, -1, -1), new Vector(-1, 1, 1)),
            new Point(0, 0, 0)
        }
    };
    
    private static object[] _noIntersectionCases =
    {
        new object[]
        {
            new Triangle(new (-1, 1, 0), new (1, 1, 0), new (1, -1, 0)),
            new Ray(new (-1, -1, 1), new Vector(0, 0, -1))
        },
        new object[]
        {
            new Triangle(new (-1, 1, 0), new (1, 1, 0), new (1, -1, 0)),
            new Ray(new (-1, 1.1f, 0.1f), new Vector(1, 0, -0.1f))
        },
        new object[]
        {
            new Triangle(new (11, 11, 5), new (5, 11, 11), new (11, 5, 11)),
            new Ray(new (4, 4, 4), new Vector(-1, -1, -1))
        }
    };

    private static object[] _ambiguousCases =
    {
        new object[]
        {   // the ray lies on the side of the triangle
            new Triangle(new (-1, 1, 0), new (1, 1, 0), new (1, -1, 0)),
            new Ray(new (-3, 1, 0), new Vector(1, 0, 0))
        },
        new object[]
        {   // the ray lies on the same plane as the triangle and intersects it
            new Triangle(new (-1, 1, 0), new (1, 1, 0), new (1, -1, 0)),
            new Ray(new (-3, 1, 0), new Vector(1, -0.01f, 0))
        },
        new object[]
        {   // the ray lies on the same plane as the triangle and intersects it at one of its vertices
            new Triangle(new (-1, 1, 0), new (1, 1, 0), new (1, -1, 0)),
            new Ray(new (0, 2, 0), new Vector(1, -1, 0))
        }
    };
}