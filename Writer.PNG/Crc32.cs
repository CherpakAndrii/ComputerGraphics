
namespace Writer.PNG
{
    //source: https://www.w3.org/TR/PNG-CRCAppendix.html
    public static class Crc32
    {
        private static uint[] crcTable = new uint[256];
        private const uint INITIAL_STATE = 0xFFFF_FFFFu;

        static Crc32()
        {
            uint c;
            for (int i = 0; i < 256; i++)
            {
                c = (uint)i;
                for (int j = 0; j < 8; j++)
                    c = (c & 1) == 1 ? (uint)0xedb88320L ^ (c >> 1) : c >> 1;
                crcTable[i] = c;
            }
        }

        public static byte[] Hash(byte[] input) => Hash(new ReadOnlySpan<byte>(input));

        public static byte[] Hash(ReadOnlySpan<byte> input) => Hash(input, INITIAL_STATE);

        public static byte[] Hash(ReadOnlySpan<byte> input, uint crc)
        {
            uint c = crc;
            for (int i = 0; i < input.Length; i++)
            {
                c = crcTable[(c ^ input[i]) & 0xff] ^ (c >> 8);
            }
            return BitConverter.GetBytes(~c);
        }
    }
}
