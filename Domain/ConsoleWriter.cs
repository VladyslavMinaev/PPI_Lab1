using System.Timers;

namespace PPI_Lab1.Domain;

public class ConsoleWriter
{
    private string _name;
    private System.Timers.Timer _timer;
    private Thread _currentThread;
    public ConsoleWriter(string name, int timerIntervalInMilliseconds)
    {
        _name = name;

        var onIntervalElapsed = new ElapsedEventHandler(ShowThreadInformation);
        _timer = new System.Timers.Timer(timerIntervalInMilliseconds);
        _timer.AutoReset = true;
        _timer.Elapsed += onIntervalElapsed;

        _currentThread = null;
    }

    public void Start(object? o)
    {
        _currentThread = Thread.CurrentThread;

        int iterationMaxValue;
        try
        {
            iterationMaxValue = (int)o;
        }
        catch (InvalidCastException)
        {
            iterationMaxValue = 100000;
            Console.WriteLine($"Invalid argument, method will run with {nameof(iterationMaxValue)}=100000");
        }

        _timer.Start();
        for (int i = 1; i <= iterationMaxValue; i++)
        {
            Console.WriteLine($"Name: {_name}; State: {i}");
            Thread.Sleep(200);
        }

        Thread.Sleep(10000);
        Console.WriteLine($"The {_name} is shutting down ...");
    }

    private void ShowThreadInformation(object? o, ElapsedEventArgs e)
    {
        if (_currentThread == null)
            throw new NullReferenceException($"{_currentThread} can not contain null");

        var th = _currentThread;

        Console.WriteLine("\n\n");
        Console.WriteLine($"Thread name: {th.Name}");
        Console.WriteLine("Managed thread #{0}: ", th.ManagedThreadId);
        Console.WriteLine("Background thread: {0}", th.IsBackground);
        Console.WriteLine("Thread pool thread: {0}", th.IsThreadPoolThread);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Priority: {0}", th.Priority);
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("\n\n");
    }
}