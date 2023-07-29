
namespace ServerCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            ReaderWriterLockTest locker = new ReaderWriterLockTest();
            locker.Main();
        }
    }
}