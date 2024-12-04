using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

internal class Day4 : ISolution
{
    public int Day => 4;

    

    public object? Solve1(string input) =>
        input.IndexOf('\n') is int width &&
        Array.Empty<int>().Aggregate(((int, int) a, (int, int) b) => (a.Item1 + b.Item1,  a.Item2 + b.Item2), (f, n) => null!) is
        Func<(int, int),(int, int),(int, int)> add &&
        new (int, int)[] {
        (-1, -1),
        (-1,  0),
        (-1, +1),
        ( 0, -1),
        ( 0, +1),
        (+1, -1),
        (+1,  0),
        (+1, +1) } is { } deltas ?
            input
                .Replace("\n", "")
                .Select((@char, index) => (index, @char))
                .Aggregate(new Dictionary<(int, int), char>(),
                      (dict, @char) => dict.TryAdd((@char.index % width, @char.index / width), @char.@char) is true ? dict : throw new Exception($"Key for {@char} already in dict!"))
                is { } table ? table.Select(kvp => 
                      deltas.Sum(dxy => !"XMAS"
                                 .Aggregate((false, kvp.Key), (flagpos, @char) => (flagpos.Item1 || !(table.TryGetValue(flagpos.Item2, out char v) && v == @char), add(flagpos.Item2, dxy)))
                                 .Item1 ? 1 : 0)).Sum() : throw null!
        : throw null!;



    public object? Solve2(string input) =>
        input.IndexOf('\n') is int width &&
        Array.Empty<int>().Aggregate((Dictionary<(int, int), char> t, (int, int) v) => t.TryGetValue(v, out char res) ? res : '\0', (f, n) => null!) is 
        Func<Dictionary<(int, int), char>, (int, int), char> getOrDefault &&
        Array.Empty<int>().Aggregate(((int, int) a, (int, int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2), (f, n) => null!) is
        Func<(int, int), (int, int), (int, int)> add &&
        Array.Empty<int>().Aggregate((Dictionary<(int, int), char> words, (int, int) loc) => words[loc] == 'A' 
            && ((getOrDefault(words, add(loc, (1, 1))) == 'M' && getOrDefault(words, add(loc, (-1, -1))) == 'S') || (getOrDefault(words, add(loc, (1, 1))) == 'S' && getOrDefault(words, add(loc, (-1, -1))) == 'M'))
            && ((getOrDefault(words, add(loc, (-1, 1))) == 'M' && getOrDefault(words, add(loc, (1, -1))) == 'S') || (getOrDefault(words, add(loc, (-1, 1))) == 'S' && getOrDefault(words, add(loc, (1, -1))) == 'M'))
        , (f, n) => null!) is
        Func<Dictionary<(int, int), char>, (int, int), bool> checkIsXMAS ?
            input
                .Replace("\n", "")
                .Select((@char, index) => (index, @char))
                .Aggregate(new Dictionary<(int, int), char>(),
                      (dict, @char) => dict.TryAdd((@char.index % width, @char.index / width), @char.@char) is true ? dict : throw new Exception($"Key for {@char} already in dict!"))
                is { } table ? table.Where(kvp => checkIsXMAS(table, kvp.Key)).Count() : throw null!
        : throw null!;
}
