using System.Collections.Generic;
using System.Linq;

public static class RunLengthEncoding
{
    private static IEnumerable<char> YieldGroup(int counter, char? currentGroup)
    {
        if (counter > 1)
        {
            foreach (var digit in counter.ToString())
            {
                yield return digit;
            }
        }

        yield return (char)currentGroup;
    }

    private static IEnumerable<char> EncodeStreaming(IEnumerable<char> input)
    {
        char? currentGroup = null;
        var counter = 0;
        foreach (var letter in input)
        {
            var hasGroupEnded = currentGroup != letter;
            if (hasGroupEnded & (counter > 0))
            {
                foreach (var character in YieldGroup(counter, currentGroup))
                {
                    yield return character;
                }

                currentGroup = letter;
                counter = 1;
            }
            else
            {
                currentGroup = letter;
                counter++;
            }
        }

        foreach (var character in YieldGroup(counter, currentGroup))
        {
            yield return character;
        }
    }

    public static string Encode(string input) =>
        input == "" ? "" : new string(EncodeStreaming(input.ToCharArray()).ToArray());

    public static string Decode(string input) =>
        input == "" ? "" : new string(DecodeStreaming(input.ToCharArray()).ToArray());

    private static IEnumerable<char> DecodeStreaming(IEnumerable<char> input)
    {
        string? counterBuilder = null;
        var inputEnumerator = input.GetEnumerator();
        while (inputEnumerator.MoveNext())
        {
            if (char.IsDigit(inputEnumerator.Current))
            {
                counterBuilder += inputEnumerator.Current;
                continue;
            }

            var counter = int.Parse(counterBuilder ?? "1");
            for (var i = 0; i < counter; i++)
            {
                yield return inputEnumerator.Current;
            }

            counterBuilder = null;
        }
    }
}