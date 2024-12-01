using System.Linq;

namespace AdventOfCode2024;

internal static class Program
{
    static void Main(string[] args)
    {
        string input = File.ReadAllText("stuff.txt");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Input:");
        Console.WriteLine(input);

        ISolution[] solutions = [.. typeof(Program)
            .Assembly
            .GetTypes()
            .Where(t => typeof(ISolution).IsAssignableFrom(t) && t.IsClass)
            .Select(t => (ISolution)Activator.CreateInstance(t)!)
            .OrderBy(o => o.Day)];


        ISolution solutionToUse = args.Length == 0 ? solutions[^1] : solutions[int.Parse(args[0])];

        Console.ForegroundColor = ConsoleColor.White;

        object? s1 = solutionToUse.Solve1(input);
        object? s2 = solutionToUse.Solve2(input);

        Console.WriteLine("\nSolutions:");
        Console.WriteLine(s1?.ToString() ?? "Part 1 Not Implemented");
        Console.WriteLine(s2?.ToString() ?? "Part 2 Not Implemented");
    }

    public static IEnumerable<T> DebugWrite<T>(this IEnumerable<T> obj)
    {
        foreach(var t in obj)
            Console.WriteLine(t);
        return obj;
    }
}
