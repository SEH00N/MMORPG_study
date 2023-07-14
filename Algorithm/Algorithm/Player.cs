namespace Algorithm
{
    public class Pos
    {
        public Pos(int x, int y) { X = x; Y = y; }
        public int X;
        public int Y;
    }

    public class Player
    {
        public int PosX { get; private set; }
        public int PosY { get; private set; }

        private Board board = null;
        private Random random = new Random();

        public enum Direction
        { 
            Up,
            Left,
            Down,
            Right
        }

        private int direction = (int)Direction.Up;
        private List<Pos> points = new List<Pos>();

        public void Initialize(int posX, int posY, Board board)
        {
            PosX = posX;
            PosY = posY;
            this.board = board;

            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };
            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

            points.Add(new Pos(PosX, PosY));
            while (PosY != board.DestY || PosX != board.DestX)
            {
                // 우수법
                // 1. 현재 바라보는 방향을 기준으로 오른쪽으로 갈 수 있는지 확인.
                // 2 - 1. 오른쪽으로 갈 수 있다면 오른쪽으로 90도 회전 후 한 보 전진
                // 2 - 2. 오른쪽으로 갈 수 없다면 현재 방향으로 전진할 수 있는지 확인 후 한 보 전진
                // 3. 둘 다 안되면 왼쪽으로 90도 회전

                if (board.Tile[PosY + rightY[direction], PosX + rightX[direction]] == Board.TileType.Empty) // 오른쪽으로 갈 수 있다면
                {
                    // 오른쪽으로 90도 회전 후
                    direction = (direction - 1 + 4) % 4;

                    // 한 보 전진
                    PosX += frontX[direction];
                    PosY += frontY[direction];

                    points.Add(new Pos(PosX, PosY));
                }
                else if(board.Tile[PosY + frontY[direction], PosX + frontX[direction]] == Board.TileType.Empty) // 현재 방향으로 전진할 수 있다면
                {
                    // 앞으로 한 보 전진
                    PosX += frontX[direction];
                    PosY += frontY[direction];

                    points.Add(new Pos(PosX, PosY));
                }
                else // 둘 다 안되면
                {
                    // 왼쪽으로 90도 회전
                    direction = (direction + 1 + 4) % 4;
                }
            }
        }

        const int MOVE_TICK = 10;
        private int sumTick = 0;

        public void Update(int deltaTick)
        {
            sumTick += deltaTick;

            if (sumTick >= MOVE_TICK)
            {
                sumTick = 0;

                RightHandMove();
                //RandomMove();
            }
        }

        private int lastIndex = 0;
        private void RightHandMove()
        {
            if (lastIndex >= points.Count)
                return;

            PosY = points[lastIndex].Y;
            PosX = points[lastIndex].X;
            lastIndex++;
        }

        private void RandomMove()
        {
            int randValue = random.Next(0, 5);
            switch (randValue)
            {
                case 0:
                    if (PosY - 1 >= 0 && board.Tile[PosY - 1, PosX] == Board.TileType.Empty)
                        PosY--;
                    break;
                case 1:
                    if (PosY + 1 < board.Size && board.Tile[PosY + 1, PosX] == Board.TileType.Empty)
                        PosY++;
                    break;
                case 2:
                    if (PosX - 1 >= 0 && board.Tile[PosY, PosX - 1] == Board.TileType.Empty)
                        PosX--;
                    break;
                case 3:
                    if (PosX + 1 < board.Size && board.Tile[PosY, PosX + 1] == Board.TileType.Empty)
                        PosX++;
                    break;
            }
        }
    }
}
