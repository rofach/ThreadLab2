using System;
using ConsoleApp1;

namespace ThreadLab;

static class Program
{
    private static readonly ArrayMinimumManager _minManager = new ArrayMinimumManager();

    static void Main(string[] args)
    {
        bool isRunning = true;


        Console.WriteLine("Enter array size");
        int size = int.Parse(Console.ReadLine() ?? "10000000");
        var arrGen = new ArrayGenerator(size);
        
        Console.WriteLine("Enter count of threads");
        List<int> countOfThreads = Console.ReadLine()?.Split(' ').Select(int.Parse).ToList() ?? new List<int> { 1, 2, 4, 8 };

        foreach (var count in countOfThreads)
        {
            Console.WriteLine($"\nTESTING WITH {count} THREADS");
            Console.WriteLine("\n--Testing ArrayMinimumFinder");
            var threadFinder = new ArrayMinimumFinder(_minManager, count);
            threadFinder.StartFindingMin(arrGen.GeneratedArray);
            Console.WriteLine($"----Found minimum: {_minManager.GetMin()}");

            Console.WriteLine("\n--Testing ArrayMinimumFinderWithCountDownEvent");
            var taskFinder = new ArrayMinimumFinderWithCountDown(_minManager, count);
            taskFinder.StartFindingMin(arrGen.GeneratedArray);
            Console.WriteLine($"----Found minimum: {_minManager.GetMin()}");
        }

        Console.ReadLine();
    }
}