package com.company;

import java.util.ArrayList;
import java.util.concurrent.Semaphore;

public class Manager {
    public final Semaphore access;
    public final Semaphore full;
    public final Semaphore empty;

    public final ArrayList<String> storage = new ArrayList<>();
    private int totalProduced = 0;
    private final int totalItems;

    public Manager(int storageSize, int totalItems) {
        this.totalItems = totalItems;
        access = new Semaphore(1);
        full  = new Semaphore(storageSize);
        empty = new Semaphore(0);
    }

    public synchronized boolean registerProduced() {
        if (totalProduced < totalItems) {
            totalProduced++;
            return true;
        }
        return false;
    }
}