using System.Collections;
using System.Text;

namespace Reader.GIF;

public class Lzw
{
	private (Dictionary<int, string>, Dictionary<int, List<int>>) GetInitializedDictionary(int maxSize)
	{
		Dictionary<int, string> dictionary = new();
		Dictionary<int, List<int>> dictionary2 = new();
		for (int i = 0; i <= maxSize - 2; i++)
		{
			dictionary.Add(i, Convert.ToChar(i).ToString());
			dictionary2.Add(i, new List<int>() { i });
		}

		return (dictionary, dictionary2);
	}
    public byte[] Decompress(IEnumerable<byte> compressedData, int code_size)
    {
	    
	    List<byte> decompressedData = new();
	    var (dictionary, dictionary2) = GetInitializedDictionary((int)Math.Pow(2, code_size) + 1);
	    int digitCapacity = code_size + 1, index = (int)Math.Pow(2, code_size) + 2, maxIndex = (int)Math.Pow(2, code_size + 1);
		string tempStr = string.Empty, prev = string.Empty;
		var firstPriorWordWasRead = true;

		foreach (var compressedByte in compressedData)
		{

			var compressedReverseByte = compressedByte;
			for (int i = 0; i <=7; i++)
			{
				int bit = (compressedReverseByte >> i) & 1;
				tempStr += bit;


				if ((tempStr).Length != digitCapacity) continue;

				int tempInt = IntFromStr(tempStr);
				
				int cc = (int)Math.Pow(2, code_size);
				int end = cc + 1;
				if (tempInt == cc)
				{
					firstPriorWordWasRead = true;
					(dictionary, dictionary2) = GetInitializedDictionary((int)Math.Pow(2, code_size) + 1);
					index = (int)Math.Pow(2, code_size) + 2;
					maxIndex = (int)Math.Pow(2, code_size + 1);
					digitCapacity = code_size + 1;
				}
				else if (tempInt == end)
				{
					Console.WriteLine("Hi");
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
							entry = value;
						}
						else {
							entry = prev + prev[0];
						}
						
						decompressedData.AddRange(Encoding.ASCII.GetBytes(entry));
						
						dictionary.Add(index++, prev + entry[0]);
						dictionary2.Add(index - 1, (prev + entry[0]).ToArray().Select(c => (int)c).ToList());
						
						if (index == maxIndex && digitCapacity < 12) {
							maxIndex *= 2;
							digitCapacity++;
						}

						prev = entry;
					}
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