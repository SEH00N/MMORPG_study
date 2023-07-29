namespace ServerCore
{
    public class MyInterlocked
    {
        private int number = 0;
        //private object locker = new object();

        private void Thread1()
        {
            for (int i = 0; i < 10000000; i++)
            {
                Interlocked.Increment(ref number);
                //lock (locker)
                //    number++;
            }
        }

        private void Thread2()
        {
            for (int i = 0; i < 10000000; i++)
            {
                Interlocked.Decrement(ref number);
                //lock (locker)
                //    number--;
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
