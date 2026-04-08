using System;
using System.Threading;

namespace ProducerConsumer
{
    public class Producer
    {
        private readonly int itemsToProduce;
        private readonly int id;
        private readonly Manager manager;

        public Producer(int itemsToProduce, int id, Manager manager)
        {
            this.itemsToProduce = itemsToProduce;
            this.id = id;
            this.manager = manager;

            Thread thread = new Thread(Run);
            thread.Name = "Producer-" + id;
            thread.Start();
        }

        private void Run()
        {
            int produced = 0;
            while (produced < itemsToProduce)
            {
                if (!manager.RegisterProduced()) break; // глобальний ліміт вичерпано

                manager.Full.WaitOne();   // чекаю
                manager.Access.WaitOne(); // взаємне виключення

                string item = $"P{id}-item{produced}";
                manager.Storage.Add(item);
                Console.WriteLine($"[Producer-{id}] Added: {item}  | storage: {manager.Storage.Count}");

                manager.Access.Release();
                manager.Empty.Release();  // сигнал споживачу

                produced++;
                Thread.Sleep(500);
            }
            Console.WriteLine($"[Producer-{id}] Done.");
        }
    }
}