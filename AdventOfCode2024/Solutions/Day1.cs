using System.Text;

namespace AdventOfCode2024.Solutions;

internal class Day1 : ISolution
{
    public int Day => 1;

    public object Solve1(string input) => input.Split('\n')
        .Select(s => s.Split("   "))
        .Aggregate((new StringBuilder(), new StringBuilder()), (t, s) => (t.Item1.Append(' ').Append(s[0]), t.Item2.Append(' ').Append(s[1])),
            t => t.Item1.ToString().Split(' ').Skip(1).Select(int.Parse).OrderBy(i => i)
            .Zip(t.Item2.ToString().Split(' ').Skip(1).Select(int.Parse).OrderBy(i => i)))
        .Select(t => t.First - t.Second)
        .Sum(Math.Abs);

    public object Solve2(string input) => input.Split('\n')
        .Select(s => s.Split("   "))
        .Aggregate((new StringBuilder(), new StringBuilder()), (t, s) => (t.Item1.Append(' ').Append(s[0]), t.Item2.Append(' ').Append(s[1])),
            t => t.Item1.ToString().Split(' ').Skip(1).Select(
                s => (int.Parse(s), t.Item2.ToString().Split(' ').Skip(1).Select(int.Parse).GroupBy(i => i).ToDictionary(l => l.Key))
                ))
        .Sum(t => (t.Item2.TryGetValue(t.Item1, out var group) ? group.Count() : 0) * t.Item1);
}
