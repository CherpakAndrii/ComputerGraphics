﻿using System.Text;
using ImageFormatConverter.Common;

namespace Writer.GIF;

public class Lzw
{
	public Dictionary<string, int> dictionary;
	
	public Dictionary<string, int> _Firstdictionary;
	public byte[] Сompress(IEnumerable<byte> decompressedData, int codeSize)
	{
		int clearCode = (int)Math.Pow(2, codeSize);
		int endOfInformation = clearCode + 1;
		int digitCapacity = codeSize + 1, index = clearCode + 2, maxIndex = clearCode * 2;
		
		var decompressedDataList = decompressedData.ToList();
		
		dictionary = GetInitializedCompressorDictionary(clearCode + 1);

		var currentlyRecognised = string.Empty;
		
		List<string> stringBinaryEncoding = new() { Helper.IntToBinary(clearCode, digitCapacity) };

		foreach (var decompressedByte in decompressedDataList)
		{
			if (dictionary.ContainsKey(currentlyRecognised + (char)decompressedByte) || string.IsNullOrEmpty(currentlyRecognised))
			{
				currentlyRecognised += ((char)decompressedByte).ToString();
			}
			else
			{
				stringBinaryEncoding.Add(Helper.IntToBinary(dictionary[currentlyRecognised], digitCapacity));

				if (index == maxIndex && digitCapacity < 13)
				{
					digitCapacity++;
					maxIndex *= 2;
				}
				
				dictionary.Add(currentlyRecognised + (char)decompressedByte, index++);
				currentlyRecognised = ((char)decompressedByte).ToString();
				
				if (digitCapacity > 12)
				{
					_Firstdictionary = dictionary;
					stringBinaryEncoding.Add(Helper.IntToBinary(clearCode, 12));
					dictionary = GetInitializedCompressorDictionary(clearCode + 1);
					index = clearCode + 2;
					maxIndex = clearCode * 2;
					digitCapacity = codeSize + 1;
					currentlyRecognised = "";
				}
			}
		}
		
		if (index == maxIndex)
		{
			digitCapacity++;
		}

		stringBinaryEncoding.Add(Helper.IntToBinary(dictionary[currentlyRecognised], digitCapacity));
		stringBinaryEncoding.Add(Helper.IntToBinary(endOfInformation, digitCapacity));

		var compressedBitsString = string.Join("", stringBinaryEncoding.ToArray());
		var compressedBits = Enumerable.Range(0, compressedBitsString.Length / 8).
			Select(pos => Convert.ToByte(
				compressedBitsString.Substring(pos * 8, 8),
				2)
			).ToList();

		if (compressedBitsString.Length % 8 == 0)
			return compressedBits.ToArray();

		var substringPosition = compressedBitsString.Length - compressedBitsString.Length % 8;
		var substringLength = compressedBitsString.Length % 8;
		compressedBits.Add(Convert.ToByte(compressedBitsString.Substring(substringPosition, substringLength).PadRight(8, '0'), 2));

		return compressedBits.ToArray();
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