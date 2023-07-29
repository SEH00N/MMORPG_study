using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class ThreadLocalStorage
    {
        static ThreadLocal<string> ThreadName = new ThreadLocal<string>(() => {
            return $"My Name Is {Thread.CurrentThread.ManagedThreadId}";
        });

        static void WhoAmI()
        {
            bool repeat = ThreadName.IsValueCreated;
            if (repeat)
                Console.WriteLine(ThreadName.Value + "(Repeat)");
            else
                Console.WriteLine(ThreadName.Value);
        }


        public static void MainThread()
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);
        }
    }
}
