namespace ServerCore
{
    public class MemoryBarrier
    {
        // 메모리 배리어
        // A) 코드 재배치 억제
        // B) 가시성

        // 메모리 배리어의 종류
        // 1) Full Memory Barrier (어셈블리 => MFENCE, C# => Thread.MemoryBarrier()) : Store (대입) / Load (값 참조) 방지
        // 2) Store Memory Barrier (어셈블리 => SFENCE) : Store 만 방지
        // 3) Load Memory Barrier (어셈블리 => LFENCE) : Load 만 방지

        private int x = 0;
        private int y = 0;
        private int result1 = 0;
        private int result2 = 0;

        private void Thread1()
        {
            y = 1;

            // --------------------------------
            Thread.MemoryBarrier();

            result1 = x;
        }

        private void Thread2()
        {
            x = 1;

            // --------------------------------
            Thread.MemoryBarrier();

            result2 = y;
        }

        public void Main()
        {
            int count = 0;
            while (true)
            {
                count++;
                x = y = result1 = result2 = 0;

                Task t1 = new Task(Thread1);
                Task t2 = new Task(Thread2);

                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                if (result1 == 0 && result2 == 0)
                    break;
            }

            Console.WriteLine($"{count}번 만에 탈출");
        }
    }
}
