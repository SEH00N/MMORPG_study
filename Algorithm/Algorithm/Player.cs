using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
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

            AStarInit();
            //BFSInit();
            //RightHandInit();
        }

        const int MOVE_TICK = 50;
        private int sumTick = 0;
        private int lastIndex = 0;

        public void Update(int deltaTick)
        {
            if(lastIndex >= points.Count)
            {
                lastIndex = 0;
                points.Clear();

                board.Initialize(board.Size, this);
                Initialize(1, 1, board);

                Thread.Sleep(500);
            }

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

        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                    return 0;
                return F < other.F ? 1 : -1;    
            }
        }

        private void AStarInit()
        {
            // U L D R UL DL DR UR
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };
            int[] cost = new int[] { 10, 10, 10, 10 };

            // 점수 매기기
            // F = G + H
            // F = 최종 비용
            // G = 시작점에서 해당 좌표까지 이동하는데 드는 비용
            // H = 목적지에서 얼마나 가까운지

            // (y, x) 이미 방문헀는지 여부
            bool[,] closed = new bool[board.Size, board.Size]; // ClosedList

            // (y, x) 가는 길을 한 번이라도 발견헀는지
            // 미발견 상태 => MaxValue
            // 발견 상태 => F = G + H
            int[,] open = new int[board.Size, board.Size]; // OpenList
            for (int y = 0; y < board.Size; y++)
                for (int x = 0; x < board.Size; x++)
                    open[y, x] = Int32.MaxValue;

            Pos[,] parent = new Pos[board.Size, board.Size]; 

            // 오픈 리스트에 있는 정보들 중에서 가장 좋은 후보를 바르게 뽑아오기 위한 도구
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

            // 시작점 발견 (예약 진행)
            open[PosY, PosX] = Math.Abs(board.DestY - PosY) + Math.Abs(board.DestX - PosX);
            pq.Push(new PQNode() { F = Math.Abs(board.DestY - PosY) + Math.Abs(board.DestX - PosX), G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosX, PosY);

            while (pq.Count > 0)
            {
                // 제일 좋은 후보를 찾는다
                PQNode node = pq.Pop();

                // 동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문(Closed)된 경우 스킵
                if (closed[node.Y, node.X])
                    continue;

                // 방문한다
                closed[node.Y, node.X] = true;

                // 목적지에 도착했으면 종료
                if (node.Y == board.DestY && node.X == board.DestX)
                    break;

                // 상하좌우 등 이동할 수 있는 좌표인지 확인해서 예약한다

                for (int i = 0; i < deltaY.Length; i++)
                {
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    // 유효범위를 벗어났으면 스킵
                    if (nextX < 0 || nextX >= board.Size || nextY < 0 || nextY >= board.Size)
                        continue;

                    // 벽으로 막혀서 갈 수 없으면 스킵
                    if (board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;

                    // 이미 방문한 곳이면 스킵
                    if (closed[nextY, nextX])
                        continue;

                    // 비용계산
                    int g = node.G + cost[i];
                    int h = Math.Abs(board.DestY - nextY) + Math.Abs(board.DestX - nextX);

                    // 다른 경로에서 더 빠른 길을 이미 찾았으면 스킵
                    if (open[nextY, nextX] < g + h)
                        continue;

                    // 예약 진행
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, X = nextX, Y = nextY });
                    parent[nextY, nextX] = new Pos(node.X, node.Y);
                }
            }
            
            CalcPathFromParent(parent);
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

            CalcPathFromParent(parent);
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

        private void CalcPathFromParent(Pos[,] parent)
        {
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
