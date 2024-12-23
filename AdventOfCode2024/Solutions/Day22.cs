using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode2024.Solutions;

internal class Day22 : ISolution
{
    public int Day => 22;

    public object? Solve1(string input) =>
        input
            .Split('\n')
            .Select(long.Parse)
            .Select(t => Enumerable.Range(0, 2000).Aggregate(t, (s, _) => ((s << 6) ^ s) % 16777216 is { } s1 && ((s1 >> 5) ^ s1) % 16777216 is { } s2 && ((s2 << 11) ^ s2) % 16777216 is { } s3 ? s3 : throw null!))
            .Sum();

    public object? Solve2(string input) =>
        input
            .Split('\n')
            .Select(long.Parse)
            .Aggregate(new Dictionary<(int, int, int, int), Dictionary<long, int>>(), (prev, t) => Enumerable.Range(1, 2000)
                            .Aggregate((D: new Dictionary<int, (int Cost, int Delta)> { { 0, ((int)(t % 10), 0) } }, P: t),
                                (s, i) => (((s.P << 6) ^ s.P) % 16777216 is { } s1 &&
                                          ((s1 >> 5) ^ s1) % 16777216 is { } s2 &&
                                          ((s2 << 11) ^ s2) % 16777216 is { } s3 ? s3
                                                : throw null!) is { } next && s.D.TryAdd(i, ((int)(next % 10), (int)(next % 10) - (int)(s.P % 10))) ?
                                                (s.D, next)
                                                    : throw null!).D
                            .Select(t => t.Value)
                            .Skip(1)
                            .ToArray() is { } x && x.Select((_, i) => x.Skip(i).Take(4).ToArray()).Where(t => t.Length == 4)
                            .Select(t => (cost: t[3].Cost, deltas: (t[0].Delta, t[1].Delta, t[2].Delta, t[3].Delta)))
                            .Select(i => prev.TryGetValue(i.deltas, out var dict) ? dict.TryGetValue(t, out int value) ? false : (dict[t] = i.cost) is { } : (prev[i.deltas] = new Dictionary<long, int> { { t, i.cost } }) is null)
                            .Count() is not -1 ? prev : throw null!)
            .Select(t => t.Value.Sum(t => t.Value))
            .Max();
}