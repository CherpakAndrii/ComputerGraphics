using System.Collections;

namespace ImageFormatConverter.Common;

public static class Helper 
{
    public static int IntFromBitArray(BitArray bitArray, int from, int to)
    {
        int value = 0;
        if (from >= 0 && to - from <= 32 && bitArray.Count >= to)
        {
            for (int i = from; i < to; i++)
            {
                if (bitArray[i])
                    value += (int)Math.Pow(2, i - from);
            }
        }
        return value;
    }

    public static int IntFromStr(string str)
    {
        var value = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '1')
                value += (int)Math.Pow(2, i);
        }
        return value;
    }
    
    public static string IntToBinary(int number, int digitCapacity)
    {
        return Convert.ToString(number, 2).PadLeft(digitCapacity, '0');
    }
}