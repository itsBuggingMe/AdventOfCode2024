using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions;
internal class Day10 : ISolution
{
    public int Day => 10;
    public object? Solve1(string input) => 
        new Func<string, int, int, int, HashSet<int>, int>[1] is { } arr &&
        (arr[0] = (string str, int width, int x, int pElevation, HashSet<int> set) => 
            x >= str.Length || x < 0 || str[x] == '\n' || str[x] == '.' || str[x] - '0' != ++pElevation || !set.Add(x) ?
                0 : str[x] == '9' ? 1 : arr[0](str, width, x + 1, pElevation, set) + 
                    arr[0](str, width, x - 1, pElevation, set) + 
                    arr[0](str, width, x - width - 1, pElevation, set) + 
                    arr[0](str, width, x + width + 1, pElevation, set)) is { } recur ? input
            .Select((i, j) => (j, i))
            .Where(t => t.i == '0')
            .Sum(t => recur(input, input.IndexOf('\n'), t.j, -1, new HashSet<int>()))
                : throw null!;
    public object? Solve2(string input) =>
        new Func<string, int, int, int, int, int>[1] is { } arr &&
        (arr[0] = (string str, int width, int x, int pElevation, int prevLocation) => 
            x >= str.Length || x < 0 || str[x] == '\n' || str[x] == '.' || str[x] - '0' != ++pElevation || prevLocation == x ?
                0 : str[x] == '9' ? 1 : 
                    arr[0](str, width, x + 1,         pElevation, x) + 
                    arr[0](str, width, x - 1,         pElevation, x) + 
                    arr[0](str, width, x - width - 1, pElevation, x) + 
                    arr[0](str, width, x + width + 1, pElevation, x)) is { } recur ? input
            .Select((i, j) => (j, i))
            .Where(t => t.i == '0')
            .Sum(t => recur(input, input.IndexOf('\n'), t.j, -1, -10))
                : throw null!;
}
