using System.Runtime.InteropServices;

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

            //BFSInit();
            //RightHandInit();
        }

        const int MOVE_TICK = 10;
        private int sumTick = 0;
        private int lastIndex = 0;

        public void Update(int deltaTick)
        {
            sumTick += deltaTick;

            if (sumTick >= MOVE_TICK)
            {
                sumTick = 0;

                if (lastIndex >= points.Count)
                    return;

                PosY = points[lastIndex].Y;
                PosX = points[lastIndex].X;
                lastIndex++;
                //RandomMove();
            }
        }

        private void BFSInit()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };

            bool[,] found = new bool[board.Size, board.Size];
            Pos[,] parent = new Pos[board.Size, board.Size];

            Queue<Pos> q = new Queue<Pos>();
            q.Enqueue(new Pos(PosX, PosY));

            found[PosY, PosX] = true;
            parent[PosY, PosX] = new Pos(PosX, PosY);

            while(q.Count > 0)
            {
                Pos pos = q.Dequeue();
                int nowY = pos.Y;
                int nowX = pos.X;

                for(int i = 0; i < 4; i++)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];

                    if (nextX < 0 || nextX >= board.Size || nextY < 0 || nextY >= board.Size)
                        continue;
                    if (board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    if (found[nextY, nextX])
                        continue;

                    q.Enqueue(new Pos(nextX, nextY));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowX, nowY);
                }
            }

            int x = board.DestX;
            int y = board.DestY;

            while (parent[y, x].X != x || parent[y, x].Y != y)
            {
                points.Add(new Pos(x, y));
                Pos pos = parent[y, x];

                x = pos.X;
                y = pos.Y;
            }

            points.Add(new Pos(x, y));
            points.Reverse();
        }
        private void RightHandInit()
        {
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
                else if (board.Tile[PosY + frontY[direction], PosX + frontX[direction]] == Board.TileType.Empty) // 현재 방향으로 전진할 수 있다면
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
