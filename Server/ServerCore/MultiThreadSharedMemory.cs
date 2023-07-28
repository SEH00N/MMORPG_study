using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class MultiThreadSharedMemory
    {
        volatile bool stop = false;

        private void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작!");

            while (stop == false)
            {

            }

            Console.WriteLine("쓰레드 종료!");
        }

        public void Main()
        {
            Task task = new Task(ThreadMain);
            task.Start();

            Thread.Sleep(1000);

            stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");

            task.Wait();

            Console.WriteLine("종료 성공");
        }
    }
}
