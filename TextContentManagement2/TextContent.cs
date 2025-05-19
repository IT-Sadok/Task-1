namespace TextContentManagement2
{
    public class TextContent // Creating a "class" that is "public" - a defenition for a computer for what to work with that can be accesses by other classes.
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string SerialNumber { get; set; }
        public int Year { get; set; }

        public TextContent(string title, string author, string serialNumber, int year) // Creating a constructor for reventing null references to the parameters of the class. 
        {
            Title = title; // No verification of parameters for simplicity.
            Author = author;
            SerialNumber = serialNumber;
            Year = year;
        }
    }
}