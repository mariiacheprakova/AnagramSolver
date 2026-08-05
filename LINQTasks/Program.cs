class Program
{
    record Student(int Id, string Name, int GroupId);
    record Groupings(int Id, string Name);
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
        var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();

        //2.Rasti nelyginių skaičių sumą
        var oddNumbersSum = numbers.Where(n => n % 2 != 0).Sum();

        //3.Pirmus 10 skaičių dalijančių iš 3
        var divideByThreeNumbers = numbers.Where(n => n % 3 == 0).Take(10).ToList();

        //4.Sugrupuoti: maži(1–33), vidutiniai(34–66), dideli(67–100)
        var groups = numbers.GroupBy(n => n <= 33 ? "small" : n <= 66 ? "Medium" : "Large").ToList();

        //5.Sukurti Dictionary: raktas = skaičius, reikšmė = ar pirminis
        var primeDict = numbers.ToDictionary(n => n,
            n => n > 1 &&
            Enumerable.Range(2,(int)Math.Sqrt(n)-1)
            .All(d=> n%d != 0));

        var sentences = new List<string>
        { "LINQ yra galingas", "C# yra puiki kalba", "Generics ir Delegates" };

        //1.Išskleisti į atskirus žodžius(Split + SelectMany)
        var wordsSeparated = sentences.SelectMany(s => s.Split(' ')).ToList();

        //2.Rasti unikalius žodžius
        var uniqueWords = wordsSeparated.Distinct();

        //3.Suskaičiuoti kiek kartų kiekvienas žodis pasikartoja\
        var wordsRepeated = wordsSeparated.CountBy(w => w);

        // Task 3
        var students = new List<Student>
        {
            new(1, "Alice", 1),
            new(2, "Bob", 2),
            new(3, "Carol", 1),
            new(4, "David", 1),
            new(5, "Emma", 3),
            new(6, "Frank", 2)
        };

           var groupings = new List<Groupings>
            {
            new(1, "Backend"),
            new(2, "Frontend"),
            new(3, "Data Analysis"),
            new(4, "Cybersecurity")
            };

        //1.Sujungti studentus su grupėmis naudojant Join
        var results = students.Join
            (groupings,
            s => s.GroupId,
            g => g.Id,
            (s, g) =>  new
            {
                studentName = s.Name,
                groupName = g.Name
            })
            .ToList();

        //2.Išvesti "Studentas X yra grupėje Y"
        results.ForEach(r =>
            Console.WriteLine($"Student {r.studentName} is in group {r.groupName}"));

        //3.Rasti grupes su daugiau nei 2 studentais(GroupJoin)
        var groupsWithMoreThanTwoStudents = groupings
            .GroupJoin(
            students,
            g => g.Id,
            s => s.GroupId,
            (g, s) => new 
            { 
                Group = g,
                Students = s
            })
            .Where(g => g.Students.Count() > 2)
            .ToList();

        groupsWithMoreThanTwoStudents.ForEach(r =>
        {
            Console.WriteLine($"Group: {r.Group.Name}");

           foreach(var student in r.Students)
            {
                Console.WriteLine($" - {student.Name}");
            }
        });

        //Task 5
        //Some LINQ methods are deferred which means that the query is not executed when it;s constructed
        //and gets evaluated only while results are being iterated over (f.e Where,Select,OrderBy etc..)
        //but methods such ToList(), First(), ToDictionary(), aggregation methods produce outcomes immediately

        var numbs = new List<int> { 1, 2, 3, 4, 5 };
        var query = numbs.Where(n => n % 2 != 0); // here we utilise Where and it;s a deferred method
        numbs.AddRange(new[] {30,22,11}); // modyfying the source before execution
        foreach(var number in query)
        {
            Console.WriteLine(number); // query executes during enumeration
        }
        var numbsWithoutQuery = new List<int> { 1, 2, 3, 4, 5 };
        var q = numbsWithoutQuery.Where(n => n % 2 != 0).ToList(); // immediate assessment of query because of ToList() method
        numbsWithoutQuery.AddRange([20, 41, 7]); // alteration do not affect the immediate results
        foreach(var n in q)
        {
            Console.WriteLine(n);
        }
    }
}