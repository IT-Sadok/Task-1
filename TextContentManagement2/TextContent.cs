namespace TextContentManagement2
{
    // Class creation - a defenition of its contents (parameters, methods, etc.) when referenced.
    public class TextContent
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string SerialNumber { get; set; }
        public int? YearValue { get; set; }    // null enabled BY DEFAULT.
        public bool Availability { get; set; }


        // Parameterless Constructor ENABLES PROPERTY-BASED DESERIALIZATION, which values are not null by default.
        public TextContent()
        {
            Title = "NO ENTRY";  // Exact name match for JSON Serializer, assigned value because not null by default.
            Author = "NO ENTRY";
            SerialNumber = "NO ENTRY";
            Availability = true; // Set "true" to initialize in the constructor.
        }


        // Parameterized Constructor for null control.
        public TextContent(string title, string author, string serialNumber, int? year)
        : this()                                                           // Calling the parameterless Constructor.
        {
            Title = string.IsNullOrWhiteSpace(title) ? "NO ENTRY" : title; // If ("??") null, then the message ().
            Author = string.IsNullOrWhiteSpace(author) ? "NO ENTRY" : author;
            SerialNumber = string.IsNullOrWhiteSpace(serialNumber) ? "NO ENTRY" : serialNumber;
            YearValue = year;                                              // Passes YearValue to YearDisplay.
        }


        // Year conversion for UI display.
        public string YearDisplay => YearValue switch // "switch" evaluates int returning string.
        {
            null => "NO ENTRY",                       // "=>" equals "if".
            < 0 => $"{Math.Abs(YearValue.Value)} BC", // "Abs" converts negative int.
            0 => "0 AD",
            _ => $"{YearValue}"                       // "${}" embeds expressions within strings.
        };
    }
}