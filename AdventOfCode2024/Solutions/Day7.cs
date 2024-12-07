namespace AdventOfCode2024.Solutions;

internal class Day7 : ISolution
{
    public int Day => 7;
    public object? Solve1(string input) =>
        input.Split('\n')
        .Select(s => (head: long.Parse(s.AsSpan(0, s.IndexOf(':'))), tail: s[(s.IndexOf(':')+2)..].Split(' ').Select(long.Parse).ToArray()))
        .Where(t => Enumerable
            .Range(0, 1 << (t.tail.Length - 1))
            .Any(b => t.tail
                .Skip(1)
                .Aggregate((bits: b, val: t.tail[0]) , 
                    (num, acc) => (num.bits >> 1, (num.bits & 1) == 0 ? acc * num.val : acc + num.val)
                    ).val == t.head)
        )
        .Sum(t => t.head);

    public object Solve2(string input) =>
        input.Split('\n')
        .Select(s => (head: long.Parse(s.AsSpan(0, s.IndexOf(':'))), tail: s[(s.IndexOf(':')+2)..].Split(' ').Select(long.Parse).ToArray()))
        .Where(t => Enumerable
            .Range(0,  Enumerable.Range(0, t.tail.Length - 1)
                .Aggregate(1, (n, a) => n * 3))
            .Any(b => t.tail
                .Skip(1)
                .Aggregate((bits: b, val: t.tail[0]),
                    (num, acc) => (bits: num.bits / 3, val: 
                        num.bits % 3 == 0 ? acc * num.val 
                            : num.bits % 3 == 1 ? acc + num.val 
                                : long.Parse($"{num.val}{acc}")))
                    .val == t.head))
        .Sum(t => t.head);

        
}
