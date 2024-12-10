namespace AdventOfCode2024.Solutions;

internal class Day6 : ISolution
{
    public int Day => 6;
    public object? Solve1(string input) =>
        input.IndexOf('\n') is int accWidth &&
        new int[] {
            -accWidth - 1,
            1,
            accWidth + 1,
            -1,
            0, input.IndexOf('^')
        } is int[] dirs &&
        Array.Empty<int>().Aggregate((int i) => unchecked((uint)i) < unchecked((uint)input.Length) ? input[i] : '\n', (a, b) => null!)
        is Func<int, char> charAt ?
        Enumerable.Range(0, int.MaxValue)
            .Select(i =>
                    (dirs[5],
                     charAt(dirs[dirs[4] % 4] + dirs[5]) == '#' ?
                     dirs[4]++ :
                     (dirs[5] += dirs[dirs[4] % 4])))
            .TakeWhile(i => charAt(i.Item1) != '\n')
            .Select(i => i.Item1)
            .ToHashSet()
            .Count
        : throw null!;

    public object Solve2(string input) =>
            input.ToArray() is char[] buffer &&
            input.IndexOf('\n') is int accWidth &&
            new int[] {
                -accWidth - 1,//0
                1,//1
                accWidth + 1,//2
                -1,//3
                0/*4*/, input.IndexOf('^')/*5*/, input.IndexOf('^')
            } is int[] dirs &&
            Array.Empty<int>().Aggregate((int i) => unchecked((uint)i) < unchecked((uint)buffer.Length) ? buffer[i] : '\n', (a, b) => null!)
            is Func<int, char> charAt ?
            buffer
                .Select((c, index) => (index, c))
                .Where(t => t.c == '.')
                .Where(t =>
                        (dirs[4] = 0) == 0 &&
                        (dirs[5] = dirs[6]) != -1 &&
                        (buffer[t.index] = '#') == '#' &&
                        new Dictionary<(int, int), int>() is 
                        Dictionary<(int, int), int> alreadyValues &&
                        Enumerable.Range(0, int.MaxValue)
                            .Select(i =>
                                 (ticks: i, gindex: dirs[5], garbage:
                                  charAt(dirs[dirs[4]] + dirs[5]) == '#' ?
                                  dirs[4] = (dirs[4] + 1) % 4 :
                                  (dirs[5] += dirs[dirs[4]])))
                            .Select(i => (isAlreadyHere: alreadyValues.ContainsKey((i.gindex, dirs[4])) && alreadyValues.Last().Key != (i.gindex, dirs[4]), isOffMap: charAt(i.gindex) == '\n', alreadyValues[(i.gindex, dirs[4])] = 0))
                            .First(t => t.isAlreadyHere || t.isOffMap)
                            .isAlreadyHere
                        &
                        (buffer[t.index] = '.') == '.'
                ).Count()
            : throw null!;
}
