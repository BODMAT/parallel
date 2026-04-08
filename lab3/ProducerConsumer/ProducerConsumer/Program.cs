using System;

namespace ProducerConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            int storageSize  = 5;
            int totalItems   = 20;
            int numProducers = 3;
            int numConsumers = 2;

            Manager manager = new Manager(storageSize, totalItems);

            Console.WriteLine($"Storage: {storageSize} | Total items: {totalItems} | " +
                              $"Producers: {numProducers} | Consumers: {numConsumers}\n");

            // Розподіл із залишком
            for (int i = 1; i <= numProducers; i++)
            {
                int items = totalItems / numProducers + (i <= totalItems % numProducers ? 1 : 0);
                new Producer(items, i, manager);
            }

            for (int i = 1; i <= numConsumers; i++)
            {
                int items = totalItems / numConsumers + (i <= totalItems % numConsumers ? 1 : 0);
                new Consumer(items, i, manager);
            }

            Console.ReadKey();
        }
    }
}