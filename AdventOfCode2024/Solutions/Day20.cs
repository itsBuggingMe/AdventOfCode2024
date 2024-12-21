using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

internal class Day20 : ISolution
{
    public int Day => 20;
    public object? Solve1(string input) => 
        input.Replace("E", ".").Replace("S", ".") is { } cleanInput &&
        input.IndexOf('\n') + 1 is { } w &&
        new int[]
        {
            1,
            -1,
            w,
            -w,
        } is { } deltas &&
        new (int F, int S)[]
        {
            (+1, +1), (-1, +1), (+w, +1), (-w, +1), (+1, -1), (-1, -1), (+w, -1), (-w, -1), (+1, +w), (-1, +w), (+w, +w), (-w, +w), (+1, -w), (-1, -w), (+w, -w), (-w, -w),
        } is { } cheats &&
        (object)((int current, Func<int, IEnumerable<(int Location, int Weight)>> getEdges) => 
            new HashSet<int>() is { } visited &&
            new Dictionary<int, int>
            {
                { current, 0 },
            } is { } costs &&
            Enumerable
                    .Range(0, int.MaxValue)
                    .Select(_ => getEdges(current).Select(t => costs.TryGetValue(t.Location, out int cost) ? costs[t.Location] = Math.Min(cost, costs[current] + t.Weight) : costs[t.Location] = costs[current] + t.Weight).Count())
                    .Select(t => visited.Add(current) && (current = costs.MinBy(t => visited.Contains(t.Key) ? int.MaxValue : t.Value).Key) is { })
                    .TakeWhile(t => !visited.Contains(current))
                    .Count() is { } ?
            costs
        : throw null!
        ) is Func<int, Func<int, IEnumerable<(int Location, int Weight)>>, Dictionary<int, int>> dijkstra && dijkstra(input.IndexOf('S'), 
                i => deltas
                    .Select(d => i + d)
                    .Select(p => (p >= 0 && p < cleanInput.Length ? cleanInput[p] : '#', p))
                    .Where(t => t.Item1 == '.')
                    .Select(t => (t.p, 1))) is { } dists ?
            dists
                .Select(t => (Location: t.Key, Distance: t.Value, ValidCheats: cheats
                    .Select(d => (F: t.Key + d.F, S: t.Key + d.F + d.S))
                    .Where(t => t.F >= 0 && t.F < cleanInput.Length && t.S >= 0 && t.S < cleanInput.Length && !(cleanInput[t.F] == '.' || cleanInput[t.S] == '.'))))
                .Select(t => (t, t.ValidCheats.SelectMany(ct =>
                {
                    if(t.Location == 155)
                    {
                        var arr1 = deltas
                            .Select(d => dists.TryGetValue(ct.F + d, out int v) ? v : int.MaxValue);
                        var arr2 = arr1.Where(dist => dist != int.MaxValue);
                        var arr3 = arr2.Select(d => d - t.Distance - 2);
                    }

                    var arr = Enumerable
                                .Concat(
                        deltas
                        .Select(d => dists.TryGetValue(ct.S + d, out int v) ? v : int.MaxValue)
                            .Where(dist => dist != int.MaxValue)
                            .Select(d => (saved: d - t.Distance - 3, ct)),
                        deltas
                        .Select(d => dists.TryGetValue(ct.F + d, out int v) ? v : int.MaxValue)
                            .Where(dist => dist != int.MaxValue)
                            .Select(d => (saved: d - t.Distance - 2, ct)));

                    return arr;
                })))
                .Select(t =>
                {
                    var maxPath = t.Item2.MaxBy(t => t.saved);
                    var res = (t.t.Location, Distance: maxPath.saved);
                    Console.WriteLine(res.Distance);
                    var chararr = input.ToArray();
                    chararr[res.Location] = '█';
                    chararr[maxPath.ct.F] = '1';
                    chararr[maxPath.ct.S] = '2';
                    Console.WriteLine(new string(chararr));
                    Console.ReadLine();
                    return res;
                })
                .GroupBy(t => t.Distance)
                .Break()
                .Count()//155
        : throw null!;
}


