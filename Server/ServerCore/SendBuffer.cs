namespace ServerCore
{
    public class SendBufferHelper
    {
        public static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>(() => null);

        public static int ChunkSize { get; set; } = 65535;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (CurrentBuffer.Value == null)
                CurrentBuffer.Value = new SendBuffer(ChunkSize);

            if (CurrentBuffer.Value.Capacity < reserveSize)
                CurrentBuffer.Value = new SendBuffer(ChunkSize);

            return CurrentBuffer.Value.Open(reserveSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return CurrentBuffer.Value.Close(usedSize);
        }
    }

    public class SendBuffer
    {
        private byte[] buffer;
        private int cursor = 0;

        public int Capacity => buffer.Length - cursor; 

        public SendBuffer(int chunkSize)
        {
            buffer = new byte[chunkSize];
        }

        public ArraySegment<byte> Open(int reserveSize)
        {
            if (reserveSize > Capacity)
                return null;

            return new ArraySegment<byte>(buffer, cursor, reserveSize);
        }

        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> register = new ArraySegment<byte>(buffer, cursor, usedSize);
            cursor += usedSize;
             
            return register;
        }
    }
}
