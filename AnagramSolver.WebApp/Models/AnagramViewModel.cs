namespace AnagramSolver.WebApp.Models
{
    public class AnagramViewModel
    {
        //parcel of info that controller hands to view
        public string? Input { get; set; } // stores the user input

        public IReadOnlyCollection<string> Anagrams { get; set; } //enables foreach to go later through a sequence of strings
        = Array.Empty<string>(); //even before a search is happening Anagrams are not null. 

    }
}
