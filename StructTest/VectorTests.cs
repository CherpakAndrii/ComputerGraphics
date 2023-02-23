using Vector = Structures.Vector;

namespace StructTest;

public class VectorTests
{
    [Test]
    public void VectorPlusVectorTest()
    {
        Vector vector1 = new Vector(1, 1, 1);
        Vector vector2 = new Vector(2, 3, 5);

        Vector result = vector1 + vector2;
        
        Assert.AreEqual(new Vector(3, 4, 6), result);
    }
    
    [Test]
    public void VectorMinusVectorTest()
    {
        Vector vector1 = new Vector(1, 1, 1);
        Vector vector2 = new Vector(2, 3, 5);

        Vector result = vector1 - vector2;
        
        Assert.AreEqual(new Vector(-1, -2, -4), result);
    }
    
    [Test]
    public void VectorModuleSimpleTest()
    {
        Vector vector = new Vector(0, 3, 4);

        float module = vector.GetModule();

        Assert.AreEqual(5, module);
    }
    
    [Test]
    public void VectorModuleTest()
    {
        Vector vector = new Vector(11, 13, 19);

        float module = vector.GetModule();

        Assert.AreEqual((float)Math.Sqrt(Math.Pow(11, 2)+Math.Pow(13, 2)+Math.Pow(19, 2)), module);
    }
    
    [Test]
    public void VectorNormalizeTest()
    {
        Vector vector = new Vector(112, 132, 197);
        vector.Normalize();

        float module = vector.GetModule();

        Assert.That(1 - module < 1e-6, Is.True);
    }
    
    [Test]
    public void VectorIsCollinearTest()
    {
        Vector vector1 = new Vector(5, -7, 4);
        Vector vector2 = new Vector(-7.5f, 10.5f, -6);

        bool isCollinear = vector1.IsCollinearTo(vector2);

        Assert.That(isCollinear, Is.True);
    }
}