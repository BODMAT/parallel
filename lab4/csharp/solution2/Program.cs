public class Program
{
    public static void Main(string[] args)
    {
        Table table = new Table();

        for (int i = 0; i < 5; i++)
            new Philosopher(i, table);
    }
}