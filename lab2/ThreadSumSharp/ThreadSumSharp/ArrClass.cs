using System;
using System.Threading;

namespace ThreadSumSharp
{
    public class ArrClass
    {
        private readonly int dim;
        private readonly int threadNum;
        public readonly int[] arr;

        private int globalMin = int.MaxValue;
        private int globalMinIndex = -1;
        private int threadCount = 0;

        private readonly object lockerForMin = new object();
        private readonly object lockerForCount = new object();

        public ArrClass(int dim, int threadNum)
        {
            this.dim = dim;
            this.threadNum = threadNum;
            arr = new int[dim];
            Random rand = new Random();

            for (int i = 0; i < dim; i++)
                arr[i] = rand.Next(1, 1_000_001);

            //!
            int negIndex = rand.Next(dim);
            arr[negIndex] = -rand.Next(1, 1_000_001);
            Console.WriteLine($"Вставлено від'ємне значення: {arr[negIndex]} за індексом: {negIndex}");
        }

        public int[] PartMin(int startIndex, int finishIndex)
        {
            int localMin = arr[startIndex];
            int localMinIndex = startIndex;

            for (int i = startIndex + 1; i < finishIndex; i++)
            {
                //! Симуляція для наглядності
                double unused = Math.Sqrt(Math.Abs(arr[i])) * Math.Sin(arr[i]);

                if (arr[i] < localMin)
                {
                    localMin = arr[i];
                    localMinIndex = i;
                }
            }
            return new int[] { localMin, localMinIndex };
        }

        // Критична секція
        public void CollectMin(int localMin, int localMinIndex)
        {
            lock (lockerForMin)
            {
                if (localMin < globalMin)
                {
                    globalMin = localMin;
                    globalMinIndex = localMinIndex;
                }
            }
        }

        public void IncThreadCount()
        {
            lock (lockerForCount)
            {
                threadCount++;
                Monitor.Pulse(lockerForCount); // аналог notify
            }
        }

        private int[] GetResult()
        {
            lock (lockerForCount)
            {
                while (threadCount < threadNum)
                {
                    Monitor.Wait(lockerForCount); // аналог wait
                }
            }
            return new int[] { globalMin, globalMinIndex };
        }

        public int[] ThreadMin()
        {
            Thread[] threads = new Thread[threadNum];
            int chunkSize = dim / threadNum;

            for (int i = 0; i < threadNum; i++)
            {
                int start = i * chunkSize;
                int end = (i == threadNum - 1) ? dim : start + chunkSize;
                threads[i] = new Thread(new ParameterizedThreadStart(StarterThread));
                threads[i].Start(new Bound(i, start, end));
            }

            return GetResult();
        }

        private void StarterThread(object? param)
        {
            if (param is Bound b)
            {
                Console.WriteLine($"Потік #{b.ThreadId} | Ім'я: {Thread.CurrentThread.ManagedThreadId,-5} | Діапазон: [{b.StartIndex}, {b.FinishIndex})");

                int[] result = PartMin(b.StartIndex, b.FinishIndex);

                Console.WriteLine($"Потік #{b.ThreadId} | Локальний мін: {result[0]} за індексом: {result[1]}");

                CollectMin(result[0], result[1]);
                IncThreadCount();
            }
        }
    }
}
