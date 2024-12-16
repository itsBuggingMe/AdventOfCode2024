using System.Diagnostics;
using System.Reflection.Metadata;

namespace AdventOfCode2024.Solutions;

internal class Day15 : ISolution
{
    public int Day => 15;
    public object? Solve1(string input) => input.Split("\n\n") is { } arr &&
        arr[0].Split('\n').Select(s => s.Replace('@', '.').ToArray()).ToArray() is char[][] buffer &&
        input.IndexOf('\n') is int width &&
        arr[1].Replace("\n", "") is string instruction &&
        (input.IndexOf('@') % (width + 1)) is int px &&
        (input.IndexOf('@') / (width + 1)) is int py &&
        (object)((int dx, int dy, int max) =>
            Enumerable.Range(1, max - 1)
                .Select(i => (x: px + dx * i, y: py + dy * i))
                .Select(t => (t.x, t.y, buffer[t.y][t.x]))
                .First(t => t.Item3 != 'O') is { } last && last.Item3 == '#' ? false : ((buffer[py + dy][px + dx], buffer[last.y][last.x]) = (buffer[last.y][last.x], buffer[py + dy][px + dx])) is { }) is
        Func<int, int, int, bool> step &&
        instruction.Select(c => c switch
        {
            '<' => step(-1, 0, px + 1) ? px-- : px,
            '>' => step(1, 0, width - px) ? px++ : px,
            '^' => step(0, -1, py + 1) ? py-- : px,
            'v' => step(0, 1, width - py) ? py++ : px,
            _ => throw new Exception($"Rogue {c} character in instructions!")
        }).Count() != -1 ?
            buffer.SelectMany((i, j) => i.Select((c, k) => (c, x: k, y: j))).Where(c => c.c == 'O').Sum(t => t.x + t.y * 100)
        : throw null!;

    public object? Solve2(string input) => (input = input!.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.")) is { } && input.Split("\n\n") is { } arr &&
        arr[0].Split('\n').Select(s => s.Replace('@', '.').ToArray()).ToArray() is char[][] buffer &&
        input.IndexOf('\n') is int width &&
        arr[1].Replace("\n", "") is string instruction &&
        (input.IndexOf('@') % (width + 1)) is int px &&
        (input.IndexOf('@') / (width + 1)) is int py &&
        (object)((object s, int bx, int by, int dx, int dy, HashSet<(int, int)> prev) => s is Func<object, int, int, int, int, HashSet<(int, int)>, bool> self ?
            buffer[by][bx] switch
            {
                '[' => (!prev.Add((bx, by)) & !prev.Add((bx + 1, by))) ? false : self(s, bx + dx, by + dy, dx, dy, prev) || self(s, bx + dx + 1, by + dy, dx, dy, prev),
                ']' => (!prev.Add((bx, by)) & !prev.Add((bx - 1, by))) ? false : self(s, bx + dx, by + dy, dx, dy, prev) || self(s, bx + dx - 1, by + dy, dx, dy, prev),
                '#' => true,
                '.' => false,
                _ => throw new Exception($"Rogue {buffer[bx][by]} character in warehouse!")
            }
            : throw null!) is
        Func<object, int, int, int, int, HashSet<(int, int)>, bool> isBlocked &&
        instruction.Select(c => new HashSet<(int x, int y)>() is HashSet<(int x, int y)> set &&
                0 is int dx &&
                0 is int dy ? (c switch
                {
                    '<' => isBlocked(isBlocked, px - 1, py, dx = -1, 0, set = new()) ? (0, false) : (px--, true),
                    '>' => isBlocked(isBlocked, px + 1, py, dx = 1, 0, set = new()) ? (0, false) : (px++, true),
                    '^' => isBlocked(isBlocked, px, py - 1, 0, dy = -1, set = new()) ? (0, false) : (py--, true),
                    'v' => isBlocked(isBlocked, px, py + 1, 0, dy = 1, set = new()) ? (0, false) : (py++, true),
                    _ => throw new Exception($"Rogue {c} character in instructions!")
                }).Item2 ?
            set.OrderBy(box => (-dy * box.y) * 100 + -dx * box.x).Select(t => (buffer[t.y + dy][t.x + dx], buffer[t.y][t.x]) = (buffer[t.y][t.x], buffer[t.y + dy][t.x + dx])).Count() :
            0 : throw null!).Count() != -1 ?
            buffer.SelectMany((i, j) => i.Select((c, k) => (c, x: k, y: j))).Where(c => c.c == '[').Sum(t => t.x + t.y * 100)
        : throw null!;
}