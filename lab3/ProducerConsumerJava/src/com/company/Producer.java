package com.company;

public class Producer implements Runnable {
    private final int itemsToProducee;
    private final Manager manager;
    private final int id;

    public Producer(int itemsToproduce, int id, Manager manager) {
        this.itemsToProducee = itemsToproduce;
        this.id = id;
        this.manager = manager;
        new Thread(this, "Producer-" + id).start();
    }

    @Override
    public void run() {
        int produced = 0;
        while (produced < itemsToProducee) {
            if (!manager.registerProduced()) break;
            try {
                manager.full.acquire();
                manager.access.acquire();

                String item = "P" + id + "-item" + produced;
                manager.storage.add(item);
                System.out.printf("[Producer-%d] Added: %s  | storage: %d%n",
                        id, item, manager.storage.size());

                manager.access.release();
                manager.empty.release();
                produced++;

                Thread.sleep(500);
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
                break;
            }
        }
        System.out.printf("[Producer-%d] Done.%n", id);
    }
}