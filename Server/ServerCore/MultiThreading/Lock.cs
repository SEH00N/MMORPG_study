using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Lock
    {
        private int number = 0;
        private object locker = new object();

        private void Thread1()
        {
            for (int i = 0; i < 10000000; i++)
            {
                // 상호배제 Mutual Exclusive
                lock (locker)
                {
                    number++;
                }

                //Monitor.Enter(locker); // 똥칸 점령

                //number++;

                //Monitor.Exit(locker); // 똥칸 점령 해제
            }
        }

        private void Thread2()
        {
            for (int i = 0; i < 10000000; i++)
            {
                Monitor.Enter(locker);

                number--;

                Monitor.Exit(locker);
            }
        }

        public void Main()
        {
            Task t1 = new Task(Thread1);
            Task t2 = new Task(Thread2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(number);
        }
    }
}
