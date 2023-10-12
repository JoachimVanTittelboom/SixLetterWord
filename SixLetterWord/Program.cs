// See https://aka.ms/new-console-template for more information
using System.Reflection;
using System.Runtime.CompilerServices;

Console.WriteLine("Hello, World!");
string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
string path = Path.Combine(currentDirectory, "input.txt");
List<string> lines = ReadTextFile(path);

Dictionary<string, List<string>> sixCharacterWords = GetSixCharacterWords(lines);
List<string> partialWords = GetNotSixCharacterWords(lines);

Dictionary<string, List<string>> wordWithPartialwords = GetWordsAndPartialCharacters(sixCharacterWords, partialWords);

Console.WriteLine("TWO WORDS COMBINATIONS");
foreach (KeyValuePair<string, List<string>> word in wordWithPartialwords)
{
    List<string> combinations = FindTwoWordsCombinations(word.Key, word.Value);
    foreach (string combination in combinations)
    {
        Console.WriteLine($"{word.Key} = {combination}");
    }
}

Console.WriteLine();
Console.WriteLine("ALL COMBINATIONS");
foreach (KeyValuePair<string, List<string>> item in wordWithPartialwords)
{
    List<string> combinations = FindCombinations(item.Key, item.Value);

    foreach (string combination in combinations)
    {
        Console.WriteLine($"{item.Key} = {combination}");
    }
}

static List<string> ReadTextFile(string path)
{
    List<string> lines = new();
    using var sr = new StreamReader(path);
    string? line;
    while ((line = sr.ReadLine()) != null)
    {
        lines.Add(line);
    }
    return lines;
}

static Dictionary<string, List<string>> GetSixCharacterWords(List<string> words)
{
    List<string> foundedWords = words.Where(x => x.Length == 6).ToList();
    Dictionary<string, List<string>> dictionary = new();
    foreach (string item in foundedWords)
    {
        if (dictionary.ContainsKey(item) is false)
        {
            dictionary.Add(item, new List<string>());
        }
    }

    return dictionary;
}

static List<string> GetNotSixCharacterWords(List<string> words)
{
    return words.Where(x => x.Length < 6).ToList();
}

static Dictionary<string, List<string>> GetWordsAndPartialCharacters(Dictionary<string, List<string>> foundedWords, List<string> partialWords)
{
    foreach (var word in partialWords)
    {
        var keys = foundedWords.Keys.Where(x => x.Contains(word)).ToList();
        foreach (var item in keys)
        {
            if (foundedWords[item].Contains(word) is false)
            {
                foundedWords[item].Add(word);
            }
        }

    }

    return foundedWords;
}

static List<string> FindTwoWordsCombinations(string word, List<string> partialWords)
{
    List<string> combinations = new List<string>();
    foreach (string partial in partialWords)
    {
        if (word.StartsWith(partial))
        {
            string restWord = word.Substring(partial.Length);
            if (partialWords.Contains(restWord))
            {
                combinations.Add($"{partial} + {restWord}");
            }
        }
    }
    return combinations;
}

static List<string> FindCombinations(string word, List<string> partialWords)
{
    List<string> combinations = new();
    FindCombinationsRecursive(word, partialWords, new List<string>(), combinations);
    return combinations;
}

static void FindCombinationsRecursive(string remaining, List<string> partialWords, List<string> current, List<string> combinations)
{
    if (string.IsNullOrEmpty(remaining))
    {
        combinations.Add(string.Join('+', current));
        return;
    }

    foreach (string partial in partialWords)
    {
        if (remaining.StartsWith(partial))
        {
            string newRemaining = remaining.Substring(partial.Length);
            current.Add(partial);
            FindCombinationsRecursive(newRemaining, partialWords, current, combinations);
            current.RemoveAt(current.Count - 1);
        }
    }
}