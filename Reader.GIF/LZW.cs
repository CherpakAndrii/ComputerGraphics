using ImageFormatConverter.Common;
using System.Collections;
using System.Text;

namespace Reader.GIF;

public class Lzw
{
	public byte[] Decompress(IEnumerable<byte> compressedData, int codeSize)
    {
	    int clearCode = (int)Math.Pow(2, codeSize);
	    int endOfInformation = clearCode + 1;

	    List<byte> decompressedData = new();
	    var dictionary = GetInitializedDecompressorDictionary(clearCode + 1);
	    int digitCapacity = codeSize + 1, index = clearCode + 2, maxIndex = clearCode * 2;
		string codeWord = string.Empty, previousCodeWord = string.Empty;

		foreach (var compressedByte in compressedData)
		{
			for (int i = 0; i <=7; i++)
			{
				var bit = (compressedByte >> i) & 1;
				codeWord += bit;
				
				if (codeWord.Length != digitCapacity)
					continue;

				int codeWordInt = Helper.IntFromStr(codeWord);
				
				if (codeWordInt == clearCode)
				{
					dictionary = GetInitializedDecompressorDictionary((int)Math.Pow(2, codeSize) + 1);
					index = clearCode + 2;
					maxIndex = clearCode * 2;
					digitCapacity = codeSize + 1;
					previousCodeWord = string.Empty;
				}
				else if (codeWordInt == endOfInformation)
				{
				}
				else
				{
					if (string.IsNullOrEmpty(previousCodeWord))
					{
						decompressedData.Add((byte)codeWordInt);
						previousCodeWord = Convert.ToChar(codeWordInt).ToString();
					}
					else
					{
						string entry;

						if (dictionary.TryGetValue(codeWordInt, out var value)) {
							entry = value;
						}
						else {
							entry = previousCodeWord + previousCodeWord[0];
						}
						
						decompressedData.AddRange(Encoding.ASCII.GetBytes(entry));
						dictionary.Add(index++, previousCodeWord + entry[0]);
						previousCodeWord = entry;

						if (index == maxIndex && digitCapacity < 12) {
							maxIndex *= 2;
							digitCapacity++;
						}

						
					}
				}

				codeWord = string.Empty;
			}
		}

		return decompressedData.ToArray();
    }

	public byte[] Сompress(IEnumerable<byte> decompressedData, int codeSize)
	{
		int clearCode = (int)Math.Pow(2, codeSize);
		int endOfInformation = clearCode + 1;
		int digitCapacity = codeSize + 1, index = clearCode + 2, maxIndex = clearCode * 2;
		
		var decompressedDataList = decompressedData.ToList();
		
		var dictionary = GetInitializedCompressorDictionary(clearCode + 1);

		var currentlyRecognised = ((char)decompressedDataList[0]).ToString();
		decompressedDataList.RemoveAt(0);
		
		List<string> stringBinaryEncoding = new();
		
		foreach (var decompressedByte in decompressedDataList)
		{
			if (dictionary.ContainsKey(currentlyRecognised + (char)decompressedByte))
			{
				currentlyRecognised += ((char)decompressedByte).ToString();
			}
			else
			{
				if (index == maxIndex)
				{
					digitCapacity++;
					maxIndex *= 2;
				}
				stringBinaryEncoding.Add(Helper.IntToBinary(dictionary[currentlyRecognised], digitCapacity));
				dictionary.Add(currentlyRecognised + (char)decompressedByte, index);
				index++;
				currentlyRecognised = ((char)decompressedByte).ToString();
			}
		}
		
		if (index == maxIndex)
		{
			digitCapacity++;
			maxIndex *= 2;
		}
		stringBinaryEncoding.Add(Helper.IntToBinary(dictionary[currentlyRecognised], digitCapacity));

		return Array.Empty<byte>();
	}
	
	private static Dictionary<int, string> GetInitializedDecompressorDictionary(int maxSize)
	{
		Dictionary<int, string> dictionary = new();
		for (int i = 0; i <= maxSize - 2; i++)
		{
			dictionary.Add(i, Convert.ToChar(i).ToString());
		}

		return dictionary;
	}

	private static Dictionary<string, int> GetInitializedCompressorDictionary(int maxSize)
	{
		Dictionary<string, int> dictionary = new();
		for (int i = 0; i <= maxSize - 2; i++)
		{
			dictionary.Add(Convert.ToChar(i).ToString(), i);
		}

		return dictionary;
	}
}