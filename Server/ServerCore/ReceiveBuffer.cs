namespace ServerCore
{
    public class ReceiveBuffer
    {
        private ArraySegment<byte> buffer;
        private int readCursor;
        private int writeCursor;

        public ReceiveBuffer(int bufferSize)
        {
            buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        public int Size => writeCursor - readCursor;
        public int Capacity => buffer.Count - writeCursor;

        public ArraySegment<byte> ReadSegment => new ArraySegment<byte>(buffer.Array, buffer.Offset + readCursor, Size);
        public ArraySegment<byte> WriteSegment => new ArraySegment<byte>(buffer.Array, buffer.Offset + writeCursor, Capacity);

        public void Clean()
        {
            int dataSize = Size;
            if(dataSize == 0)
            {
                // 남은 데이터가 없으면 (모든 데이터를 읽었으면) 커서 위치 리셋
                readCursor = writeCursor = 0;
            }
            else
            {
                // 남은 잔여 데이터들이 있으면 시작 위치로 복사
                Array.Copy(buffer.Array, buffer.Offset + readCursor, buffer.Array, buffer.Offset, dataSize);
                readCursor = 0;
                writeCursor = dataSize;
            }
        }

        public bool OnRead(int numOfBytes)
        {
            if (numOfBytes > Size)
                return false;

            readCursor += numOfBytes;
            return true;
        }

        public bool OnWrite(int numOfBytes)
        {
            if (numOfBytes > Capacity)
                return false;

            writeCursor += numOfBytes;
            return true;
        }
    }
}
