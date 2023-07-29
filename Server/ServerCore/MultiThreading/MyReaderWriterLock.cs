using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    // 재귀적 락 허용 (Yes) WriteLock -> WriteLock OK, WritLock -> ReadLock OK, ReadLock -> WriteLock NO
    // 스핀락 정책 (5000번 -> Yield)
    internal class MyReaderWriterLock
    {
        private const int EMPTY_FLAG = 0x00000000;
        private const int WRITE_MASK = 0x7FFF0000;
        private const int READ_MASK = 0x0000FFFF;
        private const int MAX_SPIN_COUNT = 5000;

        // [Unused(1)] [WriteThreadID(15)] [ReadCount(16)]
        private int flag = EMPTY_FLAG;
        private int writeCount = 0;

        public void WriteLock()
        {
            // 동일 쓰레드가 WriteLock을 이미 획득하고 있는지 확인
            int lockThreadID = (flag & WRITE_MASK) >> 16;
            if(Thread.CurrentThread.ManagedThreadId == lockThreadID)
            {
                writeCount++;
                return;
            }

            // 아무도 Write or Read Lock을 획득하고 있지 않을 때 소유권 획득
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
            while(true)
            {
                for(int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    if (Interlocked.CompareExchange(ref flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                    {
                        writeCount = 1;
                        return;
                    }
                }

                Thread.Yield();
            }
        }

        public void WriteUnlock()
        {
            int lockCount = --writeCount;
            if(lockCount == 0)
                Interlocked.Exchange(ref flag, EMPTY_FLAG);
        }

        public void ReadLock()
        {
            // 동일 쓰레드가 WriteLock을 이미 획득하고 있는지 확인
            int lockThreadID = (flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadID)
            {
                Interlocked.Increment(ref flag);
                return;
            }

            // 아무도 WriteLock을 회득하고 있지 않으면 ReadCount를 1 늘린다
            while (true)
            {
                for(int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    // read mask 부분 확보
                    int expected = (flag & READ_MASK);
                    // read mask 부분이 잘 확보 되었다 => flag가 read mask 부분과 같다
                    // 따라서 read mask 를 확보 후 flag가 read mask부분과 같으면 읽을 수 있는 상태로 판별
                    // exprected++ 로 read mask 부분 증가
                    if (Interlocked.CompareExchange(ref flag, expected + 1, expected) == expected)
                        return;
                }

                Thread.Yield();
            }
        }

        public void ReadUnlock()
        {
            Interlocked.Decrement(ref flag);
        }
    }

    public class ReaderWriterLockTest
    {
        private volatile int count = 0;
        private MyReaderWriterLock locker = new MyReaderWriterLock();

        public void Main()
        {
            Task t1 = new Task(() => {
                for (int i = 0; i < 100000; i++)
                {
                    locker.WriteLock();
                    count++;
                    locker.WriteUnlock();
                }
            });

            Task t2 = new Task(() => {
                for (int i = 0; i < 100000; i++)
                {
                    locker.WriteLock();
                    count--;
                    locker.WriteUnlock();
                }
            });

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(count);
        }
    }
}
