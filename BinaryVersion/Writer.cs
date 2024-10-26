using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BinaryVersion.Program;
namespace BinaryVersion;

public class Writer()
{
    // Hoyo are weird for adding an empty byte before a new string...
    public static void WriteCustomString(EndianBinaryWriter writer, string stringVal)
    {
        writer.Write((byte)0);
        writer.WriteString(stringVal);
        return;
    }

    public static void Write(BinaryVersionData binaryVersionData, string BinaryVersionPath)
    {
        using var ms = new MemoryStream();
        using var bw = new EndianBinaryWriter(ms, Encoding.UTF8);

        foreach (var i in typeof(BinaryVersionData).GetProperties())
        {
            switch (i.PropertyType)
            {
                case Type t when t == typeof(byte):
                    bw.Write((byte)i.GetValue(binaryVersionData)!);
                    break;
                case Type t when t == typeof(uint):
                    bw.WriteUInt32BE((uint)i.GetValue(binaryVersionData)!);
                    break;
                case Type t when t == typeof(string):
                    WriteCustomString(bw, (string)i.GetValue(binaryVersionData)!);
                    break;
                case Type t when t == typeof(bool):
                    bw.Write((bool)i.GetValue(binaryVersionData)!);
                    break;
                default:
                    // should NOT happen
                    throw new NotSupportedException("Stupid happened :(");
            }
        }

        File.WriteAllBytes(BinaryVersionPath, ms.ToArray());
        Console.WriteLine("Writing complete, the data has been saved to " + BinaryVersionPath);
    }
}