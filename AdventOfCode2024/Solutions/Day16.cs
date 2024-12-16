using System.Linq;

namespace AdventOfCode2024.Solutions;

internal class Day16 : ISolution
{
    public int Day => 16;
    public object? Solve1(string input) =>
        0 is { } direction &&
        input.IndexOf('\n') + 1  is { } width &&
        input.IndexOf('S')  is { } start &&
        input.IndexOf('E')  is { } end &&
        new (int F, int A, int B)[]
        {
            (-width, 1, -1),
            (1, width, -width),
            (-1, width, -width),
            (-width, 1, -1),
        } is { } deltas &&
        new HashSet<int>() is { } visited &&
        new SortedDictionary<int, int>(input.Select((c, i) => (i, c)).Where(c => c.c == '.').ToDictionary(c => c.i, c => int.MaxValue))
         is { } dists ?
            Enumerable.Range(0, int.MaxValue)
            .Select(t => dists[start + deltas[direction].F] = 1)
            .Select(t => dists[start + deltas[direction].A] = 1000)
            .Select(t => dists[start + deltas[direction].B] = 1000)
            .Select(t => visited.Add(start))
            .Select(t => start =  dists.MinBy(kvp => kvp.Value).Key)
            .TakeWhile(t => input[start] != 'E')
        : throw null!;

    public object? Solve2(string input) => input;
}
