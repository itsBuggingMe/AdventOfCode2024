using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;   

internal class Day19 : ISolution
{
    public int Day => 19;
    public object? Solve1(string input) => 
        input[..input.IndexOf("\n\n")].Split(',').Select(t => t.Trim()).ToArray() is { } towels &&
        (object)((object self, string match, int start, string?[] b, Dictionary<int, bool> cache) => 
            cache
                .TryGetValue(match.AsMemory(start).GetHashCode(), out var x) ? 
                    x : 
            (cache[match.AsMemory(start).GetHashCode()] = b
                .Any(f => match.Length == start || 
                    match.AsSpan(start).StartsWith(f) && 
                    ((Func<object, string, int, string?[], Dictionary<int, bool>, bool>)self)(self, match, start + f.Length, b, cache))))
        is Func<object, string, int, string?[], Dictionary<int, bool>, bool> pattern ?
            input[input.IndexOf("\n\n")..]
                .Split('\n')
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .Count(t => pattern(pattern, t, 0, towels, new()))
        : throw null!;
    public object? Solve2(string input) =>
        input[..input.IndexOf("\n\n")].Split(',').Select(t => t.Trim()).ToArray() is { } towels &&
        (object)((object self, string match, int start, string?[] b, Dictionary<int, long> cache) => cache
            .TryGetValue(match.AsMemory(start).GetHashCode(), out long x) ? 
                x : 
            (start == match.Length ? 
                1 : 
            cache[match.AsMemory(start).GetHashCode()] = b
                .Sum(f => match.AsSpan(start).StartsWith(f) ? ((Func<object, string, int, string?[], Dictionary<int, long>, long>)self)(self, match, start + f.Length, b, cache) : 0L)))
        is Func<object, string, int, string?[], Dictionary<int, long>, long> pattern ?
            input[input.IndexOf("\n\n")..]
                .Split('\n')
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .Sum(t => pattern(pattern, t, 0, towels, new()))
        : throw null!;
}


