using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using static BinaryVersion.Program;

namespace BinaryVersion
{
    public class Program
    {
        public class BinaryVersionData
        {
            public string Branch { get; set; } = string.Empty;
            public uint Revision { get; set; }
            public uint MajorVersion { get; set; }
            public uint MinorVersion { get; set; }
            public uint PatchVersion { get; set; }
            public uint KJLNPMBMHPP { get; set; }
            public uint BEHBCIGKAIA { get; set; }
            public uint DNHBBFJNFIA { get; set; }
            public uint JOGEBFOIHBA { get; set; }
            public uint KAHDPOCKEOK { get; set; }
            public uint DKAEGGDFNCE { get; set; }
            public uint IKGBOMPEEFK { get; set; }
            public uint KIIALDBNPGF { get; set; }
            public uint EAOFFMBMFEB { get; set; }
            public uint PBIENIGLJFK { get; set; }
            public uint FMOFDMGBLKN { get; set; }
            public uint NHLEELPBMPB { get; set; }
            public uint DIECOIPHCFH { get; set; }
            public uint MLBMIOPJKDN { get; set; }
            public uint AIJCAACHKPA { get; set; }
            public string Time { get; set; } = string.Empty;
            public string PakType { get; set; } = string.Empty;
            public string PakTypeDetail { get; set; } = string.Empty;
            public string StartAsset { get; set; } = string.Empty;
            public string StartDesignData { get; set; } = string.Empty;
            public string DispatchSeed { get; set; } = string.Empty;
            public string VersionString { get; set; } = string.Empty;
            public string VersionHash { get; set; } = string.Empty;
            public uint GameCoreVersion { get; set; }
            public bool IsEnableExcludeAsset { get; set; }
            public string Sdk_PS_Client_Id { get; set; } = string.Empty;
        }

        public static void Main(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: .exe <BinaryVersion.bytes path> <*BinaryVersion.json> (* = optional)");
                return;
            }

            var BinaryVersionPath = args[0];

            if (args.Length == 2)
            {
                if (!File.Exists(args[1]))
                {
                    Console.WriteLine("ERROR: The path doesn't contain BinaryVersion.json file!");
                    return;
                }
                BinaryVersionData binaryVersionData = JsonConvert.DeserializeObject<BinaryVersionData>(File.ReadAllText(args[1]))!;
                Writer.Write(binaryVersionData, BinaryVersionPath);
            } 
            else
            {
                Reader.Read(BinaryVersionPath);
            }
        }
    }
}
