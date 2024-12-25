using System.Linq;
using System.Runtime.InteropServices;

namespace AdventOfCode2024.Solutions;

internal class Day25 : ISolution
{
    public int Day => 25;

    public object? Solve1(string input) => 
        Array.Empty<object>()
            .Aggregate(input
                .Split("\n\n")
                .Select(g => (K: g[0], Data: g.Split('\n')))
                .Select(g => (g.K, Data: g.K == '#' ? g.Data : g.Data.Reverse()))
                .Select(g => (g.K, Enumerable.Range(0, 5).Select(i => g.Data.Select((j, i) => (i, j)).First(t => t.j[i] == '.').i)))
                .Aggregate(new Dictionary<char, HashSet<int[]>>(), (acc, n) => (CollectionsMarshal.GetValueRefOrAddDefault(acc, n.K, out _) ??= new()).Add(n.Item2.ToArray()) ? acc : acc), (_, _) => null!, 
            c => c['.'].Sum(l => c['#'].Count(k => k.Zip(l).All(t => t.First + t.Second <= 7))));

    public object? Solve2(string input) => "Merry Christmas!";
}