using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

internal class Day18 : ISolution
{
    public int Day => 18;
    public object? Solve1(string input) => 
        Enumerable
            .Repeat('.', 71)
            .SelectMany(i => Enumerable.Repeat(i, 71).Append('\n')).ToArray() is { } buff && 
        input.Split('\n')
            .Take(1024)
            .Select(i => i.Split(','))
            .Select(i => (x: int.Parse(i[0]), y: int.Parse(i[1])))
            .Select(i => buff[i.x + i.y * 72] = '#')
            .Count() is { } && 
        new int[] { 1, -1, 72, -72 } is { } deltas &&
        0 is { } current &&
        Enumerable.Range(0, buff.Length).Where(i => buff[i] == '.').ToDictionary(t => t, t => t == 0 ? 0 : int.MaxValue - 1) is { } dists &&
        new HashSet<int>() is { } finished &&
        Enumerable.Range(0, int.MaxValue)
                .Select(y => dists[current])
                .Select(y => deltas.Select(t => current + t).Select(t => t >= 0 && t < buff.Length && dists.TryGetValue(t, out int oldDist) && oldDist > y + 1 ? dists[t] = y + 1 : 0).Count()
                    != -1 && (finished.Add(current) & (current = dists.MinBy(t => finished.Contains(t.Key) ? int.MaxValue : t.Value).Key) is { }))
                .TakeWhile(t => finished.Count < 71 * 71 - 1024).Count() is { } ?
            dists[71 * 72 - 2]
        : throw null!;

    public object? Solve2(string input) => Enumerable
            .Repeat('.', 71)
            .SelectMany(i => Enumerable.Repeat(i, 71).Append('\n')).ToArray() is { } buff &&
        input.Split('\n')
            .Reverse()
            .Select(i => i.Split(','))
            .Select(i => (x: int.Parse(i[0]), y: int.Parse(i[1])))
            .Select((i, j) => (buff[i.x + i.y * 72] = '#') is { } ? (i, j) : (i, j))
            .ToArray() is { } locations &&
        new int[] { 1, -1, 72, -72 } is { } deltas &&
        0 is { } current &&
        new Dictionary<int, int>() is { } dists &&
        new HashSet<int>() is { } finished ?
            locations.First(l => 
            (buff[l.i.x + l.i.y * 72] = '.') is { } && (finished = new HashSet<int>()) is { } && (dists = Enumerable.Range(0, buff.Length).Where(i => buff[i] == '.').ToDictionary(t => t, t => t == 0 ? 0 : int.MaxValue - 1)) is { } && Enumerable.Range(0, int.MaxValue)
                .Select(y => dists[current])
                .Select(y => deltas.Select(t => current + t).Select(t => t >= 0 && t < buff.Length && dists.TryGetValue(t, out int oldDist) && oldDist > y + 1 ? dists[t] = y + 1 : 0).Count()
                    != -1 && (finished.Add(current) & (current = dists.MinBy(t => finished.Contains(t.Key) ? int.MaxValue : t.Value).Key) is { }))
                .TakeWhile(t => dists.Any(t => t.Value != int.MaxValue - 1 && !finished.Contains(t.Key))).Count() is { } && dists[71 * 72 - 2] != int.MaxValue - 1).i.ToString()[1..^1].Replace(" ", "")
        : throw null!;
}


