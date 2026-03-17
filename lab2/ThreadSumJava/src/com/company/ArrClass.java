package com.company;
import java.util.Random;

public class ArrClass {
    private final int dim;
    private final int threadNum;
    public final int[] arr;

    private int globalMin = Integer.MAX_VALUE;
    private int globalMinIndex = -1;
    private int threadCount = 0;

    public ArrClass(int dim, int threadNum) {
        this.dim = dim;
        this.threadNum = threadNum;
        arr = new int[dim];
        Random rand = new Random();

        for (int i = 0; i < dim; i++) {
            arr[i] = rand.nextInt(1_000_000) + 1;
        }

        int negIndex = rand.nextInt(dim); //!
        arr[negIndex] = -(rand.nextInt(1_000_000) + 1);
        System.out.println("Вставлено від'ємне значення: " + arr[negIndex]
                + " за індексом: " + negIndex);
    }

    public int[] partMin(int startIndex, int finishIndex) {
        int localMin = arr[startIndex];
        int localMinIndex = startIndex;
        for (int i = startIndex + 1; i < finishIndex; i++) {
            //! Симуляція для наглядності навантаження ядер
            int val = (int)(Math.sqrt(Math.abs(arr[i])) * Math.sin(arr[i]));

            if (arr[i] < localMin) {
                localMin = arr[i];
                localMinIndex = i;
            }
        }
        return new int[]{localMin, localMinIndex};
    }

    // критична секція
    synchronized public void collectMin(int localMin, int localMinIndex) {
        if (localMin < globalMin) {
            globalMin = localMin;
            globalMinIndex = localMinIndex;
        }
    }

    synchronized public void incThreadCount() {
        threadCount++;
        notify(); // будю getResult(), щоб перевірила умову
    }

    synchronized private int[] getResult() {
        while (threadCount < threadNum) {
            try {
                wait(); // засинає, звільняє lock
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
        return new int[]{globalMin, globalMinIndex};
    }

    public int[] threadMin() {
        ThreadMin[] threads = new ThreadMin[threadNum];
        int chunkSize = dim / threadNum;

        for (int i = 0; i < threadNum; i++) {
            int start = i * chunkSize;
            int end = (i == threadNum - 1) ? dim : start + chunkSize;
            threads[i] = new ThreadMin(i, start, end, this);
        }

        for (ThreadMin t : threads) {
            t.start();
        }

        return getResult();
    }
}
