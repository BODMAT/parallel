using System.Threading;

public class Table
{
    private readonly SemaphoreSlim[] forks = new SemaphoreSlim[5];

    public Table()
    {
        for (int i = 0; i < forks.Length; i++)
            forks[i] = new SemaphoreSlim(1, 1);
    }

    public void GetFork(int id) => forks[id].Wait();
    public void PutFork(int id) => forks[id].Release();
}