using System.Linq;

namespace AdventOfCode2024.Solutions;

internal class Day16 : ISolution
{
    public int Day => 16;
    public object? Solve1(string input) =>
        0 is { } direction &&
        input.IndexOf('\n') + 1 is { } width &&
        input.IndexOf('S') is { } current &&
        input.IndexOf('E') is { } end &&
        (input = input!.Replace('S', '.').Replace('E', '.')) is { } &&
        
        new int[]
        {
            -width,
            1,
            width,
            -1
        } is { } deltaTable &&

        new HashSet<int>() is { } visited &&
        input
            .Select((c, i) => (i, c))
            .Where(c => c.c == '.')
            .SelectMany(t => new[] { (t.i, 0), (t.i, 1), (t.i, 2), (t.i, 3) })
            .ToDictionary(c => (x: c.i, dir: c.Item2), c => int.MaxValue - 1) is { } dists ?
        dists.Where(t => true)
        : throw null!;

    public object? Solve2(string input) => input;
}
