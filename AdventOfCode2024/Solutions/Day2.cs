using System;
using System.Linq;
using System.Text;

namespace AdventOfCode2024.Solutions;

internal class Day2 : ISolution
{
    public int Day => 2;

    public object Solve1(string input) => input
        .Split('\n')
        .Select(s => s.Split(' '))
        .Select(s => s.Select(int.Parse))
        .Select(s => s.Zip(s.Skip(1)).Select(t => t.First - t.Second))
        .Where(s => s.All(a =>
            Math.Sign(a) == Math.Sign(s.First()) &&
            a != 0 && Math.Abs(a) <= 3
        ))
        .Count();

    public object Solve2(string input) => input
        .Split('\n')
        .Select(s => s.Split(' '))
        .Select(s => s.Select(int.Parse))
        .Select(s => s.ToList())
        .Where(s => s.Zip(s.Skip(1))
            .Select(t => (Math.Sign(s[0] - s[1]), t))
            .All(t => DeltaIsValid(t.Item2.First - t.Item2.Second) && Math.Sign(t.Item2.First - t.Item2.Second) == t.Item1)
            || Enumerable.Range(0, s.Count)
                .Any(i => Array.Empty<int>()
                    .Aggregate(s
                        .Where((_, index) => index != i).ToList(), (a, b) => null!, s => s.Zip(s.Skip(1))
                        .Select(t => (Math.Sign(s[0] - s[1]), t))
                        .All(t => DeltaIsValid(t.Item2.First - t.Item2.Second) && Math.Sign(t.Item2.First - t.Item2.Second) == t.Item1))))
        .Count();

    static bool DeltaIsValid(int delta)
    {
        if(Math.Abs(delta) > 3)
        {
            return false;
        }

        return delta != 0;
    }
}
