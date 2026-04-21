using System.Threading;

public class Table
{
    private readonly SemaphoreSlim[] forks = new SemaphoreSlim[5];
    private readonly SemaphoreSlim waiter = new SemaphoreSlim(2, 2); // max 2 одночасно

    public Table()
    {
        for (int i = 0; i < forks.Length; i++)
            forks[i] = new SemaphoreSlim(1, 1);
    }

    public void GetFork(int right, int left)
    {
        waiter.Wait();
        forks[right].Wait();
        forks[left].Wait();
    }

    public void PutFork(int right, int left)
    {
        forks[left].Release();
        forks[right].Release();
        waiter.Release();
    }
}