using Core.Lights;

namespace Writer.GIF;

public class Normalization
{
    public static (double, double, double)[] Normalize(Color[] colors)
    {
        double[] means = new double[3]; // rMean, gMean, bMean
        double[] deviations = new double[3];    //rDeviation, gDeviation, bDeviation;
        byte[] upperBounds = new byte[3];   //rmax, gmax, bmax
        byte[] lowerBounds = {255, 255, 255};   //rmin, gmin, bmin

        foreach (var color in colors)
        {
            byte[] components = { (byte)color.R, (byte)color.G, (byte)color.B };
            for (int i = 0; i < 3; i++)
            {
                means[i] += components[i];
                if (components[i] > upperBounds[i]) upperBounds[i] = components[i];
                if (components[i] < lowerBounds[i]) lowerBounds[i] = components[i];
            }
        }

        for (int i = 0; i < 3; i++)
        {
            means[i] /= colors.Length;
            double udev = upperBounds[i] - means[i], ldev = means[i] - lowerBounds[i];
            deviations[i] = ldev > udev ? ldev : udev > 0 ? udev : 1;
        }

        (double, double, double)[] normalized = new (double, double, double)[colors.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            normalized[i] = (
                (colors[i].R - means[0]) / deviations[0] * 100,
                (colors[i].G - means[1]) / deviations[1] * 100,
                (colors[i].B - means[2]) / deviations[2] * 100);
        }

        return normalized;
    }
    
    
}