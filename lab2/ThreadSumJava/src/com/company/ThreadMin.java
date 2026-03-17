package com.company;

public class ThreadMin extends Thread {
    private final int threadId;
    private final int startIndex;
    private final int finishIndex;
    private final ArrClass arrClass;

    public ThreadMin(int threadId, int startIndex, int finishIndex, ArrClass arrClass) {
        this.threadId = threadId;
        this.startIndex = startIndex;
        this.finishIndex = finishIndex;
        this.arrClass = arrClass;
    }

    @Override
    public void run() {
        System.out.printf("Потік #%d | Ім'я: %-20s | Діапазон: [%d, %d)%n",
                threadId, Thread.currentThread().getName(), startIndex, finishIndex);

        int[] result = arrClass.partMin(startIndex, finishIndex);

        System.out.printf("Потік #%d | Локальний мін: %d за індексом: %d%n",
                threadId, result[0], result[1]);

        arrClass.collectMin(result[0], result[1]);
        arrClass.incThreadCount();
    }
}
