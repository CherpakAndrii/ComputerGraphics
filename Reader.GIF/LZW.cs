using System.Text;

namespace Reader.GIF;

public class Lzw
{
    public byte[] Decompress(IEnumerable<byte> compressedData)
    {
	    List<byte> decompressedData = new();
        Dictionary<int, string> dictionary = new();
        for (int i = 0; i <= 255; i++)
		{
			dictionary.Add(i, Convert.ToChar(i).ToString());
		}
		int digitCapacity = 9, index = 256, maxIndex = 512;
		string tempStr = string.Empty, prev = string.Empty;
		var firstPriorWordWasRead = true;
		foreach (var compressedByte in compressedData)
		{
			for (int i = 7; i >= 0; i--)
			{
				int bit = (compressedByte >> i) & 1;
				tempStr += bit.ToString();

				if (tempStr.Length != digitCapacity) continue;

				int tempInt = Convert.ToInt32(tempStr);
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
}