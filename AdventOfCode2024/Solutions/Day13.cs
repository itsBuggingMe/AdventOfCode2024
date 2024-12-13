using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions;

internal class Day13 : ISolution
{
    public int Day => 13;
    public object? Solve1(string input) => Regex.Matches(input, @"(\d+)\D*(\d+)\D*(\d+)\D*(\d+)\D*(\d+)\D*(\d+)")
        .Select(m => (a1: double.Parse(m.Groups[1].ValueSpan), a2: double.Parse(m.Groups[2].ValueSpan), b1: double.Parse(m.Groups[3].ValueSpan), b2: double.Parse(m.Groups[4].ValueSpan), c1: double.Parse(m.Groups[5].ValueSpan), c2: double.Parse(m.Groups[6].ValueSpan)))
        .Select(t => -t.a1 / t.a2 is { } m && 
                  (t.c1 + t.c2 * m) / (t.b1 + m * t.b2) is { } y &&
                  (-t.b1 * y + t.c1) / t.a1 is { } x ? (x, y) : throw null!)
        .Sum(t => Math.Abs(Math.Round(t.x) + Math.Round(t.y) - t.x - t.y) < 0.0001 && t.x < 100 && t.y < 100 ? t.x * 3 + t.y : 0);
    
    
    public object? Solve2(string input) => Regex.Matches(input, @"(\d+)\D*(\d+)\D*(\d+)\D*(\d+)\D*(\d+)\D*(\d+)")
        .Select(m => (
            a1: double.Parse(m.Groups[1].ValueSpan), 
            a2: double.Parse(m.Groups[2].ValueSpan), 
            b1: double.Parse(m.Groups[3].ValueSpan), 
            b2: double.Parse(m.Groups[4].ValueSpan), 
            c1: double.Parse(m.Groups[5].ValueSpan) + 10000000000000, 
            c2: double.Parse(m.Groups[6].ValueSpan) + 10000000000000))
        .Select(t => -t.a1 / t.a2 is { } m && 
                  (t.c1 + t.c2 * m) / (t.b1 + m * t.b2) is { } y &&
                  (-t.b1 * y + t.c1) / t.a1 is { } x ? (x, y) : throw null!)
        .Sum(t => 
            Math.Abs(Math.Round(t.x) - t.x) < 0.001 && 
            Math.Abs(Math.Round(t.y) - t.y) < 0.001 ? t.x * 3 + t.y : 0);
}