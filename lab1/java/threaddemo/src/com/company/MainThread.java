package com.company;
import java.math.BigInteger;

public class MainThread extends Thread {
    private final int id;
    private final BreakThread breakThread;

    public MainThread(int id, BreakThread breakThread) {
        this.id = id;
        this.breakThread = breakThread;
    }

    @Override
    public void run() {
        BigInteger sum = BigInteger.ZERO;
        BigInteger arithmeticStep = BigInteger.ZERO;
        BigInteger stepVal = BigInteger.valueOf(2);
        long count = 0;
        boolean isStop = false;

        do {
            arithmeticStep = arithmeticStep.add(stepVal);
            sum = sum.add(arithmeticStep);
            count++;
            isStop = breakThread.isCanBreak();
        } while (!isStop);

        System.out.println("Thread " + id + " - Final Sum: " + sum + ", Count: " + count);
    }
}