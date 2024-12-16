using System.Linq;

namespace AdventOfCode2024.Solutions;

internal class Day16 : ISolution
{
    public int Day => 16;
    public object? Solve1(string input) =>
        0 is { } direction &&
        input.IndexOf('\n') + 1  is { } width &&
        input.IndexOf('S')  is { } start &&
        input.IndexOf('E')  is { } end &&
        new (int F, int A, int B)[]
        {
            (-width, 1, -1),
            (1, width, -width),
            (-1, width, -width),
            (-width, 1, -1),
        } is { } deltas &&
        new HashSet<int>() is { } visited &&
        new SortedDictionary<int, int>(input.Select((c, i) => (i, c)).Where(c => c.c == '.').ToDictionary(c => c.i, c => int.MaxValue))
         is { } dists ?
            Enumerable.Range(0, int.MaxValue)
            .Select(t => dists.TryGetValue(current + deltas[direction].F, out int v) ? dists[current + deltas[direction].F] = 1 : 0)
            .Select(t => dists.TryGetValue(current + deltas[direction].A, out int v) ? dists[current + deltas[direction].A] = 1000 : 0)
            .Select(t => dists.TryGetValue(current + deltas[direction].B, out int v) ? dists[current + deltas[direction].B] = 1000 : 0)
            .Select(t => visited.Add(current))
            //.Select(t => current =  dists.MinBy(kvp => kvp.Value).Key)
            .TakeWhile(t => 
			{
				var chararr = input.ToArray();
				dists.ToList().ForEach(t => chararr[t.Key] = t.Value == int.MaxValue ? 'I' : (char)('0' + t.Value % 10));
				Console.WriteLine(new string(chararr));
				Console.WriteLine();
				Console.ReadLine();
				return input[current] != 'E';
			})
			.Count()
        : throw null!;

    public object? Solve2(string input) => input;
}
