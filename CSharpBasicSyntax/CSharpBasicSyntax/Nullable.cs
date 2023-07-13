namespace CSharpBasicSyntax
{
    internal class Nullable
    { 
        static void Main(string[] args)
        {
            int? number = 5;
            int b = number ?? 0;
            Console.WriteLine(b);

            if(number != null)
            {
                int a = number.Value;
                Console.WriteLine(a);
            }

            if(number.HasValue)
            {
                int a = number.Value;
                Console.WriteLine(a);
            }

        }
    }
}
