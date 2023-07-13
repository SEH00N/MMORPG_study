namespace CSharpBasicSyntax
{
    internal class Array
    {
        static int GetHighestScore(int[] scores)
        {
            int result = 0;
            for(int i = 1; i < scores.Length; i++)
            {
                if (scores[i] > scores[result])
                    result = i;
            }

            return scores[result];
        }

        static int GetAverageScore(int[] scores)
        {
            if (scores.Length == 0)
                return -1;

            int result = 0;
            for (int i = 0; i < scores.Length; i++)
                result += scores[i];


            result = result / scores.Length;

            return result;
        }

        static int GetIndexOf(int[] scores, int value)
        {
            for (int i = 0; i < scores.Length; i++)
                if (scores[i] == value)
                    return i;
            
            return -1;
        }

        static void Sort(int[] scores)
        {
            for(int i = 0; i < scores.Length; i++)
            {
                for(int j = i; j < scores.Length - i - 1; j++)
                {
                    if (scores[j] > scores[j + 1])
                    {
                        int temp = scores[j];
                        scores[j] = scores[j + 1];
                        scores[j + 1] = temp;
                    }
                }
            }
        }

        //static void Main(string[] args)
        //{
        //    int[] scores = new int[5] { 10, 30, 40, 20, 50 };

        //    Console.WriteLine(GetHighestScore(scores));
        //    Console.WriteLine(GetAverageScore(scores));
        //    Console.WriteLine(GetIndexOf(scores, 20));

        //    Sort(scores);
        //    for(int i = 0; i < scores.Length; i++)
        //        Console.WriteLine(scores[i]);
        //}
    }
}
