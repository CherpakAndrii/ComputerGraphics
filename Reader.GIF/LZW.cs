using System.Collections;
using System.Text;

namespace Reader.GIF;

public class Lzw
{
    public byte[] Decompress(IEnumerable<byte> compressedData)
    {
	    List<byte> decompressedData = new();
        Dictionary<int, string> dictionary = new();
        for (int i = 0; i <= 63; i++)
		{
			dictionary.Add(i, Convert.ToChar(i).ToString());
		}
		int digitCapacity = 7, index = 64, maxIndex = 128;
		string tempStr = string.Empty, prev = string.Empty;
		var firstPriorWordWasRead = true;
		foreach (var compressedByte in compressedData)
		{
			var compressedReverseByte = Reverse(compressedByte);
			for (int i = 7; i >= 0; i--)
			{
				int bit = (compressedReverseByte >> i) & 1;
				tempStr += bit.ToString();

				if (tempStr.Length != digitCapacity) continue;

				int tempInt = IntFromStr(tempStr);
				if (firstPriorWordWasRead)
				{
					decompressedData.Add((byte)tempInt);
					firstPriorWordWasRead = false;
					prev = Convert.ToChar(tempInt).ToString();
				}
				else {
					if (dictionary.TryGetValue(tempInt, out var value)) {
						prev += value[0];
						dictionary.Add(index++, prev);
						if (index == maxIndex - 1) {
							maxIndex *= 2;
							digitCapacity++;
						}
						decompressedData.AddRange(Encoding.ASCII.GetBytes(dictionary[tempInt]));
					}
					else {
						dictionary.Add(index++, prev + prev[0]);
						if (index == maxIndex - 1) {
							maxIndex *= 2;
							digitCapacity++;
						}
						decompressedData.AddRange(Encoding.ASCII.GetBytes(prev + prev[0]));
					}
					prev = dictionary[tempInt];
				}
				tempStr = string.Empty;
			}
		}

		return decompressedData.ToArray();
    }

    private byte Reverse(byte b)
    {
        int a = 0;
        for (int i = 0; i < 8; i++)
            if ((b & (1 << i)) != 0)
                a |= 1 << (7 - i);
        return (byte)a;
    }

    private static int IntFromStr(string str)
    {
        int value = 0;
		for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '1')
                value += Convert.ToInt32(Math.Pow(2, i));
        }
        return value;
    }
}