using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

internal class Day24 : ISolution
{
    public int Day => 24;

    public object? Solve1(string input) =>
        input.Split("\n\n") is { } sect &&
        Regex.Matches(sect[0], @"([a-z]\d+)..(\d)")
            .Select(m => (m.Groups[1].ToString(), int.Parse(m.Groups[2].ValueSpan) == 1))
            .ToDictionary(m => m.Item1, m => m.Item2) is { } circuitValues &&
        Regex.Matches(sect[1], @"(\w+).(\w+).(\w+)....(\w+)")
            .Select(m => (A: m.Groups[1].Value, OP: m.Groups[2].Value, B: m.Groups[3].Value, Out: m.Groups[4].Value))
            .ToArray() is (string A, string OP, string B, string Out)[] thing &&
        Enumerable.Range(0, int.MaxValue)
            .Select(_ => thing = thing.Where(t => !(circuitValues.TryGetValue(t.A, out var av) && circuitValues.TryGetValue(t.B, out var bv) && ((circuitValues[t.Out] = t.OP switch
            {
                "AND" => av & bv,
                "OR" => av | bv,
                "XOR" => av ^ bv,
                _ => throw new Exception("unknown op"),
            }) | true))).ToArray()).TakeWhile(t => thing.Length > 0).Count() != -1 ?
            circuitValues
                .Where(k => k.Key.StartsWith('z'))
                .Select(k => (int.Parse(k.Key.AsSpan(1)), k.Value))
                .OrderBy(i => -i.Item1)
                .Select(i => i.Item2)
                .Aggregate(0L, (accum, n) => (accum << 1) | (n ? 1L : 0L))
        : throw null!;

    public object? Solve2(string input) =>
        input.Split("\n\n") is { } sect &&
        Regex.Matches(sect[0], @"([a-z]\d+)..(\d)")
            .Select(m => (m.Groups[1].ToString(), int.Parse(m.Groups[2].ValueSpan) == 1))
            .ToDictionary(m => m.Item1, m => m.Item2) is { } circuitValues &&
        Regex.Matches(sect[1], @"(\w+).(\w+).(\w+)....(\w+)")
            .Select(m => (A: m.Groups[1].Value, OP: m.Groups[2].Value, B: m.Groups[3].Value, Out: m.Groups[4].Value))
            .ToArray() is { } gates &&
        (circuitValues
                .Where(k => k.Key.StartsWith('x'))
                .Select(k => (int.Parse(k.Key.AsSpan(1)), k.Value))
                .OrderByDescending(t => t.Item1)
                .Aggregate(0L, (accum, n) => (accum << 1) | (n.Value ? 1L : 0L)) + circuitValues
                .Where(k => k.Key.StartsWith('y'))
                .Select(k => (int.Parse(k.Key.AsSpan(1)), k.Value))
                .OrderByDescending(t => t.Item1)
                .Aggregate(0L, (accum, n) => (accum << 1) | (n.Value ? 1L : 0L))) is { } answer &&
            gates.Select((g, i) => (i, g)).Aggregate(new Dictionary<string, HashSet<(int ID, (string A, string OP, string B, string Out))>>(),
                (d, s) => ((CollectionsMarshal.GetValueRefOrAddDefault(d, s.g.A, out _) ??= new()).Add((s.i, s.g)) && (CollectionsMarshal.GetValueRefOrAddDefault(d, s.g.B, out _) ??= new()).Add((s.i, s.g))) ? d : d)
        is { } wireToGate &&
                gates
                .Select((w, i) => (ID: i, w.OP, w.Out, w.A, w.B, Set: wireToGate.TryGetValue(w.Out, out var v) ? v : new()))
                .Where(w => (!(w.OP switch
                {
                    "OR" => w.Set.Any(g => g.Item2.OP == "XOR"),
                    "AND" => w.Set.Any(g => g.Item2.OP == "OR"),
                    "XOR" => w.Set.Any(g => g.Item2.OP == "XOR") || w.Out[0] == 'z',
                    _ => throw new Exception("Unknown Op")
                }) &&
                   (w.OP != "AND" || !(wireToGate["y00"].Any(i => i.ID == w.ID) && wireToGate["x00"].Any(i => i.ID == w.ID))) || (w.OP == "XOR" && gates.Where(g => (g.A == w.Out || g.B == w.Out) && g.OP == "AND" && gates.Count(g1 => g1.OP == "XOR" && (g1.Out == g.A || g1.Out == g.B)) >= 2).Any() && gates.Any(g => g.OP == "XOR" && (g.Out == w.A || g.Out == w.B))))
                ).OrderBy(t => t.Out).Reverse().Skip(1).ToArray() is { } candidates ?
            string.Join(',', candidates.Select(c => c.Out).Order())
        : throw null!;
}
