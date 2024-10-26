using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BinaryVersion.Program;
namespace BinaryVersion;

public class Reader()
{
    // Hoyo are weird for adding an empty byte before a new string...
    public static string ReadCustomString(BinaryReader reader)
    {
        reader.ReadByte();
        return reader.ReadString();
    }

    public static void Read(string BinaryVersionPath)
    {
        if (!File.Exists(BinaryVersionPath))
        {
            Console.WriteLine("ERROR: The path doesn't contain BinaryVersion.bytes file!");
            return;
        }

        var BinaryVersionBytes = File.ReadAllBytes(BinaryVersionPath);
        using var ms = new MemoryStream(BinaryVersionBytes);
        using var br = new EndianBinaryReader(ms, Encoding.UTF8);
        BinaryVersionData binaryVersionData = new();

        foreach (var i in typeof(BinaryVersionData).GetProperties())
        {
            switch (i.PropertyType)
            {
                case Type t when t == typeof(byte):
                    i.SetValue(binaryVersionData, br.ReadByte());
                    break;
                case Type t when t == typeof(uint):
                    i.SetValue(binaryVersionData, br.ReadUInt32BE());
                    break;
                case Type t when t == typeof(string):
                    i.SetValue(binaryVersionData, ReadCustomString(br));
                    break;
                case Type t when t == typeof(bool):
                    i.SetValue(binaryVersionData, br.ReadBoolean());
                    break;
                default:
                    // should NOT happen
                    throw new NotSupportedException("Stupid happened :(");
            }
        }

        string jsonStr = JsonConvert.SerializeObject(binaryVersionData, Formatting.Indented);
        File.WriteAllText("BinaryVersion.json", jsonStr);
        Console.WriteLine("Parsing complete, the data will be saved in BinaryVersion.json.");
    }
}