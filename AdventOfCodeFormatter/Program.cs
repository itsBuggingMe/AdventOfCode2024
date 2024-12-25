using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using TextCopy;

namespace AdventOfCodeFormatter;

internal class Program
{
    static void Main(string[] args)
    {
        Options options = ReadOptions(args);

        string text = File.ReadAllText(options.InputPath);
        int textArea = text.Length;

        string result;
        using var image = Image.Load<Rgba32>(File.OpenRead(options.ImagePath));
        {
            int imageArea = CountImageDarkPixels(image);

            float aspectRatio = 27f / 52f;
            float areaMultiplier = MathF.Sqrt(textArea / (float)(imageArea * aspectRatio));

            float constantMultiplier = 0.95f;
            bool usedPadding;

            do
            {
                Vector2 largeTextAreaSize = new Vector2(image.Width * areaMultiplier, image.Height * aspectRatio * areaMultiplier) * constantMultiplier;
                result = GenerateFormattedCode((int)largeTextAreaSize.X, (int)largeTextAreaSize.Y, text, image, out usedPadding);
                constantMultiplier += 0.001f;
            } while (!usedPadding);
        }

        Console.WriteLine(result);
        File.WriteAllText(options.OutputPath, result);
    }

    private static string GenerateFormattedCode(int width, int height, ReadOnlySpan<char> sourceCode, Image<Rgba32> image, out bool usedPadding)
    {
        StringBuilder sb = new StringBuilder(width * height);
        StringBuilder block = new StringBuilder();
        usedPadding = false;
        ReadOnlySpan<char> chunk = [];

        for(int i = 0; i < height; i++)
        {

            for(int j = 0; j < width;)
            {
                //move across a line
                int py = (int)(i / (double)height * image.Height);
                int px = (int)(j / (double)width * image.Width);

                if (IsConsideredDark(image[px, py]))
                {
                    if (chunk.Length == 0)
                    {
                        chunk = GetNextSmallestChunk(ref sourceCode, out var b);
                        usedPadding |= b;
                    }
                    else
                    {
                        if (chunk[0] == ' ')
                        {
                            sb.Append(' ');
                            j++;
                        }

                        sb.Append(chunk[0]);
                        chunk = chunk.Slice(1);
                        j++;
                    }
                }
                else
                {
                    if(chunk.Length == 0)
                    {
                        sb.Append(' ');
                        j++;
                    }
                    else
                    {
                        sb.Append(chunk[0]);
                        chunk = chunk.Slice(1);
                        sb.Remove(LastIndexOfDouble (sb, ' '), 1);
                    }
                }
            }

            sb.AppendLine();
        }

        return sb.ToString();

        static ReadOnlySpan<char> GetNextSmallestChunk(scoped ref ReadOnlySpan<char> source, out bool usedPadding)
        {
            if(source.Length == 0)
            {
                usedPadding = true;
                return "/**/";
            }

            int i;
            for(i = 1; i < source.Length; i++)
            {
                bool res = source[i] switch
                {
                    ',' => true,
                    '.' => i + 1 < source.Length && char.IsLetter(source[i + 1]),
                    ' ' => true,
                    '{' => true,
                    '}' => true,
                    '(' => true,
                    ')' => true,
                    '[' => true,
                    ']' => true,
                    _ => false,
                };

                if(res)
                    break;
            }

            var s = source[..i];
            source = source[i..];
            usedPadding = false;
            return s;
        }
    }

    private static int LastIndexOfDouble(StringBuilder sb, char c)
    {
        for(int i = sb.Length - 1; i > 0; i--)
        {
            if (sb[i] == c && sb[i - 1] == c)
            {
                return i;
            }
        }

        return -1;
    }

    private static int CountImageDarkPixels(Image<Rgba32> image)
    {
        int count = 0;

        image.ProcessPixelRows(acc =>
        {
            for(int i = 0; i < acc.Height; i++)
            {
                foreach (ref var p in acc.GetRowSpan(i))
                {
                    if(IsConsideredDark(p))
                        count++;
                }
            }
        });

        return count;
    }

    private static bool IsConsideredDark(Rgba32 p) => p.R + p.G + p.B < 382;

    internal record class Options(string OutputPath, string InputPath, string ImagePath);

    private static Options ReadOptions(string[] args)
    {
        string? outputPath = null;
        string? inputPath = null;
        string? imagePath = null;

        if (args.Length % 2 != 0)
            throw new Exception("Invalid arguments");

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-o":
                    outputPath = args[i + 1];
                    break;
                case "-i":
                    inputPath = args[i + 1];
                    break;
                case "-p":
                    imagePath = args[i + 1];
                    break;
                default:
                    throw new Exception("Unknown option");
            }
        }

        if (inputPath is null || imagePath is null)
        {
            foreach(var file in Directory.EnumerateFiles(Directory.GetCurrentDirectory()))
            {
                if(file.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                    imagePath = file;
                if (file.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase))
                    inputPath = file;
                if (file.EndsWith(".txt", StringComparison.InvariantCultureIgnoreCase))
                    inputPath = file;
            }

            if (inputPath is null || imagePath is null)
                throw new Exception("Must provide at least image and input path");
        }

        outputPath ??= inputPath;

        return new Options(outputPath, inputPath, imagePath);
    }
}
