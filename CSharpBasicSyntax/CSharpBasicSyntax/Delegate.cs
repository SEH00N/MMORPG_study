namespace CSharpBasicSyntax
{
    class InputManager
    {
        public delegate void OnInputKey();
        public event OnInputKey InputKey;

        public void Update()
        {
            if (Console.KeyAvailable == false)
                return;

            ConsoleKeyInfo info = Console.ReadKey();
            if(info.Key == ConsoleKey.A)
                InputKey();
        }
    }

    internal class Delegate
    {
        delegate int OnClicked();

        static void ButtonPressed(OnClicked clickedEvent)
        {
            clickedEvent?.Invoke();
        }

        static int TestDelegate()
        {
            Console.WriteLine("Hello, Delegate!");
            return 0;
        }

        static int TestDelegate2()
        {
            Console.WriteLine("Hello, Delegate2!");
            return 0;
        }

        static void OnInputTest()
        {
            Console.WriteLine("Input Received!");
        }

        //static void Main(string[] args)
        //{
        //    //OnClicked clicked = new OnClicked(TestDelegate);
        //    //clicked += TestDelegate2;

        //    //ButtonPressed(clicked);

        //    InputManager inputManager = new InputManager();
        //    inputManager.InputKey += OnInputTest;

        //    while (true)
        //        inputManager.Update();
        //}
    }
}
