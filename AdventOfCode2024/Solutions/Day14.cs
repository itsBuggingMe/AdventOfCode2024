using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

internal class Day14 : ISolution
{
    public int Day => 14;

    public object? Solve1(string input) =>
        Regex.Matches(input, @"=(-?\d+),(-?\d+)..=(-?\d+),(-?\d+)")
        .Select(s => (x: int.Parse(s.Groups[1].ValueSpan) + 20200000, y: int.Parse(s.Groups[2].ValueSpan) + 20600000, dx: int.Parse(s.Groups[3].ValueSpan), dy: int.Parse(s.Groups[4].ValueSpan)))
        .Select(s => (x: (s.x + s.dx * 100) % 101, y: (s.y + s.dy * 100) % 103))
        .Aggregate(new int[5], (a, g) => a[g switch
        {
            (> 50, < 51) => 0,
            (< 50, < 51) => 1,
            (> 50, > 51) => 2,
            (< 50, > 51) => 3,
            _ => 4,
        }]++ == -1 ? a : a, t => t[0] * t[1] * t[2] * t[3]);

    public object? Solve2(string input) =>
        Regex.Matches(input, @"=(-?\d+),(-?\d+)..=(-?\d+),(-?\d+)")
        .Select(s => (x: int.Parse(s.Groups[1].ValueSpan) + 20200000, y: int.Parse(s.Groups[2].ValueSpan) + 20600000, dx: int.Parse(s.Groups[3].ValueSpan), dy: int.Parse(s.Groups[4].ValueSpan)))
        .ToArray() is { } initpos && 
        new bool[101 * 103] is bool[] buff ?
            Enumerable.Range(0, int.MaxValue).First(t => 
                (buff = new bool[101 * 103]) is { } && 
                initpos.All(s => (s.x + s.dx * t) % 101 + (s.y + s.dy * t) % 103 * 101 is int index && buff[index] ? false : buff[index] = true))
        : throw null!;
}
