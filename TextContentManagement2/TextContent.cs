namespace TextContentManagement2
{

    // Class creation - a defenition of its contents (parameters, methods, etc.) when referenced.
    public class TextContent
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string SerialNumber { get; set; }
        public int? YearValue { get; set; } // null enabled.
        public bool Availability { get; set; }


        // Constructor creation for null control.
        public TextContent(string title, string author, string serialNumber, int? year)
        {
            Title = title ?? "NO ENTRY"; // If ("??") null, then the message ().
            Author = author ?? "NO ENTRY";
            SerialNumber = serialNumber ?? "NO ENTRY";
            YearValue = year;           // Passes YearValue to YearDisplay.
            Availability = true;
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