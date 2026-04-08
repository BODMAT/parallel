package com.company;

public class Main {

    public static void main(String[] args) throws InterruptedException {
        int storageSize  = 5;
        int totalItems   = 20;
        int numProducers = 3;
        int numConsumers = 2;

        Manager manager = new Manager(storageSize, totalItems);

        System.out.printf("Storage: %d | Total items: %d | Producers: %d | Consumers: %d%n%n",
                storageSize, totalItems, numProducers, numConsumers);

        // Розподіл із залишком — перші отримують +1
        for (int i = 1; i <= numProducers; i++) {
            int items = totalItems / numProducers + (i <= totalItems % numProducers ? 1 : 0);
            new Producer(items, i, manager);
        }

        for (int i = 1; i <= numConsumers; i++) {
            int items = totalItems / numConsumers + (i <= totalItems % numConsumers ? 1 : 0);
            new Consumer(items, i, manager);
        }
    }
}