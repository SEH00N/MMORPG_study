namespace ServerCore
{
    public class Program
    {
        volatile static bool stop = false;

        private static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작!");

            while (stop == false)
            {

            }

            Console.WriteLine("쓰레드 종료!");
        }

        static void Main(string[] args)
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