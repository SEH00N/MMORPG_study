namespace PacketGenerator
{
    public class PacketFormats
    {
        // {0} 패킷 등록
        public static string ManagerFormat =
@"using System;
using System.Collections.Generic;
using ServerCore;

public class PacketManager
{{
    private static PacketManager instance = new PacketManager();
    public static PacketManager Instance => instance;

    private Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> onReceive = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();
    private Dictionary<ushort, Action<PacketSession, IPacket>> handlers = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    private PacketManager()
    {{
        Register();
    }}
            
    public void Register()
    {{
{0}
    }}

    public void OnReceivePacket(PacketSession session, ArraySegment<byte> buffer)
    {{
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;

        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Action<PacketSession, ArraySegment<byte>> action = null;
        if (onReceive.TryGetValue(id, out action))
            action?.Invoke(session, buffer);
    }}

    private void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
    {{
        T packet = new T();
        packet.Read(buffer);

        Action<PacketSession, IPacket> action = null;
        if (handlers.TryGetValue(packet.Protocol, out action))
            action?.Invoke(session, packet);
    }}
}}";

        // {0} 패킷 이름
        public static string ManagerRegisterFormat =
@"        onReceive.Add((ushort)PacketID.{0}, MakePacket<{0}>);
        handlers.Add((ushort)PacketID.{0}, PacketHandler.{0}Handler);
";

        // {0} 패킷 정보
        // {1} 패킷 목록
        public static string FileFormat =
@"using System;
using System.Collections.Generic;
using System.Text;
using ServerCore;

public enum PacketID
{{
    {0}
}}

public interface IPacket
{{
    public ushort Protocol {{ get; }}
    
    public void Read(ArraySegment<byte> buffer);
	public ArraySegment<byte> Write();
}}
{1}
";

        // {0} 패킷 이름
        // {1} 패킷 번호
        public static string PacketEnumFormat = 
@"{0} = {1},
    ";

        // {0} 패킷 이름
        // {1} 멤버 변수
        // {2} Read
        // {3} Write
        public static string PacketFormat =
@"
public class {0} : IPacket
{{
    public ushort Protocol => (ushort)PacketID.{0};

    {1}

    public void Read(ArraySegment<byte> buffer)
    {{
        ushort count = 0;

        ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(buffer.Array, buffer.Offset, buffer.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);

        {2}
    }}

    public ArraySegment<byte> Write()
    {{
        ArraySegment<byte> buffer = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> span = new Span<byte>(buffer.Array, buffer.Offset, buffer.Count);

        count += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort)PacketID.{0});
        count += sizeof(ushort);

        {3}

        success &= BitConverter.TryWriteBytes(span, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }}
}}
";
        // {0} 자료형
        // {1} 변수명
        public static string MemberFormat = @"public {0} {1};";

        // {0} 리스트 이름 [대문자]
        // {1} 리스트 이름 [소문자]
        // {2} 멤버 변수
        // {3} Read
        // {4} Write
        public static string MemberListFormat =
@"public class {0}
{{
    {2}

    public void Read(ReadOnlySpan<byte> span, ref ushort count)
    {{
        {3}
    }} 

    public bool Write(Span<byte> span, ref ushort count)
    {{
        bool success = true;

        {4}

        return success;
    }}
}}
public List<{0}> {1}s = new List<{0}>();
";

        // {0} 변수명
        // {1} Format
        // {2} 자료형
        public static string ReadFormat =
@"this.{0} = BitConverter.{1}(span.Slice(count, span.Length - count));
count += sizeof({2});";
        
        // {0} 변수명
        // {1} 자료형
        public static string ReadByteFormat =
@"this.{0} = ({1})buffer.Array[buffer.Offset + count];
count += sizeof({1});";

        // {0} 변수명
        public static string ReadStringFormat =
@"ushort {0}Len = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
count += sizeof(ushort);

this.{0} = Encoding.Unicode.GetString(span.Slice(count, {0}Len));
count += {0}Len;";

        // {0} 리스트 이름 [대문자]
        // {1} 리스트 이름 [소문자]
        public static string ReadListFormat =
@"this.{1}s.Clear();
ushort {1}Len = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
count += sizeof(ushort);

for(int i = 0; i < {1}Len; i++)
{{
    {0} {1} = new {0}();
    {1}.Read(span, ref count);
    {1}s.Add({1});
}}";

        // {0} 변수명
        // {1} 자료형
        public static string WriteFormat =
@"success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), this.{0});
count += sizeof({1});";

        // {0} 변수명
        // {1} 자료형
        public static string WriteByteFormat =
@"buffer.Array[buffer.Offset + count] = (byte)this.{0};
count += sizeof({1});";

        // {0} 변수명
        public static string WriteStringFormat =
@"ushort {0}Len = (ushort)Encoding.Unicode.GetBytes(this.{0}, 0, this.{0}.Length, buffer.Array, buffer.Offset + count + sizeof(ushort));
success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), {0}Len);
count += sizeof(ushort);
count += {0}Len;";

        // {0} 리스트 이름 [대문자]
        // {1} 리스트 이름 [소문자]
        public static string WriteListFormat =
@"success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort)this.{1}s.Count);
count += sizeof(ushort);
foreach({0} {1} in this.{1}s)
    success &= {1}.Write(span, ref count);";
    }
}
