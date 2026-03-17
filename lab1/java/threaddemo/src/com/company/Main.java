package com.company;

public class Main {
    public static void main(String[] args) {
        int cores = Runtime.getRuntime().availableProcessors();
        boolean[] flags = new boolean[cores];

        for (int i = 0; i < cores; i++) {
            new MainThread(i + 1, flags, i).start();
        }

        new Thread(new BreakThread(flags)).start();
    }
}
