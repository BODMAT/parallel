using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer
{
    public class Manager
    {
        public Semaphore Access;
        public Semaphore Full;
        public Semaphore Empty;

        public List<string> Storage = new List<string>();

        private int totalProduced = 0;
        private readonly int totalItems;
        private readonly object lockObj = new object();

        public Manager(int storageSize, int totalItems)
        {
            this.totalItems = totalItems;
            Access = new Semaphore(1, 1);
            Full   = new Semaphore(storageSize, storageSize);
            Empty  = new Semaphore(0, storageSize);
        }

        public bool RegisterProduced()
        {
            lock (lockObj)
            {
                if (totalProduced < totalItems)
                {
                    totalProduced++;
                    return true;
                }
                return false;
            }
        }
    }
}