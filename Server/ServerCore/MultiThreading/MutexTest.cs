namespace ServerCore
{
    public class MutexTest
    {
        private int num = 0;

        // AutoResetEvent, ManualResetEvent 보다 훨씬 많은 정보
        // Mutex 또한 커널 단위에서 이루어지는 락
        private Mutex mutex = new Mutex();

        private void Thread1()
        {
            for (int i = 0; i < 10000; i++)
            {
                mutex.WaitOne();
                num++;
                mutex.ReleaseMutex();
            }
        }

        private void Thread2()
        {
            for (int i = 0; i < 10000; i++)
            {
                mutex.WaitOne();
                num--;
                mutex.ReleaseMutex();
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
