using System;
using System.Threading;

public class Philosopher
{
    private readonly Table table;
    private readonly int leftFork, rightFork;
    private readonly int id;
    private readonly Thread thread;

    public Philosopher(int id, Table table)
    {
        this.id = id;
        this.table = table;
        rightFork = id;
        leftFork = (id + 1) % 5;
        thread = new Thread(Run);
        thread.Start();
    }

    private void Run()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Philosopher{id} is thinking ({i + 1})");
            table.GetFork(rightFork, leftFork);
            Console.WriteLine($"Philosopher{id} is eating ({i + 1})");
            table.PutFork(rightFork, leftFork);
        }
        Console.WriteLine($"Philosopher{id} finished.");
    }
}