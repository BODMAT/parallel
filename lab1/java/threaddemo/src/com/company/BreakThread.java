package com.company;
import java.util.Arrays;
import java.util.Random;

public class BreakThread implements Runnable {
    private final boolean[] flags;
    private final Random random = new Random();

    public BreakThread(boolean[] flags) {
        this.flags = flags;
    }

    @Override
    public void run() {
        long[][] times = new long[flags.length][2];
        for (int i = 0; i < flags.length; i++) {
            times[i][0] = 2000 + random.nextInt(5001); 
            times[i][1] = i;                            
        }

        Arrays.sort(times, (a, b) -> Long.compare(a[0], b[0]));

        long elapsed = 0;
        for (long[] entry : times) {
            long delay = entry[0] - elapsed;
            try {
                Thread.sleep(delay);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            elapsed = entry[0];
            flags[(int) entry[1]] = true;
        }
    }
}
