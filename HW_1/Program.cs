using System.Text.RegularExpressions;

namespace HW_1;

public static partial class Program
{
    public static void Main()
    {
        Console.WriteLine(ReverseWord("привет"));
        Console.WriteLine(ReverseWords("Привет, как твои дела?"));
    }

    public static string ReverseWord(string word)
    {
        ArgumentNullException.ThrowIfNull(word);
        return string.Concat(word.Reverse());
    }

    public static string ReverseWords(string sentence)
    {
        ArgumentNullException.ThrowIfNull(sentence);
        return WordRegex().Replace(sentence, match => ReverseWord(match.Value));
    }

    [GeneratedRegex(@"\p{L}+")]
    private static partial Regex WordRegex();
}

