START ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
XCOPY /Y /I Packets.cs "../../DummyClient/Packet/*"
XCOPY /Y /I Packets.cs "../../Server/Packet/*"

XCOPY /Y /I ClientPacketManager.cs "../../DummyClient/Packet/*"
XCOPY /Y /I ServerPacketManager.cs "../../Server/Packet/*"