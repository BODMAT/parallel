import java.util.concurrent.Semaphore;

public class Table {
    private final Semaphore[] forks = new Semaphore[5];
    private final Semaphore waiter = new Semaphore(2);

    public Table() {
        for (int i = 0; i < forks.length; i++) {
            forks[i] = new Semaphore(1);
        }
    }

    public void getFork(int right, int left) throws InterruptedException {
        waiter.acquire();
        forks[right].acquire();
        forks[left].acquire();
    }

    public void putFork(int right, int left) {
        forks[left].release();
        forks[right].release();
        waiter.release();
    }
}