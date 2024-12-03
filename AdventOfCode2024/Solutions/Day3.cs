using System.Text;

namespace AdventOfCode2024.Solutions;

internal class Day2 : ISolution
{
    public int Day => 3;

    public object Solve1(string input) => Regex
      .Matches(input, @"(?<=mul\()[0-9]+,[0-9]+(?=\))")
      .Sum(s => s.Value.IndexOf(',') is { } index ? 
           (int.Parse(s.ValueSpan[..index]) * int.Parse(s.ValueSpan[(index+1)..]))
           : 1);

    public object Solve2(string input) => null!;
}
