namespace StructTest;

public class RayTests
{
    [Test]
    public void NearestPointOnRayToAnotherPointTest()
    {
        var ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 15));
        var point = new Point(197, 25, 55);

        var nearest = ray.GetNearestPointTo(point);
        
        Assert.That(nearest, Is.EqualTo(new Point(0, 0, 55)));
    }
    
    [Test]
    public void NearestPointOnRayToAnotherPointTest__StartPoint()
    {
        var ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, -15));
        var point = new Point(197, 25, 55);

        var nearest = ray.GetNearestPointTo(point);
        
        Assert.That(nearest, Is.EqualTo(new Point(0, 0, 0)));
    }
}