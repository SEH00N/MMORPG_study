using System.Threading;
using System;

namespace ServerCore
{
    // SpinLock -> 프로그램에서 작동하는 락
    // Event를 활용한 Lock, Mutex -> 커널 (하드웨어) 단위에서 작동하는 락
    public class MyLock
    {
        private AutoResetEvent available = new AutoResetEvent(true);
        //private ManualResetEvent available = new ManualResetEvent(true);

        public void Acquire()
        {
            available.WaitOne(); // 입장 시도
            //available.Reset(); // 문 닫기
        }

        public void Release()
        {
            available.Set(); // flag = true (문 열기)
        }
    }

    public class MyLockTest
    {
        private int num = 0;
        private MyLock myLock = new MyLock();

        private void Thread1()
        {
            for (int i = 0; i < 10000; i++)
            {
                myLock.Acquire();
                num++;
                myLock.Release();
            }
        }

        private void Thread2()
        {
            for (int i = 0; i < 10000; i++)
            {
                myLock.Acquire();
                num--;
                myLock.Release();
            }
        }

        public void Main()
        {
            Task t1 = new Task(Thread1);
            Task t2 = new Task(Thread2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(num);
        }
    }
}
