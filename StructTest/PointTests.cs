namespace StructTest;

public class PointTests
{
    [Test]
    public void PointPlusVectorTest()
    {
        Point point = new Point(1, 1, 1);
        Vector vector = new Vector(2, 3, 5);

        Point result = point + vector;
        
        Assert.AreEqual(new Point(3, 4, 6), result);
    }
    
    [Test]
    public void PointMinusVectorTest()
    {
        Point point = new Point(1, 1, 1);
        Vector vector = new Vector(2, 3, 5);

        Point result = point - vector;
        
        Assert.AreEqual(new Point(-1, -2, -4), result);
    }
}