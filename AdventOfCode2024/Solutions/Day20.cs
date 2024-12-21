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
        new int[] { 1, -1, w, -w, } is { } deltas &&
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
        : throw null!) is Func<int, Func<int, IEnumerable<(int Location, int Weight)>>, Dictionary<int, int>> dijkstra && dijkstra(input.IndexOf('S'), 
                i => deltas
                    .Select(d => i + d)
                    .Select(p => (p >= 0 && p < cleanInput.Length ? cleanInput[p] : '#', p))
                    .Where(t => t.Item1 == '.')
                    .Select(t => (t.p, 1))) is { } dists ?
            dists
                .Select(t => (Location: t.Key, Distance: t.Value, ValidCheats: deltas
                    .Select(d => t.Key + d)
                    .Where(t => t >= 0 && t < cleanInput.Length && cleanInput[t] != '.')))
                .Select(t => (t, t.ValidCheats.SelectMany(c => deltas
                        .Select(d => dists.TryGetValue(c + d, out int v) ? v : int.MaxValue)
                            .Where(dist => dist != int.MaxValue)
                            .Select(d => (saved: d - t.Distance - 2, c)))))
                .SelectMany(t => t.Item2.Select(t => t.saved))
                .Where(t => t >= 100)
                .Count()
        : throw null!;

    public object? Solve2(string input) =>
        input.Replace("E", ".").Replace("S", ".") is { } cleanInput &&
        input.IndexOf('\n') + 1 is { } w &&
        new int[] { 1, -1, w, -w, } is { } deltas &&
        Enumerable.Range(-20, 41).SelectMany(i => /*[-20, 20]*/ Math.Abs(i) - 20 is int count ? Enumerable.Range(count, -count * 2 + 1).Select(t => (dx: i, dy: t)) : default).ToArray() is 
            (int dx, int dy)[] taxiCircle &&
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
        : throw null!) is Func<int, Func<int, IEnumerable<(int Location, int Weight)>>, Dictionary<int, int>> dijkstra && dijkstra(input.IndexOf('S'),
                i => deltas
                    .Select(d => i + d)
                    .Select(p => (p >= 0 && p < cleanInput.Length ? cleanInput[p] : '#', p))
                    .Where(t => t.Item1 == '.')
                    .Select(t => (t.p, 1))) is { } dists ?
            dists
                .Select(t => (X: t.Key % w, Y: t.Key / w, Distance: t.Value))
                .SelectMany(t => taxiCircle
                    .Select(dxy => (X: dxy.dx + t.X, Y: dxy.dy + t.Y, t.Distance, dxy.dx, dxy.dy, O: t))
                    .Where(p => p.X > 0 && p.Y > 0 && p.Y < input.Length / w && p.X < w && dists.ContainsKey(p.X + p.Y * w))
                    .Select(p => (CheatInfo: p, CheatScore: dists[p.X + p.Y * w] - t.Distance - (Math.Abs(p.dx) + Math.Abs(p.dy))))
                    .Where(t => t.CheatScore >= 100))
                .Count()
        : throw null!;
}


