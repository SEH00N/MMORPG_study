namespace ServerCore
{
    public class MultiThreading
    {
        // Task         =>  비동기 형태로 동작하는 멀티 스레딩 async와 await 생각하면 됨
        //                  근데 내부적으론 ThreadPool로 구현되어있음
        //                  다만 옵션을 설정하여 병목현상을 예방할 수 있도록 구현되어 있음

        // Thread       =>  CPU에 말 그대로 스레드를 추가하는 방식
        //                  근본 방식

        // ThreadPool   =>  스레드에 동력을 공급해서 일을 시키는 놈이 코어라고 하는 놈인데
        //                  스레드 수가 코어 수보다 많아지면 속도 향상을 바라기는 힘듦
        //                  그래서 c#에서 스레드를 일정 개수만큼 미리 대기 시켜두고 필요할 때 풀링해서 쓰는 방식
        //                  들어 온 작업을 대기시켜둔 스레드들한테 작업을 할당해서 처리하는 방식이기 때문에
        //                  너무 오래 걸리는 작업(while loop 같은 것)을 ThreadPool로 처리하면 병목현상이 일어날 수 있음
        //                  따라서 가벼운 작업을 할 때 ThreadPool을 사용하고 오래걸리는 작업을 할 땐 Thread를 사용하는 걸 권장함

        private void MainThread(object state)
        {
            while (true)
                Console.WriteLine("Hello, Thread!");
        }

        public void Main()
        {
            Task task = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning);
            task.Start();

            #region ThreadPool 병목현상 발생시키기

            //ThreadPool.SetMinThreads(1, 1);
            //ThreadPool.SetMaxThreads(5, 5);

            //for(int i = 0; i < 5; i++)
            //    ThreadPool.QueueUserWorkItem((obj) => { while (true) { } });

            //ThreadPool.QueueUserWorkItem(MainThread);

            #endregion

            #region 멀티스레딩

            //Thread thread = new Thread(MainThread);
            //thread.Name = "Test Thread";
            //thread.IsBackground = true;
            //thread.Start();

            //Console.WriteLine("Waiting For Thread");

            //thread.Join();
            //Console.WriteLine("Hello, World!");

            #endregion

            while (true)
            {

            }
        }
    }
}
