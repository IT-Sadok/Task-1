namespace TextContentManagement2
{
    public class TextContent(string title, string author, string serialNumber, string year) // CHANGE #5: primary constructor used (simplified).
 {
     public string Title { get; } = title; // upper case (Title) for public properties, methods, and class names. Lower case (title) for private equivalent, and local variables (for clarity and convention). Here - a constructor's parameter.
     public string Author { get; } = author;
     public string SerialNumber { get; } = serialNumber;
     public string Year { get; } = year; // CHANGE #3 (from int to string) to simplify null validation.
 }
}
