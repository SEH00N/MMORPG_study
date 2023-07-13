namespace CSharpBasicSyntax
{
    internal class _2DArray
    {
        class Map
        {
            int[,] tiles = {
                { 1, 1, 1, 1, 1 },
                { 1, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 1 },
                { 1, 1, 1, 1, 1 },
            };

            public void Render()
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                for(int y = 0; y < tiles.GetLength(1); y++)
                {
                    for (int x = 0; x < tiles.GetLength(0); x++)
                    {
                        Console.ForegroundColor = (tiles[y, x] == 1) ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.Write('\u25cf');
                    }
                    Console.WriteLine();
                }

                Console.ForegroundColor = defaultColor;
            }
        }

        static void Main(string[] args)
        {
            int[,] arr = new int[2, 3] {
                { 1, 2, 3 },
                { 1, 2, 3 }
            };
            arr[0, 0] = 1;
            arr[1, 0] = 1;

            Map map = new Map();
            map.Render();

            // [. . .]
            // [. . .]

        }
    }
}
