namespace AnagramSolver.EF.DatabaseFirst.Models;
public partial class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual List<Word> Words { get; set; } = new();
}
