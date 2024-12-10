using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions;
internal class Day9 : ISolution
{
    public int Day => 9;
    public object? Solve1(string input) =>
        input.SelectMany((i, index) => Enumerable.Range(0, i - '0').Select(n => index % 2 == 0 ? index / 2 : -1)).ToArray() is
        int[] buff &&
        new int[2] { 0, buff.Length } is int[] vars &&
        Enumerable.Range(0, int.MaxValue)
            .Select(s => buff[s] == -1 ? (buff[s] = buff.Take(vars[1]).Reverse().First(i => (buff[--vars[1]] = -1) * 0 + i != -1)) * 0 + s : s)
            .TakeWhile(i => i < vars[1])
            .Count() != -1 ?
                buff.Where(i => i != -1).Select((i, j) => (long)i * j).Sum()
        : throw null!;

    public object? Solve2(string input) =>
        input.SelectMany((i, index) => Enumerable.Range(0, i - '0').Select(n => index % 2 == 0 ? index / 2 : -1)).ToArray() is
        int[] buff &&
            buff
                .Select((id, index) => (id, index))
                .GroupBy(i => i.id)
                .Where(g => g.Key != -1)
                .Select(g => (Id: g.Key, Index: g.First().index, Length: g.Count()))
                .Reverse()
                .ToArray()
                .Count(file => Enumerable.Range(0, buff.Length).FirstOrDefault(i => buff.AsSpan(i).StartsWith((stackalloc int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1, }).Slice(0, file.Length))) is 
                    int replacementIndex && replacementIndex != 0 && replacementIndex < file.Index ? Enumerable.Range(0, file.Length).Count(offset => (buff[replacementIndex + offset] = buff[file.Index + offset]) + (buff[file.Index + offset] = -1) == 0) == 0
                : false) is { } ?
                buff.Select((i, j) => i == -1 ? 0 : (long)i * j).Sum()
        : throw null!;
}
