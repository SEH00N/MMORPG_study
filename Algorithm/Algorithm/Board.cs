namespace Algorithm
{
    public class Board
    {
        const char CIRCLE = '\u25cf';

        public TileType[,] Tile { get; private set; } // 배열
        public int Size { get; private set; }
        
        public int DestX { get; private set; }
        public int DestY { get; private set; }

        private Player player;

        public enum TileType
        {
            Empty,
            Wall
        }

        public void Initialize(int Size, Player player)
        {
            if (Size % 2 == 0)
                return;

            Tile = new TileType[Size, Size];
            this.Size = Size;
            this.player = player;

            DestX = this.Size - 2;
            DestY = this.Size - 2;
            
            //GenerateByBinaryTree();
            GenerateBySideWinder();
        }

        /// <summary>
        /// Side Winder 알고리즘
        /// </summary>
        private void GenerateBySideWinder()
        {
            // 모든 길을 막기
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }

            // 랜덤으로 우측 혹은 하단 길을 뚫기
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                int count = 1;
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    #region 테두리 막기
                    if (y == Size - 2 && x == Size - 2)
                        continue;

                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }
                    #endregion

                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                    else
                    {
                        int randomIndex = rand.Next(0, count);
                        Tile[y + 1, x - randomIndex * 2] = TileType.Empty;

                        count = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Binary Tree 알고리즘
        /// </summary>
        private void GenerateByBinaryTree()
        {
            // 모든 길을 막기
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }

            // 랜덤으로 우측 혹은 하단 길을 뚫기
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    #region 테두리 막기
                    if (y == Size - 2 && x == Size - 2)
                        continue;

                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }
                    #endregion

                    if (rand.Next(0, 2) == 0)
                        Tile[y, x + 1] = TileType.Empty;
                    else
                        Tile[y + 1, x] = TileType.Empty;
                }
            }
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // 플레이어 좌표라면 플레이어 렌더링하기
                    if(y == player.PosY && x == player.PosX)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if(y == DestY && x == DestX)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);

                    Console.Write(CIRCLE);
                }

                Console.WriteLine();
            }

            Console.ForegroundColor = prevColor;
        }

        private ConsoleColor GetTileColor(TileType type)
        {
            switch (type)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.Wall:
                    return ConsoleColor.Red;
            }

            return ConsoleColor.Green;
        }
    }
}
