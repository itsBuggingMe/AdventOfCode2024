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
        Array.Empty<int>().Aggregate((int i) => (uint)i < (uint)input.Length ? input[i] : '\n', (a, b) => null!)
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

    public object? Solve2(string input) => 
        input.ToArray() is char[] buffer &&
        input.IndexOf('\n') is int accWidth &&
        new int[] {
            -accWidth - 1,
            1,
            accWidth + 1,
            -1,
            0, input.IndexOf('^'), input.IndexOf('^')
        } is int[] dirs &&
        Array.Empty<int>().Aggregate((int i) => (uint)i < (uint)buffer.Length ? buffer[i] : '\n', (a, b) => null!)
        is Func<int, char> charAt ?
        buffer
            .Select((c, index) => (index, c))
            .Where(t => 
                    t.c == '.' && 
                    (dirs[4] = 0) == 0 && 
                    (dirs[5] = dirs[6]) != -1 && 
                    (buffer[t.index] = '#') == '#' &&
                    
                    
                 charAt(Enumerable.Range(0, int.MaxValue)
                     .Select(i =>
                             (gindex: dirs[5], garbage:
                              charAt(dirs[dirs[4] % 4] + dirs[5]) == '#' ?
                              dirs[4]++ :
                              (dirs[5] += dirs[dirs[4] % 4])))
                     .TakeWhile(i => {
                         var ret = charAt(i.gindex) != '\n';
                         if(i.gindex == t.index && dirs[4] == 0)
                         {

                         }
                         return ret;
                     })
                     .Last().gindex) != '\n' &&

                    (buffer[t.index] = '.') == '.'
            )
            .Count()
        : throw null!;
}
