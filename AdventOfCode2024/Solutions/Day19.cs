using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;   

internal class Day19 : ISolution
{
    public int Day => 19;
    public object? Solve1(string input) => 
        input[..input.IndexOf("\n\n")].Split(',').Select(t => t.Trim()).ToArray() is { } towels &&
        (object)((object self, ReadOnlyMemory<char> match, string?[] b) =>
        {
            return b.Select((j, i) => (index: i, patt: j))
                .Where(c => c.patt is not null)
                .Any(f =>
                {
                    if(match.Length == 0)
                    {
                        return true;
                    }

                    if (match.Span.StartsWith(f.patt))
                    {
                        b[f.index] = null;
                        bool result = ((Func<object, ReadOnlyMemory<char>, string?[], bool>)self)(self, match.Slice(f.patt.Length), b);

                        if (result)
                        {
                            return true;
                        }
                        else
                        {
                            b[f.index] = f.patt;
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                });
        })
        is Func<object, ReadOnlyMemory<char>, string?[], bool> pattern ?
            input[input.IndexOf("\n\n")..]
                .Split('\n')
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .Count(t => pattern(pattern, t.AsMemory(), towels).WriteSelf())
        : throw null!;
    public object? Solve2(string input) => null;
}


