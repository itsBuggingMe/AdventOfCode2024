using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions;
internal class Day11 : ISolution
{
    public int Day => 11;
    public object? Solve1(string input) => 
        (object)((object obj, long n, int cycle) => 
            obj is Func<object, long, int, long> self ?
                 cycle-- == 0 ?
                     1 :
                     n == 0 ?
                         self(self, 1, cycle) :
                             (long)Math.Ceiling(Math.Log10(n + 1)) is { } digits &&
                             digits % 2 == 0  && (long)Math.Pow(10, digits / 2) is { } block ?
                                 self(self, n / block, cycle) + self(self, n % block, cycle) :
                                     self(self, n * 2024, cycle)
                 : throw null!) is
        Func<object, long, int, long> recur ?
        input
        .Split(' ')
        .Select(int.Parse)
        .Sum(f => recur(recur, f, 25))
        : throw null!;

    public object? Solve2(string input) => 
        (object)((object obj, Dictionary<(long N, int Cycle), long> cache, long n, int cycle) => 
            cache.TryGetValue((n, cycle), out long count) ? count :
                cache[(n, cycle)] = obj is Func<object, Dictionary<(long Start, int Left), long>, long, int, long> self ?
                 cycle-- == 0 ?
                     1 :
                     n == 0 ?
                         self(self, cache, 1, cycle) :
                             (long)Math.Ceiling(Math.Log10(n + 1)) is { } digits &&
                             digits % 2 == 0  && (long)Math.Pow(10, digits / 2) is { } block ?
                                 self(self, cache, n / block, cycle) + self(self, cache, n % block, cycle) :
                                     self(self, cache, n * 2024, cycle)
                 : throw null!) is
        Func<object, Dictionary<(long Start, int Left), long>, long, int, long> recur ?
        input
            .Split(' ')
            .Select(int.Parse)
            .Sum(f => recur(recur, new(), f, 75))
            : throw null!;
}