namespace AdventOfCode2024.Solutions;

internal class Day8 : ISolution
{
    public int Day => 8;
    public object? Solve1(string input) =>
        //tuple lib lol
        Array.Empty<int>().Aggregate(((int x, int y) a, (int x, int y) b) => (a.x + b.x, a.y + b.y), (a, b) => null!) is
        Func<(int, int), (int, int), (int, int)> add &&
        Array.Empty<int>().Aggregate(((int x, int y) a, (int x, int y) b) => (a.x - b.x, a.y - b.y), (a, b) => null!) is
        Func<(int, int), (int, int), (int, int)> sub &&
        Array.Empty<int>().Aggregate(((int x, int y) a, (int x, int y) b) => (a.x * b.x, a.y * b.y), (a, b) => null!) is
        Func<(int, int), (int, int), (int, int)> mul &&

        input.IndexOf('\n') is int width &&
        (int)Math.Round(input.Length / (width + 1f)) is int height &&
        Array.Empty<int>().Aggregate(((int x, int y) a) => a.x >= 0 && a.y >= 0 && a.x < width && a.y < height, (a, b) => null!) is
        Func<(int, int), bool> inRange ?

        input
            .Where(t => t != '\n')
            .Select((c, index) => (x: index % width, y: index / width, @char: c))
            .Where(c => c.@char != '.')
            .ToLookup(c => c.@char, c => (c.x, c.y))
            .SelectMany(grouping => grouping
                .SelectMany(i => grouping
                    .Select(j => (left: i, right: j))
                    .Where(t => t.left != t.right)
                    .Select(t => add(t.left, mul((2, 2), sub(t.right, t.left))) is { } node && inRange(node) ? node : (-1, -1))
                ))
            .Distinct()
        .Count() - 1

        : throw null!;

    public object Solve2(string input) =>
        Array.Empty<int>().Aggregate(((int x, int y) a, (int x, int y) b) => (a.x + b.x, a.y + b.y), (a, b) => null!) is
        Func<(int, int), (int, int), (int, int)> add &&
        Array.Empty<int>().Aggregate(((int x, int y) a, (int x, int y) b) => (a.x - b.x, a.y - b.y), (a, b) => null!) is
        Func<(int, int), (int, int), (int, int)> sub &&
        Array.Empty<int>().Aggregate(((int x, int y) a, (int x, int y) b) => (a.x * b.x, a.y * b.y), (a, b) => null!) is
        Func<(int, int), (int, int), (int, int)> mul &&

        input.IndexOf('\n') is int width &&
        (int)Math.Round(input.Length / (width + 1f)) is int height ?

        input
            .Where(t => t != '\n')
            .Select((c, index) => (x: index % width, y: index / width, @char: c))
            .Where(c => c.@char != '.')
            .ToLookup(c => c.@char, c => (c.x, c.y))
            .SelectMany(grouping => grouping
                .SelectMany(i => grouping
                    .Select(j => (left: i, right: j))
                    .Where(t => t.left != t.right)
                    .SelectMany(t => 
                        Enumerable.Range(0, int.MaxValue)
                        .Select(i => add(t.left, mul(sub(t.right, t.left), (i, i))))
                        .TakeWhile(((int x, int y) a) => a.x >= 0 && a.y >= 0 && a.x < width && a.y < height))
                ))
            .Distinct()
        .Count()

        : throw null!;
}
