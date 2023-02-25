using Structures.BaseGeometricalStructures;

namespace StructTest;

public class PointTests
{
    [Test]
    public void PointPlusVectorTest()
    {
        Point point = new Point(1, 1, 1);
        Vector vector = new Vector(2, 3, 5);

        Point result = point + vector;
        
        Assert.That(result, Is.EqualTo(new Point(3, 4, 6)));
    }
    
    [Test]
    public void PointMinusVectorTest()
    {
        Point point = new Point(1, 1, 1);
        Vector vector = new Vector(2, 3, 5);

        Point result = point - vector;
        
        Assert.That(result, Is.EqualTo(new Point(-1, -2, -4)));
    }
    
    [Test]
    public void DistanceBetweenPointsTest()
    {
        Point point1 = new Point(1, 1, 1);
        Point point2 = new Point(5, 3, 4);

        float result = point1.GetDistance(point2);
        
        Assert.That(result, Is.EqualTo((float)Math.Sqrt(29)));
    }
}