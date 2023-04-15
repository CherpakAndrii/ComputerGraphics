using System.Collections;
using System.Text;

namespace Reader.GIF;

public class Lzw
{
	private Dictionary<int, string> GetInitializedDictionary()
	{
		Dictionary<int, string> dictionary = new();
		for (int i = 0; i <= 129; i++)
		{
			dictionary.Add(i, Convert.ToChar(i).ToString());
		}

		return dictionary;
	}
    public byte[] Decompress(IEnumerable<byte> compressedData)
    {
	    
	    List<byte> decompressedData = new();
	    Dictionary<int, string> dictionary = GetInitializedDictionary();
	    int code_size = 7;
	    int digitCapacity = code_size + 1, index = 130, maxIndex = 256;
		string tempStr = string.Empty, prev = string.Empty;
		string tempStr2 = string.Empty;
		var firstPriorWordWasRead = true;
		int counter = 0;
		
		int cc = (int)Math.Pow(2, code_size);
		int end = cc + 1;
		foreach (var compressedByte in compressedData)
		{
			counter++;
			var compressedReverseByte = compressedByte;
			for (int i = 7; i >= 0; i--)
			{
				int bit = (compressedReverseByte >> i) & 1;
				if ((tempStr2 + tempStr).Length < 8)
				{
					tempStr += bit;
				}
				else
				{
					tempStr2 += bit;
				}
				
				
				if ((tempStr2 + tempStr).Length != digitCapacity) continue;

				tempStr = tempStr + tempStr2;

				int tempInt = IntFromStr(tempStr);
				if (tempInt == cc)
				{
					firstPriorWordWasRead = false;
					dictionary = GetInitializedDictionary();
					digitCapacity = code_size + 1;
				}
				else if (tempInt == end)
				{
					
				}
				else
				{
					if (firstPriorWordWasRead)
					{
						decompressedData.Add((byte)tempInt);
						firstPriorWordWasRead = false;
						prev = Convert.ToChar(tempInt).ToString();
					}
					else
					{
						string entry;
						if (dictionary.TryGetValue(tempInt, out var value)) {
							prev += value[0];
							dictionary.Add(index++, prev);
							if (index == maxIndex - 1) {
								maxIndex *= 2;
								digitCapacity++;
							}
							decompressedData.AddRange(Encoding.ASCII.GetBytes(dictionary[tempInt]));
							entry = value;
						}
						else {
							dictionary.Add(index++, prev + prev[0]);
							if (index == maxIndex - 1) {
								maxIndex *= 2;
								digitCapacity++;
							}

							if (digitCapacity > 13)
							{
								Console.WriteLine("Hello");
							}
							decompressedData.AddRange(Encoding.ASCII.GetBytes(prev + prev[0]));
							entry = prev + prev[0];
						}

						prev = entry;
						if (digitCapacity > 13)
						{
							dictionary = GetInitializedDictionary();
							digitCapacity = code_size + 1;
							firstPriorWordWasRead = false;
						}
					}
				}
				tempStr = string.Empty;
				tempStr2 = string.Empty;
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
                value += Convert.ToInt32(Math.Pow(2, (str.Length - i - 1)));
        }
        return value;
    }
}