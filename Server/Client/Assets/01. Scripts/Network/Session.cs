using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerCore
{
    public abstract class PacketSession : Session
    {
        public static readonly int HeaderSize = 2;

        // 버퍼 형태
        // [size(2)][packetID(2)][ ... ] [size(2)][packetID(2)][ ... ]
        public sealed override int OnReceive(ArraySegment<byte> buffer)
        {
            int processLength = 0;
            int __packetCount = 0;

            while(true)
            {
                // 최소한 헤더를 파싱할 수 있는지 확인
                if (buffer.Count < HeaderSize)
                    break;

                // 패킷이 완전체로 도착했는지
                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                if (buffer.Count < dataSize)
                    break;

                // 어떻게든 하나 이상의 패킷을 조립 가능
                OnReceivePacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));
                __packetCount++;

                processLength += dataSize;
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
            }

            if(__packetCount > 1)
                Console.WriteLine($"Packet Received : {__packetCount}");

            return processLength;
        }

        public abstract void OnReceivePacket(ArraySegment<byte> buffer);
    }

    public abstract class Session
    {
        private Socket socket;
        private int disconnected = 0;

        private ReceiveBuffer receiveBuffer = new ReceiveBuffer(65535);

        private Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();
        private List<ArraySegment<byte>> pendingList = new List<ArraySegment<byte>>();
        private object locker = new object();

        private SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
        private SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();

        public abstract void OnConnected(EndPoint endPoint);
        public abstract void OnDisconnected(EndPoint endPoint);
        public abstract int  OnReceive(ArraySegment<byte> buffer);
        public abstract void OnSend(int numOfBytes);

        private void Clear()
        {
            lock(locker)
            {
                sendQueue.Clear();
                pendingList.Clear();
            }
        }

        public void Start(Socket socket)
        {
            this.socket = socket;

            receiveArgs.Completed += OnReceiveCompleted;
            sendArgs.Completed += OnSendCompleted;

            RegisterReceive();
        }

        public void Send(List<ArraySegment<byte>> sendBufferList)
        {
            if (sendBufferList.Count == 0)
                return;

            lock (locker)
            {
                foreach(ArraySegment<byte> sendBuffer in sendBufferList)
                    sendQueue.Enqueue(sendBuffer);

                if (pendingList.Count == 0)
                    RegisterSend();
            }
        }

        public void Send(ArraySegment<byte> sendBuffer)
        {
            lock(locker)
            {
                sendQueue.Enqueue(sendBuffer);

                if(pendingList.Count == 0)
                    RegisterSend();
            }
        }

        public void Disconnect()
        {
            if (Interlocked.Exchange(ref disconnected, 1) == 1)
                return;

            OnDisconnected(socket.RemoteEndPoint);

            // 쫓아내기
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            Clear();
        }

        #region Network Communication

        private  void RegisterSend()
        {
            if (disconnected == 1)
                return;

            while (sendQueue.Count > 0)
            {
                ArraySegment<byte> buffer = sendQueue.Dequeue();
                pendingList.Add(buffer);
            }

            sendArgs.BufferList = pendingList;

            try {
                bool pending = socket.SendAsync(sendArgs);
                if (pending == false)
                    OnSendCompleted(null, sendArgs);
            } catch (Exception e) {
                Console.WriteLine($"RegisterSend Failed {e}");
            }
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock(locker)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        sendArgs.BufferList = null;
                        pendingList.Clear();

                        OnSend(sendArgs.BytesTransferred);

                        if (sendQueue.Count > 0)
                            RegisterSend();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"OnSendCompleted Faild | {e}");
                    }
                }
                else
                {
                    Disconnect();
                }
            }
        }

        private void RegisterReceive()
        {
            if (disconnected == 1)
                return;

            receiveBuffer.Clean();
            ArraySegment<byte> buffer = receiveBuffer.WriteSegment;
            receiveArgs.SetBuffer(buffer.Array, buffer.Offset, buffer.Count);

            try {
                bool pending = socket.ReceiveAsync(receiveArgs);
                if (pending == false)
                    OnReceiveCompleted(null, receiveArgs);
            } catch (Exception e) {
                Console.WriteLine($"RegisterReceive Failed {e}");
            }

        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    // WriteCursor 이동
                    if(receiveBuffer.OnWrite(args.BytesTransferred) == false)
                    {
                        Disconnect();
                        return;
                    }

                    // 컨텐츠 쪽으로 데이터를 넘겨주고 얼마나 처리했는지 받는다
                    int processLength = OnReceive(receiveBuffer.ReadSegment);
                    if(processLength < 0 || receiveBuffer.Size < processLength)
                    {
                        Disconnect();
                        return;
                    }

                    // ReadCursor 이동
                    if(receiveBuffer.OnRead(processLength) == false)
                    {
                        Disconnect();
                        return;
                    }

                    RegisterReceive();
                }
                catch(Exception e)
                {
                    Console.WriteLine($"OnReceiveCompleted Faild | {e}");
                }
            }
            else
            {
                Disconnect();
            }
        }

        #endregion
    }
}
