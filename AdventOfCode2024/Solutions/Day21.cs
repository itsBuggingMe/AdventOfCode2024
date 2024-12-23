using System.Diagnostics;
using System.Text;

namespace AdventOfCode2024.Solutions;

internal class Day21 : ISolution
{
    public int Day => 21;
    public object? Solve1(string input) => (object)(((int x, int y) a, (int x, int y) b) => (a.x - b.x, a.y - b.y)) is
        Func<(int, int), (int, int), (int X, int Y)> sub &&
        (object)(((int x, int y) a, (int x, int y) b) => (a.x + b.x, a.y + b.y)) is
        Func<(int, int), (int, int), (int, int)> add &&
        new Dictionary<char, (int, int)>
        {
            { 'A', (0, 0) },
            { '^', (-1, 0) },
            { '<', (-2, 1) },
            { 'v', (-1, 1) },
            { '>', (0, 1) },
        } is { } directional &&
        new Dictionary<char, (int, int)>
        {
            { 'A', (0, 0)   },
            { '0', (-1, 0)  },
            { '1', (-2, -1) },
            { '2', (-1, -1) },
            { '3', (0, -1)  },
            { '4', (-2, -2) },
            { '5', (-1, -2) },
            { '6', (0, -2)  },
            { '7', (-2, -3) },
            { '8', (-1, -3) },
            { '9', (0, -3)  },
        } is { } numeric &&

        (object)((object self, (int, int) current, char type, int depth, Dictionary<((int, int), (int, int), int), long> cache) =>
            (depth == 0 ? numeric : directional)[type] is { } next && current is { } oldCurrent && sub(next, current) is { } delta && cache.TryGetValue((current, next, depth), out long cachedValue) ? cachedValue :
            new StringBuilder() switch
            {
                var s when delta.X < 0 && (current = add(current, (delta.X, 0))) is (-2, 0) => s.Append(delta.Y < 0 ? '^' : 'v', Math.Abs(delta.Y)).Append('<', -delta.X).Append('A'),
                var s when delta.X < 0 => s.Append('<', -delta.X),
                var s when true => s,
            } switch
            {
                var s when s.Length > 0 && s[^1] == 'A' => s,
                var s when delta.Y > 0 && (current = add(current, (0, delta.Y))) is (-2, 0) => s.Append(delta.X < 0 ? '<' : '>', Math.Abs(delta.X)).Append('v', delta.Y).Append('A'),
                var s when delta.Y > 0 => s.Append('v', delta.Y),
                var s when true => s,
            } switch
            {
                var s when s.Length > 0 && s[^1] == 'A' => s,
                var s when delta.Y < 0 && (current = add(current, (0, delta.Y))) is (-2, 0) => s.Append(delta.X < 0 ? '<' : '>', Math.Abs(delta.X)).Append('^', -delta.Y).Append('A'),
                var s when delta.Y < 0 => s.Append('^', -delta.Y),
                var s when true => s,
            } switch
            {
                var s when s.Length > 0 && s[^1] == 'A' => s,
                var s when delta.X > 0 && (current = add(current, (delta.X, 0))) is (-2, 0) => s.Append(delta.Y < 0 ? '^' : 'v', Math.Abs(delta.Y)).Append('>', delta.X).Append('A'),
                var s when delta.X > 0 => s.Append('>', delta.X).Append('A'),
                var s when true => s.Append('A'),
            } is StringBuilder sb ? cache[(oldCurrent, next, depth)] = cachedValue = depth == 2/*5*/ ? sb.Length : sb.ToString().Aggregate((pos: (0, 0), accum: 0L), (acc, c) => (directional[c], acc.Item2 + ((Func<object, (int, int), char, int, Dictionary<((int, int), (int, int), int), long>, long>)self)(self, acc.pos, c, depth + 1, cache))).accum : throw null!)
        is Func<object, (int, int), char, int, Dictionary<((int, int), (int, int), int), long>, long> genSeq &&
        new Dictionary<((int, int), (int, int), int), long>() is { } cache ?
            input
            .Split('\n')
            .Select(i => i.Aggregate((pos: (0, 0), accum: 0L), (acc, c) => (numeric[c], acc.accum + genSeq(genSeq, acc.pos, c, 0, cache))).accum * int.Parse(i.AsSpan()[..^1]))
            .Sum()  
        : throw null!;

