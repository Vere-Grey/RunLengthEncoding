using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class RunLengthEncoding
{
    private static string EncodeGroup(int counter, char? letter) => counter > 1 ? $"{counter}{letter}" : $"{letter}";

    private static IEnumerable<char> EncodeStreaming(IEnumerable<char> input)
    {
        char? currentGroup = null;
        var counter = 0;
        foreach (var letter in input)
        {
            var hasGroupEnded = currentGroup != letter;
            if (hasGroupEnded & (counter > 0))
            {
                if (counter > 1)
                {
                    foreach (var digit in counter.ToString())
                    {
                        yield return digit;
                    }
                }

                yield return (char)currentGroup;
                currentGroup = letter;
                counter = 1;
            }
            else
            {
                currentGroup = letter;
                counter++;
            }
        }

        if (counter > 1)
        {
            foreach (var digit in counter.ToString())
            {
                yield return digit;
            }
        }

        yield return (char)currentGroup;
    }

    public static string Encode(string input) =>
        input == "" ? "" : new string(EncodeStreaming(input.ToCharArray()).ToArray());

    // public static string EncodeMemoryHungry(string input)
    // {
    //     char? currentGroup = null;
    //     var counter = 0;
    //     var result = new StringBuilder();
    //
    //     foreach (var letter in input)
    //     {
    //         var doesLetterDiffer = currentGroup != letter;
    //         if (doesLetterDiffer & (counter > 0))
    //         {
    //             result.Append(EncodeGroup(counter, currentGroup));
    //             currentGroup = letter;
    //             counter = 1;
    //         }
    //         else
    //         {
    //             currentGroup = letter;
    //             counter++;
    //         }
    //     }
    //
    //     result.Append(EncodeGroup(counter, currentGroup));
    //     return result.ToString();
    // }

    private static int TakeNextCounter(string input, ref int index)
    {
        var digits = new string(input.TakeWhile(char.IsDigit).ToArray());
        index += digits.Length;
        return digits == "" ? 1 : int.Parse(digits);
    }

    public static string Decode(string input)
    {
        var result = new StringBuilder();

        for (var index = 0; index < input.Length; index++)
        {
            var counter = TakeNextCounter(input.Substring(index), ref index);
            var letter = input.Substring(index).First();
            result.Append(letter, counter);
        }

        return result.ToString();
    }
}