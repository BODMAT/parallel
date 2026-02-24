using System;

namespace threaddemo
{
    public class MainThread
    {
        private readonly int id;
        private readonly BreakHandler handler;

        public MainThread(int id, BreakHandler handler)
        {
            this.id = id;
            this.handler = handler;
        }

        public void Run()
        {
            long sum = 0;
            long step = 0;
            long stepVal = 2;
            long count = 0;

            while (!handler.IsCanBreak)
            {
                step += stepVal;
                sum += step;
                count++;
            }

            Console.WriteLine($"Потік {id}: Сума = {sum}, Кількість ітерацій = {count}");
        }
    }
}