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
               .Select(s => int.Parse(s.ValueSpan)).ToArray();
        Span<int> outputbuffer = new int[64];
        Span<long> registers = new long[3];
        ReadOnlySpan<int> instructions = program.AsSpan(3);

        long current = 0;
        
        Span<int> output = default;
        int correctBits = 1;

        ReadOnlySpan<int> matchReq = instructions[^correctBits..];

        while (true)
        {
            registers[0] = current;
            registers[1] = registers[2] = 0;
            output = RunProgram(registers, instructions, outputbuffer);

            if (output.SequenceEqual(matchReq))
            {
                if (++correctBits <= instructions.Length)
                {
                    matchReq = instructions[^correctBits..];
                }
                else
                {
                    break;
                }

                current *= 8;
                current--;
            }

            current++;
        }
        
        return current;
    }

    private static string GenerateCommaDelimitedString(Span<int> output)
    {
        Span<char> finalString = stackalloc char[output.Length * 2];

        int fi = 0;
        for (int i = 0; i < output.Length; i++)
        {
            finalString[fi++] = (char)('0' + output[i]);
            finalString[fi++] = ',';
        }

        return new string(finalString[..^1]);
    }

    private static Span<int> RunProgram(Span<long> registers, ReadOnlySpan<int> program, Span<int> output)
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
                3 => regA == 0 ? 0 : pc = program[pc + 1] - 2,
                4 => regB ^= regC,
                5 => output[outputPtr++] = (int)(Combo(pc, ref program, ref registers) & 7),
                6 => regB = regA / (1 << (int)Combo(pc, ref program, ref registers)),
                7 => regC = regA / (1 << (int)Combo(pc, ref program, ref registers)),
                _ => Throw_UnknownOp()
            };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static long Combo(int pc, ref ReadOnlySpan<int> program, ref Span<long> reg) => program[pc + 1] is { } op && op < 4 ? op : reg[op - 4];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static long Throw_UnknownOp() => throw new Exception("Unknown operator");

        return output.Slice(0, outputPtr);
    }
}


