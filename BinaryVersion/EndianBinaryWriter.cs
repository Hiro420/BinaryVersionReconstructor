using System.Text;

namespace BinaryVersion;

public class EndianBinaryWriter : BinaryWriter
{
    public EndianBinaryWriter(Stream output) : base(output)
    {
    }

    public EndianBinaryWriter(Stream output, Encoding encoding) : base(output, encoding)
    {
    }

    public EndianBinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
    {
    }

    public void WriteUInt32BE(uint value) => base.Write(SwapBytes(value));

    public void WriteUInt64BE(ulong value) => base.Write(SwapBytes(value));

    public void WriteInt32BE(int value) => base.Write(SwapBytes((uint)value));

    public void WriteInt64BE(long value) => base.Write(SwapBytes((ulong)value));

    public void WriteExcelBitfield(List<bool> bits)
    {
        byte currentByte = 0;
        int bitCount = 0;

        foreach (var bit in bits)
        {
            currentByte >>= 1;
            if (bit)
                currentByte |= 0x80;

            bitCount++;
            if (bitCount == 7)
            {
                currentByte >>= 1; // Leave the top bit clear
                base.Write(currentByte);
                currentByte = 0;
                bitCount = 0;
            }
        }

        if (bitCount > 0)
        {
            currentByte >>= (7 - bitCount);
            base.Write((byte)(currentByte | 0x80)); // Set the continuation bit
        }
    }

    public void WriteVarInt(ulong value)
    {
        while (value > 0x7F)
        {
            base.Write((byte)((value & 0x7F) | 0x80));
            value >>= 7;
        }
        base.Write((byte)(value & 0x7F));
    }

    public void WriteSignedVarInt(long value)
    {
        WriteVarInt(EncodeZigZag(value));
    }

    private static ulong EncodeZigZag(long value)
    {
        return (ulong)((value << 1) ^ (value >> 63));
    }

    public void WriteHash(string hash)
    {
        if (hash.Length != 32)
            throw new ArgumentException("Hash must be exactly 32 characters long.");

        var fullHash = new byte[16];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                fullHash[i * 4 + j] = Convert.ToByte(hash.Substring((3 - j) * 2 + i * 8, 2), 16);
            }
        }

        base.Write(fullHash);
    }

    public void WriteStraightHash(string hash)
    {
        if (hash.Length != 32)
            throw new ArgumentException("Hash must be exactly 32 characters long.");

        var fullHash = Enumerable.Range(0, hash.Length / 2)
            .Select(x => Convert.ToByte(hash.Substring(x * 2, 2), 16))
            .ToArray();

        base.Write(fullHash);
    }

    private uint SwapBytes(uint x)
    {
        x = (x >> 16) | (x << 16);
        return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
    }

    private ulong SwapBytes(ulong x)
    {
        x = (x >> 32) | (x << 32);
        x = ((x & 0xFFFF0000FFFF0000) >> 16) | ((x & 0x0000FFFF0000FFFF) << 16);
        return ((x & 0xFF00FF00FF00FF00) >> 8) | ((x & 0x00FF00FF00FF00FF) << 8);
    }

    public void WriteString(string value, Encoding? encoding = null)
    {
        if (encoding == null)
        {
            encoding = Encoding.UTF8;
        }

        base.Write((byte)value.Length);
        var bytes = encoding.GetBytes(value);
        base.Write(bytes);
    }
}
