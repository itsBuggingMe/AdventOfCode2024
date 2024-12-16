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
        new (int Forward, int TurnA, int TurnB)[]
        {
            (-width, 1, -1),
            (1, width, -width),
            (-1, width, -width),
            (-width, 1, -1),
        } is { } deltas &&
        new HashSet<int>() is { } visited &&
        new SortedDictionary<int, int>(input.Select((c, i) => (i, c)).Where(c => c.c == '.').ToDictionary(c => c.i, c => int.MaxValue))
         is { } buff ?
            Enumerable.Range(0, int.MaxValue)
            .Select(t => new int[] {  })
        : throw null!;

    public object? Solve2(string input) => input;
}