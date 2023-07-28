namespace ServerCore
{
    public class SpinLock
    {
        private volatile int locked = 0;

        public void Acquire()
        {
            while(true)
            {
                //int original = Interlocked.Exchange(ref locked, 1);
                //if (original == 0)
                //    break;

                // CAS Compare-And-Swap
                int expected = 0;
                int desired = 1;
                if(Interlocked.CompareExchange(ref locked, desired, expected) == expected)
                    break;


                //Thread.Sleep(1); // 무조건 휴식 => 무조건 1ms 정도 휴식
                //Thread.Sleep(0); // 조건부 양보 => 나보다 우선순위가 낮은 애들에게는 양보 불가 => 나보다 우선순위가 같거나 높은 쓰레드가 없으면 복귀
                Thread.Yield();  // 관대한 양보 => 지금 실행이 가능한(실행해야 할) 쓰레드가 있으면 양보 => 실행 가능한 쓰레드가 없으면 남은 시간 소진

                // 쓰레드를 휴식시키고 다른 쓰레드로 변환하는 것에는 Context Switch 라는 비용이 들게 됨
                // 따라서 지금 당장 필요하지 않다고 해서 매번 쓰레드를 바꾸는 것은 마냥 좋은 선택지가 아닐 수 있음
            }
        }

        public void Release()
        {
            locked = 0;
        }
    }

    public class SpinLockTest
    {
        private int num = 0;
        private SpinLock spinLock = new SpinLock();

        private void Thread1()
        {
            for (int i = 0; i < 10000000; i++)
            {
                spinLock.Acquire();
                num++;
                spinLock.Release();
            }
        }

        private void Thread2()
        {
            for (int i = 0; i < 10000000; i++)
            {
                spinLock.Acquire();
                num--;
                spinLock.Release();
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
