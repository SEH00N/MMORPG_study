namespace Algorithm
{
    public class Board
    {
        const char CIRCLE = '\u25cf';

        public TileType[,] tile; // 배열
        public int size;
        //public List<int> data2 = new List<int>(); // 동적 배열
        //public LinkedList<int> data3 = new LinkedList<int>(); // 연결 리스트

        public enum TileType
        {
            Empty,
            Wall,
        }

        public void Initialize(int size)
        {
            tile = new TileType[size, size];
            this.size = size;

            for(int y = 0; y < this.size; y++)
            {
                for(int x = 0; x < this.size; x++)
                {
                    if(x == 0 || x == this.size - 1 || y == 0 || y == this.size - 1)
                        tile[y, x] = TileType.Wall;
                    else
                        tile[y, x] = TileType.Empty;
                }
            }
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Console.ForegroundColor = GetTileColor(tile[y, x]);
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
