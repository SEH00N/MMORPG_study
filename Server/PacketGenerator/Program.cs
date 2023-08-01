using System.Xml;

namespace PacketGenerator
{
    internal class Program
    {
        static string generatedPackets;
        static ushort packetID;
        static string packetEnums;

        static string clientRegister;
        static string serverRegister;

        static void Main(string[] args)
        {
            // PDL => Packet Definition List
            string pdlPath = "../PDL.xml";

            XmlReaderSettings settings = new XmlReaderSettings()
            { 
                IgnoreComments = true,
                IgnoreWhitespace = true
            };

            if(args.Length >= 1)
                pdlPath = args[0];

            using (XmlReader reader = XmlReader.Create(pdlPath, settings))
            {
                reader.MoveToContent();
                while(reader.Read())
                {
                    if(reader.Depth == 1 && reader.NodeType == XmlNodeType.Element)
                        ParsePacket(reader);
                    Console.WriteLine(reader.Name + " " + reader["name"]);
                }

                string result = string.Format(PacketFormats.FileFormat, packetEnums, generatedPackets);
                File.WriteAllText("Packets.cs", result);

                string clientManagerText = string.Format(PacketFormats.ManagerFormat, clientRegister);
                File.WriteAllText("ClientPacketManager.cs", clientManagerText);

                string serverManagerText = string.Format(PacketFormats.ManagerFormat, serverRegister);
                File.WriteAllText("ServerPacketManager.cs", serverManagerText);
            }
        }

        public static void ParsePacket(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.EndElement)
                return;

            if (reader.Name.ToLower() != "packet")
            {
                Console.WriteLine("Invalid packet node");
                return;
            }

            string packetName = reader["name"];
            if(string.IsNullOrEmpty(packetName))
            {
                Console.WriteLine("Packet without name");
                return;
            }

            Tuple<string, string, string>  tuple = ParseMembers(reader);
            generatedPackets += string.Format(PacketFormats.PacketFormat, packetName, tuple.Item1, tuple.Item2, tuple.Item3);
            packetEnums += string.Format(PacketFormats.PacketEnumFormat, packetName, ++packetID);

            // S_ => 클라가 서버로 보내는 패킷
            // C_ => 서버가 클라로 보내는 패킷
            if(packetName.StartsWith("S_") || packetName.StartsWith("s_"))
                clientRegister += string.Format(PacketFormats.ManagerRegisterFormat, packetName);
            else
                serverRegister += string.Format(PacketFormats.ManagerRegisterFormat, packetName);
        }

        // {1} 멤버 변수
        // {2} Read
        // {3} Write
        private static Tuple<string, string, string> ParseMembers(XmlReader reader)
        {
            string packetName = reader["name"];
            
            string memberCode = "";
            string readCode = "";
            string writeCode = "";

            int depth = reader.Depth + 1;
            while(reader.Read())
            {
                if(reader.Depth != depth)
                {
                    break;
                }

                string memberName = reader["name"];
                if(string.IsNullOrEmpty(memberName))
                {
                    Console.WriteLine("Member without name");
                    return null;
                }

                if (string.IsNullOrEmpty(memberCode) == false)
                    memberCode += Environment.NewLine;
                if (string.IsNullOrEmpty(readCode) == false)
                    readCode += Environment.NewLine;
                if (string.IsNullOrEmpty(writeCode) == false)
                    writeCode += Environment.NewLine;

                string memberType = reader.Name.ToLower();
                switch(memberType)
                {
                    case "bool":
                    case "short":
                    case "ushort":
                    case "int":
                    case "long":
                    case "float":
                    case "double":
                        memberCode += string.Format(PacketFormats.MemberFormat, memberType, memberName);
                        readCode += string.Format(PacketFormats.ReadFormat, memberName, MemberTypeFormat(memberType), memberType);
                        writeCode += string.Format(PacketFormats.WriteFormat, memberName, memberType);
                        break;
                    case "byte":
                    case "sbyte":
                        memberCode += string.Format(PacketFormats.MemberFormat, memberType, memberName);
                        readCode += string.Format(PacketFormats.ReadByteFormat, memberName, memberType);
                        writeCode += string.Format(PacketFormats.WriteByteFormat, memberName, memberType);
                        break;
                    case "string":
                        memberCode += string.Format(PacketFormats.MemberFormat, memberType, memberName);
                        readCode += string.Format(PacketFormats.ReadStringFormat, memberName);
                        writeCode += string.Format(PacketFormats.WriteStringFormat, memberName);
                        break;
                    case "list":
                        Tuple<string, string, string> tuple = ParseList(reader);
                        memberCode += tuple.Item1;
                        readCode += tuple.Item2;
                        writeCode += tuple.Item3;
                        break;
                    default:
                        break;
                }
            }

            memberCode = memberCode.Replace("\n", "\n\t");
            readCode = readCode.Replace("\n", "\n\t\t");
            writeCode = writeCode.Replace("\n", "\n\t\t");
            return new Tuple<string, string, string>(memberCode, readCode, writeCode);
        }

        private static Tuple<string, string, string> ParseList(XmlReader reader)
        {
            string listName = reader["name"];
            if(string.IsNullOrEmpty(listName))
            {
                Console.WriteLine("List without name");
                return null;
            }

            Tuple<string, string, string> tuple = ParseMembers(reader);
            string memberCode = string.Format(
                PacketFormats.MemberListFormat,
                FirstCharToUpper(listName),
                FirstCharToLower(listName),
                tuple.Item1,
                tuple.Item2,
                tuple.Item3
            );
            string readCode = string.Format(PacketFormats.ReadListFormat, FirstCharToUpper(listName), FirstCharToLower(listName));
            string writeCode = string.Format(PacketFormats.WriteListFormat, FirstCharToUpper(listName), FirstCharToLower(listName));

            return new Tuple<string, string, string>(memberCode, readCode, writeCode);
        }

        public static string MemberTypeFormat(string memberType)
        {
            switch (memberType)
            {
                case "bool":
                    return "ToBoolean";
                case "short":
                    return "ToInt16";
                case "ushort":
                    return "ToUInt16";
                case "int":
                    return "ToInt32";
                case "long":
                    return "ToInt64";
                case "float":
                    return "ToSingle";
                case "double":
                    return "ToDouble";
                default:
                    return "";
            }
        }

        public static string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input[0].ToString().ToUpper() + input.Substring(1);
        }

        public static string FirstCharToLower(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input[0].ToString().ToLower() + input.Substring(1);
        }
    }
}