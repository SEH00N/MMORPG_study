namespace CSharpBasicSyntax
{
    class Function
    {
        static int Add(int a, int b)
        {
            int result = a + b;
            return result;
        }

        static int Add(int a, int b, int c = 0)
        {
            int result = a + b + c;
            return result;
        }

        static float Add(float a, float b)
        {
            float result = a + b;
            return result;
        }

        static void AddOne(ref int a)
        {
            a = a + 1;
        }

        static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        static void Divide(int a, int b, out int result)
        {
            result = a / b;
        }

        static int Factorial(int n)
        {
            //int result = 1;
            //for(int i = 2; i < n + 1; i++)
            //    result *= i;

            //return result;

            if (n <= 2)
                return n;

            return n * Factorial(n - 1);
        }

        //static void Main(string[] args)
        //{
        //    //int result = Function.Add(4, 5);
        //    //Console.WriteLine(result);

        //    //int a = 0;
        //    //Function.AddOne(ref a);
        //    //Console.WriteLine(a);

        //    //int a = 3, b = 5;
        //    //Swap(ref a, ref b);
        //    //Console.WriteLine($"a: {a}, b: {b}");

        //    //int a = 6, b = 3;
        //    //Divide(a, b, out int result);
        //    //Console.WriteLine(result);

        //    //float result = Add(1f, 3f);
        //    //Console.WriteLine(result);

        //    //for(int i = 2; i < 10; i++)
        //    //    for (int j = 1; j < 10; j++)
        //    //        Console.WriteLine($"{i} x {j} = {i*j}");

        //    //for(int i = 0; i < 5; i++)
        //    //{
        //    //    for(int j = 0; j < i + 1; j++)
        //    //        Console.Write('*');

        //    //    Console.WriteLine();
        //    //}

        //    Console.WriteLine(Factorial(3));
        //}
    }
}