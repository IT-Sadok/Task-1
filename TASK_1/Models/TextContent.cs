using System.Text.Json.Serialization;

namespace TASK_1.Models
{
    public class TextContent
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int YearOfPublication { get; set; }
        public string Code { get; set; }
        public TextStatus Status { get; set; }

        public TextContent(string title, string author, int yearOfPublication, string code)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));
            
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be empty", nameof(author));
            
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Content code cannot be empty", nameof(code));
            
            if (yearOfPublication <= 0)
                throw new ArgumentException("Year of publication to be positive", nameof(yearOfPublication));

            Title = title;
            Author = author;
            YearOfPublication = yearOfPublication;
            Code = code;
            Status = TextStatus.Available;
        }
    }

    public enum TextStatus
    {
        Available,
        InSession
    }
} 