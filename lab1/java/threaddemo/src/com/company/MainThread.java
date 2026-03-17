package com.company;
import java.math.BigInteger;

public class MainThread extends Thread {
    private final int id;
    private final boolean[] flags;
    private final int index;

    public MainThread(int id, boolean[] flags, int index) {
        this.id = id;
        this.flags = flags;
        this.index = index;
    }

    @Override
    public void run() {
        BigInteger sum = BigInteger.ZERO;
        BigInteger current = BigInteger.ZERO;
        BigInteger step = BigInteger.valueOf(id);
        long count = 0;
        long startTime = System.currentTimeMillis();

        while (!flags[index]) {
            sum = sum.add(current);
            current = current.add(step);
            count++;
        }

        long elapsed = System.currentTimeMillis() - startTime;
        System.out.println("Thread " + id + " | Sum: " + sum + " | Count: " + count + " | Time: " + elapsed + "ms");
    }
}
