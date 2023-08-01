using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public interface IJobQueue
    {
        public void Push(Action job);
    }

    public class JobQueue : IJobQueue
    {
        private object locker = new object();
        private Queue<Action> jobQueue = new Queue<Action>();
        private bool flush = false;

        public void Push(Action job)
        {
            bool flush = false;
            lock (locker)
            {
                jobQueue.Enqueue(job);

                if (this.flush == false)
                    flush = this.flush = true;
            }

            if (flush)
                Flush();
        }

        private void Flush()
        {
            while(true)
            {
                Action action = Pop();
                if (action == null)
                    return;

                action?.Invoke();
            }
        }

        private Action Pop()
        {
            lock(locker)
            {
                if (jobQueue.Count == 0)
                {
                    flush = false;
                    return null;
                }

                return jobQueue.Dequeue();
            }
        }
    }

}
