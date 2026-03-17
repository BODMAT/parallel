using System;
using System.Threading;

namespace ThreadSumSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int cores = Environment.ProcessorCount;
            const int DIM = 1_000_000_000;
            int[] threadCounts = { 4, cores };

            Console.WriteLine($"Доступних процесорів (ядер): {cores}");
            Console.WriteLine($"Розмір масиву: {DIM}");
            Console.WriteLine(new string('=', 60));

            foreach (int threadNum in threadCounts)
            {
                Console.WriteLine($"\n>>> Запуск з кількістю потоків: {threadNum}");

                ArrClass arrClass = new ArrClass(DIM, threadNum);

                long startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                int[] result = arrClass.ThreadMin();
                long elapsed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startTime;

                Console.WriteLine($"Мінімальний елемент: {result[0]}, індекс: {result[1]}");
                Console.WriteLine($"Час виконання: {elapsed} мс");
                Console.WriteLine(new string('-', 60));
            }

            Console.ReadKey();
        }
    }
}
