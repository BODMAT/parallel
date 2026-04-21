using System;
using System.Threading;

namespace ProducerConsumer
{
    public class Consumer
    {
        private readonly int itemsToConsume;
        private readonly int id;
        private readonly Manager manager;

        public Consumer(int itemsToConsume, int id, Manager manager)
        {
            this.itemsToConsume = itemsToConsume;
            this.id = id;
            this.manager = manager;

            Thread thread = new Thread(Run);
            thread.Name = "Consumer-" + id;
            thread.Start();
        }

        private void Run()
        {
            for (int i = 0; i < itemsToConsume; i++)
            {
                manager.Empty.WaitOne(); 
                manager.Access.WaitOne();

                string item = manager.Storage[0];
                manager.Storage.RemoveAt(0);
                Console.WriteLine($"[Consumer-{id}] Took: {item}  | storage: {manager.Storage.Count}");

                manager.Access.Release();
                manager.Full.Release();

                Thread.Sleep(800);
            }
            Console.WriteLine($"[Consumer-{id}] Done.");
        }
    }
}