using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

internal class Day5 : ISolution
{
    public int Day => 5;



    public object? Solve1(string input) =>
        Regex.Matches(input, @"(\d+)\|(\d+)")
        .Select(m => (int.Parse(m.Groups[1].ValueSpan), int.Parse(m.Groups[2].ValueSpan)))
        .ToLookup(t => t.Item1) is ILookup<int, (int, int)> table ? 
        Regex.Matches(input, @"(\d+,)+\d+")
        .Select(m => m.Value.Split(',').Select(int.Parse))
        .Where(t => t
            .Select((num, index) => (index, num))   
            .TakeWhile(e => t.Take(e.index).All(pn => !table[e.num].Contains((e.num, pn))))
            .Count() == t.Count()
            )
        .Select(a => a.Skip(a.Count() / 2).First())
        .Sum()
        : throw null!;



    public object? Solve2(string input) =>
        Regex.Matches(input, @"(\d+)\|(\d+)")
        .Select(m => (int.Parse(m.Groups[1].ValueSpan), int.Parse(m.Groups[2].ValueSpan)))
        .ToLookup(t => t.Item1) is ILookup<int, (int, int)> table ?
        Regex.Matches(input, @"(\d+,)+\d+")
        .Select(m => m.Value.Split(',').Select(int.Parse))
        .Where(t => t
            .Select((num, index) => (index, num))
            .TakeWhile(e => t.Take(e.index).All(pn => !table[e.num].Contains((e.num, pn))))
            .Count() != t.Count()
            )
        .Select(i => i.OrderBy(n => n, Comparer<int>.Create((a, b) => !table[a].Contains((a, b)) ? 1 : -1))).Break()
        .Select(a => a.Skip(a.Count() / 2).First())
        .Sum()
        : throw null!;
}
