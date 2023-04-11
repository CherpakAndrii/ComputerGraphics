using System.Diagnostics;

namespace Writer.GIF;

public static class ColorClusterization
{
    public static ushort[] Clusterize((double, double, double)[] normalizedData, ref ushort numberOfClusters)
    {
        ushort[] clusterIndexes = InitializeRandomClusters((uint)normalizedData.Length, numberOfClusters);
        (double, double, double)[] centers = new (double, double, double)[numberOfClusters];
        int ctr = 50;
        while (ctr-- > 0 && GetClusterCenters(ref centers, normalizedData, numberOfClusters, clusterIndexes))
        {
            clusterIndexes = UpdateClustersByCenters(centers, normalizedData);
        }

        centers = ClearUnusedClusters(centers, clusterIndexes);
        clusterIndexes = UpdateClustersByCenters(centers, normalizedData);

        numberOfClusters = (ushort)centers.Length;
        return clusterIndexes;
    }

    private static ushort[] InitializeRandomClusters(uint sizeOfData, ushort numberOfClusters)
    {
        ushort[] clusterIndexes = new ushort[sizeOfData];
        for (int i = 0; i < sizeOfData && i < numberOfClusters; i++)
        {
            clusterIndexes[i] = (ushort)i;
        }

        Random random = new Random();
        for (int i = numberOfClusters; i < sizeOfData; i++)
        {
            clusterIndexes[i] = (ushort)random.Next(numberOfClusters);
        }

        return clusterIndexes;
    }
    
    private static bool GetClusterCenters(ref (double, double, double)[] centers, (double, double, double)[] normalizedData,
        ushort numberOfClusters, ushort[] clusterIndexes)
    {
        (double, double, double)[] newCenters = new (double, double, double)[numberOfClusters];
        int[] clusterCounters = new int[numberOfClusters];

        for (int i = 0; i < normalizedData.Length; i++)
        {
            clusterCounters[clusterIndexes[i]]++;
            newCenters[clusterIndexes[i]] = newCenters[clusterIndexes[i]].Plus(normalizedData[i]);
        }

        bool smthIsChanged = false;
        
        for (int i = 0; i < numberOfClusters; i++)
        {
            if (clusterCounters[i] == 0) continue;
            newCenters[i] = newCenters[i].Divide(clusterCounters[i]);
            if (centers[i] != newCenters[i])
            {
                smthIsChanged = true;
                centers[i] = newCenters[i];
            }
        }

        return smthIsChanged;
    }
    
    private static ushort[] UpdateClustersByCenters((double, double, double)[] centers, (double, double, double)[] normalizedData)
    {
        ushort[] clusterIndexes = new ushort[normalizedData.Length];
        Parallel.For(0, normalizedData.Length,
            (i) => { var ind = GetClosestCluster(centers, normalizedData[i]);
                clusterIndexes[i] = ind;
            });

        return clusterIndexes;
    }

    private static ushort GetClosestCluster((double, double, double)[] centers, (double, double, double) element)
    {
        ushort closestIndex = 0;
        double minDistance = GetDistance(centers[0], element), distance;
        for (ushort i = 1; i < centers.Length; i++)
        {
            distance = GetDistance(centers[i], element);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private static double GetDistance((double, double, double) el1, (double, double, double) el2)
    {
        return Math.Sqrt(Math.Pow(el1.Item1 - el2.Item1, 2) + Math.Pow(el1.Item2 - el2.Item2, 2) +
                         Math.Pow(el1.Item3 - el2.Item3, 2));
    }

    private static (double, double, double)[] ClearUnusedClusters((double, double, double)[] centers, ushort[] clusterIndexes)
    {
        int[] clusterCounters = new int[centers.Length];
        foreach (var i in clusterIndexes)
        {
            clusterCounters[i]++;
        }

        (double, double, double)[] newCenters = new (double, double, double)[centers.Length];
        int ctr = 0;
        for (int i = 0; i < centers.Length; i++)
        {
            if (clusterCounters[i] > 0)
                newCenters[ctr++] = centers[i];
        }

        return newCenters[..ctr];
    }
}

public static class TupleOperations
{
    public static (double, double, double) Plus(this (double, double, double) t1, (double, double, double) t2)
    {
        return (t1.Item1 + t2.Item1, t1.Item2 + t2.Item2, t1.Item3 + t2.Item3);
    }
    
    public static (double, double, double) Divide(this (double, double, double) t1, double denominator)
    {
        return (t1.Item1/denominator, t1.Item2/denominator, t1.Item3/denominator);
    }
}