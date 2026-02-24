using System;
using System.Threading;

namespace threaddemo
{
    class Program
    {
        static void Main(string[] args)
        {
            BreakHandler handler = new BreakHandler();
            
            int cores = Environment.ProcessorCount;

            for (int i = 1; i < cores; i++)
            {
                MainThread mainThread = new MainThread(i, handler);
                Thread thread = new Thread(mainThread.Run);
                thread.Start();
            }

            handler.HoldSleep(15);
        }
    }
}