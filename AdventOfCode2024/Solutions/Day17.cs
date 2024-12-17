using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

internal class Day17 : ISolution
{
    public int Day => 17;
    public object? Solve1(string input) => Regex.Matches(input, @"\d+")
               .Select(s => long.Parse(s.ValueSpan)).ToArray() is { } p &&
                p[..3] is { } reg &&
                p[3..] is { } program &&
                (object)((long i) => program[i + 1] is { } op && op < 4 ? op : reg[op - 4]) is
                Func<long, long> combo &&
                0L is { } pc ?
                    string.Join(',', Enumerable.Range(0, int.MaxValue)
                    .Select(t => pc < program.Length ? program[pc] switch
                    {
                        0 => (reg[0] = reg[0] / (1 << (int)combo(pc)), 0),
                        1 => (reg[1] ^= program[pc + 1], 0),
                        2 => (reg[1] = combo(pc) & 7, 0),
                        3 => (reg[0] == 0 ? 0 : pc = program[pc + 1] - 2, 0),
                        4 => (reg[1] ^= reg[2], 0),
                        5 => (combo(pc) & 7, 1),
                        6 => (reg[1] = reg[0] / (1 << (int)combo(pc)), 0),
                        7 => (reg[2] = reg[0] / (1 << (int)combo(pc)), 0),
                        _ => throw new Exception()
                    } : (0, -1))
                    .Select(t => (pc += 2) is { } ? t : throw null!)
                    .TakeWhile(t => t.Item2 != -1)
                    .Where(t => t.Item2 == 1)
                    .Select(t => t.Item1))
                : throw null!;

    public object? Solve2(string input)
    {
        var program = Regex.Matches(input, @"\d+")
               .Select(s => long.Parse(s.ValueSpan)).ToArray();

        Span<int> output = stackalloc int[64];
        output.Fill(-1);

        RunProgram(program[0..3], program[3..], output);

        int len = output.IndexOf(-1);
        output = output[..len];
        Span<char> finalString = stackalloc char[len * 2];

        int fi = 0;
        for (int i = 0; i < output.Length; i++)
        {
            finalString[fi++] = (char)('0' + output[i]);
            finalString[fi++] = ',';
        }

        return new string(finalString[..^1]);
    }


    private static void RunProgram(Span<long> registers, ReadOnlySpan<long> program, Span<int> output)
    {
        ref long regA = ref registers[0];
        ref long regB = ref registers[1];
        ref long regC = ref registers[2];

        int outputPtr = 0;

        for (int pc = 0; pc < program.Length; pc += 2)
        {
            _ = program[pc] switch
            {
                0 => regA /= (1 << (int)Combo(pc, ref program, ref registers)),
                1 => regB ^= program[pc + 1],
                2 => regB = Combo(pc, ref program, ref registers) & 7,
                3 => regA == 0 ? 0 : pc = (int)program[pc + 1] - 2,
                4 => regB ^= regC,
                5 => output[outputPtr++] = (int)(Combo(pc, ref program, ref registers) & 7),
                6 => regB = regA / (1 << (int)Combo(pc, ref program, ref registers)),
                7 => regC = regA / (1 << (int)Combo(pc, ref program, ref registers)),
                _ => Throw_UnknownOp()
            };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static long Combo(int pc, ref ReadOnlySpan<long> program, ref Span<long> reg) => program[pc + 1] is { } op && op < 4 ? op : reg[(int)op - 4];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static long Throw_UnknownOp() => throw new Exception("Unknown operator");
    }
}

/*
 *                     .Select(t =>
                {
                    pc += 2;
                    Console.WriteLine($"Program State:\nA:{reg[0]}\nB:{reg[1]}\nC:{reg[2]}\n{new string(' ', 2 * pc)}v\n{input[(input.IndexOf(',') - 1)..]}");
                    Console.WriteLine();
                    return t;
                })
 * */


