using System;
using System.Threading;

namespace threaddemo
{
    record BreakEntry(long Time, int Index);

    public class BreakThread
    {
        private readonly bool[] flags;
        private readonly Random random = new Random();

        public BreakThread(bool[] flags)
        {
            this.flags = flags;
        }

        public void Run()
        {
            int n = flags.Length;
            BreakEntry[] times = new BreakEntry[n];

            for (int i = 0; i < n; i++)
            {
                times[i] = new BreakEntry(1000 + random.Next(2000), i);
            }

            Array.Sort(times, (a, b) => a.Time.CompareTo(b.Time));

            long elapsed = 0;
            foreach (BreakEntry entry in times)
            {
                Thread.Sleep((int)(entry.Time - elapsed));
                elapsed = entry.Time;
                flags[entry.Index] = true;
            }
        }
    }
}
