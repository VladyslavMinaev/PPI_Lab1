using PPI_Lab1.Domain;

// var onIntervalElapsed = new ElapsedEventHandler(ShowThreadInformation);
string firstThreadName = "Thread 1";
string secondThreadName = "Thread 2";

var writer1 = new ConsoleWriter(firstThreadName, 2000);
var writer2 = new ConsoleWriter(secondThreadName, 2000);

Thread thread1 = new Thread(new ParameterizedThreadStart(writer1.Start))
{
    Priority = ThreadPriority.Lowest
};
Thread thread2 = new Thread(new ParameterizedThreadStart(writer2.Start))
{
    Priority = ThreadPriority.AboveNormal
};

thread1.Start(100000);
Thread.Sleep(500);
thread2.Start(100000);

await ThreadPriorityAsyncHandler();

async Task ThreadPriorityAsyncHandler()
{
    await Task.Run(() =>
    {
        while (true)
        {
            var command = GenerateCommand();
            if (string.IsNullOrEmpty(command) == true)
                return;

            var threadAndPriority = ParseCommand(command);
            if (threadAndPriority == null)
                return;

            var thread = threadAndPriority.Item1;
            var priority = threadAndPriority.Item2;

            thread.Priority = priority;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"The {command.Split(':')[0]} has been changed to priority: {priority}");
            Console.ForegroundColor = ConsoleColor.Black;

            Thread.Sleep(10000);
        }
    });
}

string? GenerateCommand()
{
    var value = Random.Shared.Next(0, 2);

    if (value == 0)
        return $"{firstThreadName}:";
    else if (value == 1)
        return $"{secondThreadName}:";

    return string.Empty;
}

Tuple<Thread, ThreadPriority>? ParseCommand(string message)
{
    var parameters = message.Split(':');

    if (parameters.Length != 2)
        return null;

    var threadToChange = GetTreadByName(parameters[0]);
    if (threadToChange == null)
        return null;

    var newPriority = GetRandomThreadPriority();

    return new Tuple<Thread, ThreadPriority>(threadToChange, newPriority);
}

Thread? GetTreadByName(string threadName)
{
    if (firstThreadName == threadName)
        return thread1;
    else if (secondThreadName == threadName)
        return thread2;

    return null;
}

ThreadPriority GetRandomThreadPriority()
{
    var value = Random.Shared.Next(0, 3);

    if (value == 0)
        return ThreadPriority.BelowNormal;
    else if (value == 1)
        return ThreadPriority.Normal;
    else if (value == 2)
        return ThreadPriority.AboveNormal;

    return ThreadPriority.Lowest;
}