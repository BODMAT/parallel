package com.company;

public class Main {

    public static void main(String[] args) {
        BreakThread breakThread = new BreakThread();
        
        int cores = Runtime.getRuntime().availableProcessors();
        for (int i = 1; i <= cores; i++) {
            new MainThread(i, breakThread).start();
        }

        new Thread(breakThread).start();
    }
}