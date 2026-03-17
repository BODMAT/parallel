using System;
using System.Threading;

namespace threaddemo
{
    public class MainThread
    {
        private readonly int id;
        private readonly bool[] flags;
        private readonly int index;

        public MainThread(int id, bool[] flags, int index)
        {
            this.id = id;
            this.flags = flags;
            this.index = index;
        }

        public void Run()
        {
            long sum = 0, current = 0, step = id, count = 0;
            DateTime startTime = DateTime.Now;

            while (!flags[index])
            {
                sum += current;
                current += step;
                count++;
            }

            double elapsed = (DateTime.Now - startTime).TotalMilliseconds;
            Console.WriteLine($"Thread {id} | Sum: {sum} | Count: {count} | Time: {elapsed:F0}ms");
        }
    }
}
