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
            
            if (id == 4)
            {
                table.GetFork(leftFork);
                table.GetFork(rightFork);
            }
            else
            {
                table.GetFork(rightFork);
                table.GetFork(leftFork);
            }

            Console.WriteLine($"Philosopher{id} is eating ({i + 1})");
            table.PutFork(leftFork);
            table.PutFork(rightFork);
        }
        Console.WriteLine($"Philosopher{id} finished.");
    }
}