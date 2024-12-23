using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions;
internal class Day23 : ISolution
{
    public int Day => 23;

    public object? Solve1(string input) => input
        .Split('\n')
        .Select(t => (new string(t.AsSpan(0, 2)), new string(t.AsSpan(3, 2))))
        .SelectMany(t => new (string A, string B)[] { t, (t.Item2, t.Item1) })
        .Aggregate(new Dictionary<string, HashSet<string>>(), (dic, str) => (CollectionsMarshal.GetValueRefOrAddDefault(dic, str.A, out _) ??= new()).Add(str.B) ? dic : dic)
        is { } edges ?
            edges
                .Where(t => t.Value.Count >= 2 && t.Key[0] == 't')
                .SelectMany(t => t.Value.Append(t.Key)
                    .SelectMany(s => t.Value.Select(z => (s, z)))
                    .SelectMany(s => t.Value.Select(z => (A: s.s, B: s.z, C: z)))
                    .Where(t => t.A[0] == 't' || t.B[0] == 't' || t.C[0] == 't')
                    .Where(t => edges[t.A].Contains(t.B) && edges[t.A].Contains(t.C) && edges[t.B].Contains(t.C)))
                .DistinctBy(t => unchecked(t.A.GetHashCode() + t.B.GetHashCode() + t.C.GetHashCode()))
                .Count()
        : throw null!;

    public object? Solve2(string input) => input
        .Split('\n')
        .Select(t => (new string(t.AsSpan(0, 2)), new string(t.AsSpan(3, 2))))
        .SelectMany(t => new (string A, string B)[] { t, (t.Item2, t.Item1) })
        .Aggregate(new Dictionary<string, HashSet<string>>(), (dic, str) => (CollectionsMarshal.GetValueRefOrAddDefault(dic, str.A, out _) ??= new()).Add(str.B) ? dic : dic)
        is { } edges && new HashSet<string>() is { } visited && new HashSet<string>() is { } maxClique &&
        edges
                .Where(e => !visited.Contains(e.Key))
                .Select(e => new HashSet<string> { e.Key } is { } clique && e.Value.Where(s => clique.All(c => edges[c].Contains(s))).Select(s => clique.Add(s)).Count() > -1 ? clique : throw null!)
                .Select(e => e.Count > maxClique.Count ? maxClique = e : null)
                .Count() != -1 ?
            string.Join(',', maxClique.OrderBy(t => t))
        : throw null!;
}