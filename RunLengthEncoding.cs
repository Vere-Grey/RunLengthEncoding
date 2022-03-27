using System.Linq;
using System.Text;

public static class RunLengthEncoding
{
    private static string EncodeGroup(int counter, char? letter) => counter > 1 ? $"{counter}{letter}" : $"{letter}";

    public static string Encode(string input)
    {
        char? currentGroup = null;
        var counter = 0;
        var result = new StringBuilder();

        foreach (var letter in input)
        {
            if ((currentGroup != letter) & (counter > 0))
            {
                result.Append(EncodeGroup(counter, currentGroup));
                currentGroup = letter;
                counter = 1;
            }
            else
            {
                currentGroup = letter;
                counter++;
            }
        }

        result.Append(EncodeGroup(counter, currentGroup));
        return result.ToString();
    }

    private static int parseNextMultiplier(string input, ref int index)
    {
        var number = new string(input.TakeWhile(char.IsDigit).ToArray());
        index += number.Length;
        return number == "" ? 1 : int.Parse(number);
    }

    public static string Decode(string input)
    {
        var result = new StringBuilder();

        for (var index = 0; index < input.Length; index++)
        {
            var multiplier = parseNextMultiplier(input.Substring(index), ref index);
            var letter = input.Substring(index).First();
            result.Append(letter, multiplier);
        }

        return result.ToString();
    }
}