using System.Linq;

namespace AdventOfCode2024;

internal static class Program
{    
    static void Main(string[] args)
    {
        string input = File.ReadAllText(@"C:\Users\1017251\Desktop\readme.txt");

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Input:");
        Console.WriteLine(input);
            
        ISolution[] solutions = typeof(Program)
            .Assembly
            .GetTypes()
            .Where(t => typeof(ISolution).IsAssignableFrom(t) && t.IsClass)
            .Select(t => (ISolution)Activator.CreateInstance(t)!)
            .OrderBy(o => o.Day)
            .ToArray();


        ISolution solutionToUse = args.Length == 0 ? solutions[^1] : solutions[int.Parse(args[0])];

        Console.ForegroundColor = ConsoleColor.White;

        object? s1 = solutionToUse.Solve1(input);
        object? s2 = solutionToUse.Solve2(input);

        Console.WriteLine("\nSolutions:");
        Console.WriteLine(s1?.ToString() ?? "Part 1 Not Implemented");
        Console.WriteLine(s2?.ToString() ?? "Part 2 Not Implemented");
    }

    public static IEnumerable<T> WriteIter<T>(this IEnumerable<T> obj, Action<T>? tranform = null, bool newline = false)
    {
        foreach(var t in obj)
        {
            if(tranform is null)
            {
                Console.Write(newline ? t + "\n" : t);
            }
            else
            {
                tranform(t);
            }
        }

        return obj;
    }

    public static T WriteSelf<T>(this T o)
    {
        Console.WriteLine(o);
        return o;
    }

    public static T Write<T>(this T o, object v)
    {
        Console.WriteLine(v);
        return o;
    }

    public static T Eval<T>(this T o, Action action)
    {
        action();
        return o;
    }
}
