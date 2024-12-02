using System.Text;

namespace AdventOfCode2024.Solutions;

internal class Day2 : ISolution
{
    public int Day => 2;

    public object Solve1(string input) => input
        .Split('\n')
        .Select(s => s.Split(' '))
        .Select(s => s.Select(int.Parse))
        .Select(s => s.Zip(s.Skip(1)).Select(t => t.First - t.Second))
        .Where(s => s.All(a => 
            Math.Sign(a) == Math.Sign(s.First()) && 
            a != 0 && Math.Abs(a) <= 3
        ))
        .Count();

    public object Solve2(string input) => input
        .Split('\n')
        .Select(s => s.Split(' '))
        .Select(s => s.Select(int.Parse))
        .Where(s => s.Skip(1).Aggregate((0, s.First()), (tuple, next) =>
        {
            bool isSafe = IsSafeDelta(s.First() - s.Skip(1).First(), tuple.Item2 - next);
            if(tuple.Item1 == 0)
            {
                return isSafe ? (0, next) : (1, next);
            }
            else if(tuple.Item1 == 1)
            {
                return isSafe ? (1, next) : (-1, next);
            }

            return (-1, 0);
        }).Item1 != -1)
        .Count();

    public static bool IsSafeDelta(int sign, int delta)
    {
        return Math.Sign(delta) == Math.Sign(sign) &&
            delta != 0 && Math.Abs(delta) <= 3;
    }
}
