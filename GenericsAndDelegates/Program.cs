class Program
{
    public static void Main(string[] args)
    {
        // Task 1
        var numberOne = 10;
        var numberTwo = 25;

        Swap(ref numberOne, ref numberTwo);

        var wordOne = "first";
        var wordTwo = "second";

        Swap(ref wordOne, ref wordTwo);

        var objectOne = new Custom<char>();
        objectOne.Set('A');
        var objectTwo = new Custom<char>();
        objectTwo.Set('B');

        Swap(ref objectOne, ref objectTwo);
        Console.WriteLine($"{numberOne}, {numberTwo}");
        Console.WriteLine($"{wordOne}, {wordTwo}");
        Console.WriteLine($"{objectOne.Get()}, {objectTwo.Get()}");

        //Task 2
        var longStrings = FilterVariables(["per m", "maz", "daugggg", "per daug"], x => x.Length > 4);
        var evenNumbers = FilterVariables([3, 4, 66, 3, 55, 20], x => x % 2 == 0);
        Console.WriteLine(string.Join(", ", longStrings));
        Console.WriteLine(string.Join(", ", evenNumbers));


        //Task 3



        var operations = new Dictionary<string, Func<string, string>>
        {
            {"upper", s => s.ToUpper() },
            {"lower", s => s.ToLower() },
            {"reverse", ReverseString}
        };

        Console.WriteLine(operations["upper"]("mAiau"));
        Console.WriteLine(operations["lower"]("MIAu"));
        Console.WriteLine(operations["reverse"]("Miau"));


    }

    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    public class Custom<T>
    {
        private T? _value;
        public void Set(T value) => _value = value;

        public T? Get() => _value;

    }
    public static List<T> FilterVariables<T>(IEnumerable<T> source, Predicate<T> condition)
    {
        var result = new List<T>();
        foreach (var item in source)
        {
            if (condition(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public static string ReverseString(string s) => new string(s.ToCharArray().Reverse().ToArray());

}