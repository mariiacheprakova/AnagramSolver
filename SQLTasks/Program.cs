using System;

class Program
{
    public static void Main(string[] args)
    {
        var words = new List<string>
{ "alus", "sula", "la", "vanduo", "programavimas",
"katinas", "saulė", "medis", "oras", "knyga" };

        //1.Rasti žodžius ilgesnius nei 4 simboliai
        var longerThanFourChars = words.Where(w => w.Length > 4).ToList();

        //2.Rasti žodžius, kurie prasideda raide "s"
        var startWithS = words.Where(w => w.StartsWith('s')).ToList();

        //3.Surikiuoti pagal ilgį
        var orderedByLength = words.OrderBy(w => w.Length).ToList();

        //4.Paversti visus į didžiąsias raides
        var upperWords = words.Select(w => w.ToUpper()).ToList();

        //5.Sugrupuoti pagal pirmą raidę
        var groupedByFirstChar = words.GroupBy(w => w[0]).ToList();

        //6.Rasti ilgiausią žodį
        var longestWord = words.MaxBy(w => w.Length);

        //7.Patikrinti ar visi žodžiai turi bent 1 simbolį
        bool allWordsHaveAtLeastOneCharacter = words.All(w => w.Length >= 1);


        // Task 2
        var numbers = Enumerable.Range(1, 100).ToList();

        //1.Rasti visus lyginius skaičius
        var neatNumbers = numbers.Where(n => n % 2 == 0).ToList();

        //2.Rasti nelyginių skaičių sumą
        var oddNumbersSum = numbers.Where(n => n % 2 != 0).Sum();

        //3.Pirmus 10 skaičių dalijančių iš 3
        var divideByThreeNumbers = numbers.Where(n => n % 3 == 0).Take(10).ToList();

        //4.Sugrupuoti: maži(1–33), vidutiniai(34–66), dideli(67–100)
        var groups = numbers.GroupBy(n => n <= 33 ? "small" : n <= 66 ? "Medium" : "Large").ToList();

        //5.Sukurti Dictionary: raktas = skaičius, reikšmė = ar pirminis
        var dictionary = numbers.ToDictionary(n => n, n => n != 0);

        var sentences = new List<string>
{ "LINQ yra galingas", "C# yra puiki kalba", "Generics ir Delegates" };

        //1.Išskleisti į atskirus žodžius(Split + SelectMany)
        var wordsSeparated = sentences.SelectMany(s => s.Split(' ')).ToList();

        //2.Rasti unikalius žodžius
        var uniqueWords = wordsSeparated.Distinct();

        //3.Suskaičiuoti kiek kartų kiekvienas žodis pasikartoja\
        var wordsRepeated = wordsSeparated.CountBy(w => w);




    }
}