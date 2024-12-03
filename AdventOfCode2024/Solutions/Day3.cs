using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

internal class Day3 : ISolution
{
    public int Day => 3;

    public object Solve1(string input) => Regex
      .Matches(input, @"(?<=mul\()[0-9]+,[0-9]+(?=\))")
      .Sum(s => s.Value.IndexOf(',') is { } index ? 
           (int.Parse(s.ValueSpan[..index]) * int.Parse(s.ValueSpan[(index+1)..]))
           : 1);

    public object Solve2(string input) => input
    	.Split("do()")
    	.Select(s => s.IndexOf("don't()") is { } index && index != -1 ? s[..index] : s)
    	.Sum(s => Regex
          .Matches(s, @"(?<=mul\()[0-9]+,[0-9]+(?=\))")
          .Sum(s => s.Value.IndexOf(',') is { } index ? 
               (int.Parse(s.ValueSpan[..index]) * int.Parse(s.ValueSpan[(index+1)..]))
               : 1));
}
