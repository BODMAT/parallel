package com.company;

public class Main {
    public static void main(String[] args) throws Exception  {
        System.setOut(new java.io.PrintStream(System.out, true, "UTF-8"));

        final int cores = Runtime.getRuntime().availableProcessors();

        final int DIM = 500_000_000;
        int[] threadCounts = {4, cores};

        System.out.println("Доступних процесорів (ядер): " + cores);
        System.out.println("Розмір масиву: " + DIM);
        System.out.println("=".repeat(60));

        for (int threadNum : threadCounts) {
            System.out.printf("%n>>> Запуск з кількістю потоків: %d%n", threadNum);

            ArrClass arrClass = new ArrClass(DIM, threadNum);

            long startTime = System.currentTimeMillis();
            int[] result = arrClass.threadMin();
            long elapsed = System.currentTimeMillis() - startTime;

            System.out.printf("Мінімальний елемент: %d, індекс: %d%n",
                    result[0], result[1]);
            System.out.printf("Час виконання: %d мс%n", elapsed);
            System.out.println("-".repeat(60));
        }
    }
}
