package com.company;

public class Consumer implements Runnable {
    private final int itemsToConsume;
    private final Manager manager;
    private final int id;

    public Consumer(int itemsToConsume, int id, Manager manager) {
        this.itemsToConsume = itemsToConsume;
        this.id = id;
        this.manager = manager;
        new Thread(this, "Consumer-" + id).start();
    }

    @Override
    public void run() {
        for (int i = 0; i < itemsToConsume; i++) {
            try {
                manager.empty.acquire();  // чекаю
                manager.access.acquire(); // вхід у критичну секцію (mutex)

                String item = manager.storage.remove(0);
                System.out.printf("[Consumer-%d] Took: %s  | storage: %d%n",
                        id, item, manager.storage.size());

                manager.access.release();
                manager.full.release();   // звільняю місце

                Thread.sleep(800);
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
                break;
            }
        }
        System.out.printf("[Consumer-%d] Done.%n", id);
    }
}