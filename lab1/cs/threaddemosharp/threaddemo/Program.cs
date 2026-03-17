using System;
using System.Threading;

namespace threaddemo
{
    class Program
    {
        static void Main(string[] args)
        {
            int cores = Environment.ProcessorCount;
            bool[] flags = new bool[cores];

            for (int i = 0; i < cores; i++)
            {
                int id = i + 1, idx = i;
                MainThread worker = new MainThread(id, flags, idx);
                new Thread(worker.Run).Start();
            }

            new Thread(new BreakThread(flags).Run).Start();
        }
    }
}
