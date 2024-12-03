namespace AdventOfCode2024;

internal interface ISolution
{
    int Day { get; }
    object? Solve1(string input) => null;
    object? Solve2(string input) => null;
}