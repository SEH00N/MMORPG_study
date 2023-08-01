
namespace ServerCore
{
    public struct JobTimerElement : IComparable<JobTimerElement>
    {
        public int execTick;
        public Action action;

        public int CompareTo(JobTimerElement other)
        {
            return other.execTick - execTick;
        }
    }

    public class JobTimer
    {
        private PriorityQueue<JobTimerElement> priorityQueue = new PriorityQueue<JobTimerElement>();
        private object locker = new object();

        public static JobTimer Instance { get; } = new JobTimer();

        public void PusH(Action action, int tickAfter = 0)
        {
            JobTimerElement job;
            job.execTick = Environment.TickCount + tickAfter;
            job.action = action;

            lock (locker)
                priorityQueue.Push(job);
        }

        public void Flush()
        {
            while(true)
            {
                int now = Environment.TickCount;
                JobTimerElement job;

                lock(locker)
                {
                    if (priorityQueue.Count == 0)
                        break;

                    job = priorityQueue.Peek();
                    if (job.execTick > now)
                        break;

                    priorityQueue.Pop();
                }

                job.action?.Invoke();
            }
        }
    }
}
