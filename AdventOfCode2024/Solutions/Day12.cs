using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions;
internal class Day12 : ISolution
{
    public int Day => 12;
    public object? Solve1(string input) => 
        input.IndexOf('\n') is int inputWidth &&
        new int[] { 1, -1, -inputWidth - 1, inputWidth + 1 } is { } deltas && 
        new HashSet<int>() is { } large ?
            input
            .Select((c, i) => (i, c))
            .Where(c => c.c != '\n')
            .Select(c => (object)((object self, string str, int width, int x, char tile, int[] deltas, HashSet<int> set, HashSet<int> total) =>
            x >= str.Length ||
            x < 0 ||
            str[x] != tile ||
            !total.Add(x) ||
            !set.Add(x) ||
            self is not Func<object, string, int, int, char, int[], HashSet<int>, HashSet<int>, HashSet<int>> func ?
                set : deltas
                    .Select(i => func(self, str, width, x + i, tile, deltas, set, total))
                    .Where(t => true)
                    .Last()) is Func<object, string, int, int, char, int[], HashSet<int>, HashSet<int>, HashSet<int>> func ? 
                    (set: func(func, input, inputWidth, c.i, c.c, deltas, new(), large), c.c) : throw null!)
            .Where(t => t.set.Count != 0)
            .Sum(t => t.set.Count * t.set.Sum(p => deltas.Where(d => p + d is int x && (x < 0 || x >= input.Length || input[x] != t.c)).Count()))
        : throw null!;

    public object? Solve2(string input) =>
        input.IndexOf('\n') is int inputWidth &&
        new int[] { 1, -1, -inputWidth - 1, inputWidth + 1 } is { } deltas &&
        new HashSet<int>() is { } large ?
            input
            .Select((c, i) => (i, c))
            .Where(c => c.c != '\n')
            .Select(c => (object)((object self, string str, int width, int x, char tile, int[] deltas, HashSet<int> set, HashSet<int> total) =>
            x >= str.Length ||
            x < 0 ||
            str[x] != tile ||
            !total.Add(x) ||
            !set.Add(x) ||
            self is not Func<object, string, int, int, char, int[], HashSet<int>, HashSet<int>, HashSet<int>> func ?
                set : deltas
                    .Select(i => func(self, str, width, x + i, tile, deltas, set, total))
                    .Where(t => true)
                    .Last()) is Func<object, string, int, int, char, int[], HashSet<int>, HashSet<int>, HashSet<int>> func ?
                    (set: func(func, input, inputWidth, c.i, c.c, deltas, new(), large), c.c) : throw null!)
            .Where(t => t.set.Count != 0)
            .Sum(t => CountSides(t.set, inputWidth + 1, deltas, input, t.c) * t.set.Count)
        : throw null!;

    private static int CountSides(HashSet<int> sides, int width, int[] deltas, string input, char c)
    {
        var x = sides.SelectMany(p => deltas.Where(d => p + d is int x && (x < 0 || x >= input.Length || input[x] != c)).Select(d => (normal: d, pos: p))).ToArray();
        var y = x.GroupBy(t => Math.Abs(t.normal) == 1 ? (t.normal, edgepos: t.pos % width) : (t.normal, edgepos: t.pos / width)).ToArray();
        return y.Length;
    }
}