    public object? Solve2(string input) => (object)(((int x, int y) a, (int x, int y) b) => (a.x - b.x, a.y - b.y)) is
        Func<(int, int), (int, int), (int X, int Y)> sub &&
        (object)(((int x, int y) a, (int x, int y) b) => (a.x + b.x, a.y + b.y)) is
        Func<(int, int), (int, int), (int, int)> add &&
        new Dictionary<char, (int, int)>
        {
            { 'A', (0, 0) },
            { '^', (-1, 0) },
            { '<', (-2, 1) },
            { 'v', (-1, 1) },
            { '>', (0, 1) },
        } is { } directional &&
        new Dictionary<char, (int, int)>
        {
            { 'A', (0, 0)   },
            { '0', (-1, 0)  },
            { '1', (-2, -1) },
            { '2', (-1, -1) },
            { '3', (0, -1)  },
            { '4', (-2, -2) },
            { '5', (-1, -2) },
            { '6', (0, -2)  },
            { '7', (-2, -3) },
            { '8', (-1, -3) },
            { '9', (0, -3)  },
        } is { } numeric &&

        (object)((object self, (int, int) current, char type, int depth, Dictionary<((int, int), (int, int), int), long> cache) =>
            (depth == 0 ? numeric : directional)[type] is { } next && current is { } oldCurrent && sub(next, current) is { } delta && cache.TryGetValue((current, next, depth), out long cachedValue) ? cachedValue :
            new StringBuilder() switch
            {
                var s when delta.X < 0 && (current = add(current, (delta.X, 0))) is (-2, 0) => s.Append(delta.Y < 0 ? '^' : 'v', Math.Abs(delta.Y)).Append('<', -delta.X).Append('A'),
                var s when delta.X < 0 => s.Append('<', -delta.X),
                var s when true => s,
            } switch
            {
                var s when s.Length > 0 && s[^1] == 'A' => s,
                var s when delta.Y > 0 && (current = add(current, (0, delta.Y))) is (-2, 0) => s.Append(delta.X < 0 ? '<' : '>', Math.Abs(delta.X)).Append('v', delta.Y).Append('A'),
                var s when delta.Y > 0 => s.Append('v', delta.Y),
                var s when true => s,
            } switch
            {
                var s when s.Length > 0 && s[^1] == 'A' => s,
                var s when delta.Y < 0 && (current = add(current, (0, delta.Y))) is (-2, 0) => s.Append(delta.X < 0 ? '<' : '>', Math.Abs(delta.X)).Append('^', -delta.Y).Append('A'),
                var s when delta.Y < 0 => s.Append('^', -delta.Y),
                var s when true => s,
            } switch
            {
                var s when s.Length > 0 && s[^1] == 'A' => s,
                var s when delta.X > 0 && (current = add(current, (delta.X, 0))) is (-2, 0) => s.Append(delta.Y < 0 ? '^' : 'v', Math.Abs(delta.Y)).Append('>', delta.X).Append('A'),
                var s when delta.X > 0 => s.Append('>', delta.X).Append('A'),
                var s when true => s.Append('A'),
            } is StringBuilder sb ? cache[(oldCurrent, next, depth)] = cachedValue = depth == 25 ? sb.Length : sb.ToString().Aggregate((pos: (0, 0), accum: 0L), (acc, c) => (directional[c], acc.Item2 + ((Func<object, (int, int), char, int, Dictionary<((int, int), (int, int), int), long>, long>)self)(self, acc.pos, c, depth + 1, cache))).accum : throw null!)
        is Func<object, (int, int), char, int, Dictionary<((int, int), (int, int), int), long>, long> genSeq &&
        new Dictionary<((int, int), (int, int), int), long>() is { } cache ?
            input
            .Split('\n')
            .Select(i => i.Aggregate((pos: (0, 0), accum: 0L), (acc, c) => (numeric[c], acc.accum + genSeq(genSeq, acc.pos, c, 0, cache))).accum * int.Parse(i.AsSpan()[..^1]))
            .Sum()
        : throw null!;
}
