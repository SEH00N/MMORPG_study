namespace Algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            const int WAIT_TICK = 1000 / 30;
            int lastTick = 0;

            Board board = new Board();
            board.Initialize(25);

            while (true)
            {
                #region 프레임 관리
                int currentTick = System.Environment.TickCount;

                if (currentTick - lastTick < WAIT_TICK)
                    continue;

                lastTick = currentTick;
                #endregion

                Console.SetCursorPosition(0, 0);
                board.Render();
            }
        }
    }
}