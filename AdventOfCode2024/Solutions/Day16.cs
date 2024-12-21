using System.Data;
using System.Linq;

namespace AdventOfCode2024.Solutions;

internal class Day16 : ISolution
{
    static Dictionary<(int, int), int> kvp = new Dictionary<(int, int), int>();
    public int Day => 16;
    public object? Solve1(string input) =>
        input.IndexOf('\n') + 1 is { } width &&
        new int[]
        {
            -width,
            1,
            width,
            -1
        } is { } deltaTable &&
        (object)(((int, int) current, Func<(int, int), IEnumerable<((int, int) Location, int Weight)>> getEdges) =>
                new HashSet<(int, int)>() is { } visited &&
                (kvp = new Dictionary<(int, int), int>
                {
                    { current, 0 },
                }) is { } costs &&
                Enumerable
                        .Range(0, int.MaxValue)
                        .Select(_ => getEdges(current).Select(t => costs.TryGetValue(t.Location, out int cost) ? costs[t.Location] = Math.Min(cost, costs[current] + t.Weight) : costs[t.Location] = costs[current] + t.Weight).Count())
                        .Select(t => visited.Add(current) && (current = costs.MinBy(t => visited.Contains(t.Key) ? int.MaxValue : t.Value).Key) is { })
                        .TakeWhile(t => !visited.Contains(current))
                        .Count() is { } ?
                costs
            : throw null!) is Func<(int, int), Func<(int Pos, int Dir), IEnumerable<((int, int) Location, int Weight)>>, Dictionary<(int, int), int>> dijkstra &&
        dijkstra((input.IndexOf('S'), 1), t =>
        {
            return t.Pos + deltaTable[t.Dir] is int forward && forward >= 0 && forward < input.Length && input[forward] != '\n' && input[forward] != '#' ?
            new ((int, int), int)[] {
                    ((forward, t.Dir), 1),
                    ((t.Pos, (t.Dir + 4 + 1) % 4), 1000),
                    ((t.Pos, (t.Dir + 4 - 1) % 4), 1000) }
                : new ((int, int), int)[] {
                    ((t.Pos, (t.Dir + 4 + 1) % 4), 1000),
                    ((t.Pos, (t.Dir + 4 - 1) % 4), 1000) };
        }) is { } dists ?
            new int[] { dists[(input.IndexOf('E'), 0)], dists[(input.IndexOf('E'), 1)], dists[(input.IndexOf('E'), 2)], dists[(input.IndexOf('E'), 3)] }.Min()
        : throw null!;

    public object? Solve2(string input) => input.IndexOf('\n') + 1 is { } width && input.IndexOf('E') is { } exit &&
        new int[]
        {
            -width,
            1,
            width,
            -1
        } is { } deltaTable &&
        (object)(((int, int) current, Func<(int, int), IEnumerable<((int, int) Location, int Weight)>> getEdges) =>
                new HashSet<(int, int)>() is { } visited &&
                (kvp = new Dictionary<(int, int), int>
                {
                    { current, 0 },
                }) is { } costs &&
                Enumerable
                        .Range(0, int.MaxValue)
                        .Select(_ => getEdges(current).Select(t => costs.TryGetValue(t.Location, out int cost) ? costs[t.Location] = Math.Min(cost, costs[current] + t.Weight) : costs[t.Location] = costs[current] + t.Weight).Count())
                        .Select(t => visited.Add(current) && (current = costs.MinBy(t => visited.Contains(t.Key) ? int.MaxValue : t.Value).Key) is { })
                        .TakeWhile(t => !visited.Contains(current))
                        .Count() is { } ?
                costs
            : throw null!) is Func<(int, int), Func<(int Pos, int Dir), IEnumerable<((int, int) Location, int Weight)>>, Dictionary<(int, int), int>> dijkstra &&
        dijkstra((input.IndexOf('S'), 1), t =>
        {
            return t.Pos + deltaTable[t.Dir] is int forward && forward >= 0 && forward < input.Length && input[forward] != '\n' && input[forward] != '#' ?
            new ((int, int), int)[] {
                    ((forward, t.Dir), 1),
                    ((t.Pos, (t.Dir + 4 + 1) % 4), 1000),
                    ((t.Pos, (t.Dir + 4 - 1) % 4), 1000) }
                : new ((int, int), int)[] {
                    ((t.Pos, (t.Dir + 4 + 1) % 4), 1000),
                    ((t.Pos, (t.Dir + 4 - 1) % 4), 1000) };
        }) is { } dists &&
        (object)((object self, HashSet<(int, int)> set, (int Pos, int Dir) current, int expected) => 
            self is Func<object, HashSet<(int, int)>, (int Pos, int Dir), int, HashSet<(int, int)>> x && 
            current.Pos >= 0 && 
            current.Pos < input.Length && 
            input[current.Pos] != '\n' && 
            input[current.Pos] != '#' && 
            dists[current] == expected && 
            set.Add(current) ?
                (x(x, set, (current.Pos, (current.Dir + 4 + 1) % 4), expected - 1000),
                x(x, set, (current.Pos, (current.Dir + 4 - 1) % 4), expected - 1000),
                x(x, set, (current.Pos - deltaTable[current.Dir], current.Dir), expected - 1)).Item1 : set)
        is Func<object, HashSet<(int, int)>, (int Pos, int Dir), int, HashSet<(int, int)>> floodfill &&
        Enumerable.Range(0, 4).MinBy(t => dists[(exit, t)]) is { } mindex ?
            floodfill(floodfill, new(), (exit, mindex), dists[(exit, mindex)]).DistinctBy(t => t.Item1).Count()
        : throw null!;
}
