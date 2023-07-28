using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class SessionManager
    {
        private static object locker = new object();

        public static void Test()
        {
            lock(locker)
            {
                UserManager.TestUser();
            }
        }

        public static void TestSession()
        {
            lock (locker)
            {

            }
        }
    }

    public class UserManager
    {
        private static object locker = new object();

        public static void Test()
        {
            lock (locker)
            {

            }
        }

        public static void TestUser()
        {
            lock (locker)
            {
                SessionManager.TestSession();
            }
        }
    }


    public class DeadLock
    {
        private void Thread1()
        {
            for (int i = 0; i < 100; i++)
                SessionManager.Test();
        }
        
        private void Thread2()
        {
            for (int i = 0; i < 100; i++)
                UserManager.Test();
        }

        public void Main()
        {
            Task t1 = new Task(Thread1);
            Task t2 = new Task(Thread2);

            t1.Start();

            Thread.Sleep(100);

            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine("Hello, World!");
        }
    }
}
