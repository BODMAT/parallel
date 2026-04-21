import java.util.concurrent.Semaphore;

public class Table {
    private final Semaphore[] forks = new Semaphore[5];

    public Table() {
        for (int i = 0; i < forks.length; i++) {
            forks[i] = new Semaphore(1);
        }
    }

    public void getFork(int id) {
        try {
            forks[id].acquire();
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
    }

    public void putFork(int id) {
        forks[id].release();
    }
}
