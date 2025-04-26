using ConsoleApp1.Interfaces;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class ContentService
    {
        private readonly ITextContentRepository _contentRepository;

        public ContentService(ITextContentRepository contentRepository)
        {
            _contentRepository = contentRepository ?? throw new ArgumentNullException(nameof(contentRepository));
        }

        public List<TextContent> GetAllContent()
        {
            try
            {
                return _contentRepository.GetAllContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve content", ex);
            }
        }

        public bool AddContent(string title, string author, int yearOfPublication, string code)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));
            
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be empty", nameof(author));
            
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Content code cannot be empty", nameof(code));
            
            if (yearOfPublication <= 0)
                throw new ArgumentException("Year of publication to be positive", nameof(yearOfPublication));

            try
            {
                var content = new TextContent(title, author, yearOfPublication, code);
                _contentRepository.AddContent(content);
                return true;
            }
            catch (InvalidOperationException)
            {
                // This is an expected exception when content with the same code already exists
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add content: {ex.Message}", ex);
            }
        }

        public bool DeleteContent(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Content code cannot be empty", nameof(code));

            try
            {
                return _contentRepository.DeleteContent(code);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete content: {ex.Message}", ex);
            }
        }

        public List<TextContent> SearchByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Search title cannot be empty", nameof(title));

            try
            {
                return _contentRepository.FindContentByTitle(title);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to search content by title: {ex.Message}", ex);
            }
        }

        public List<TextContent> SearchByAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Search author cannot be empty", nameof(author));

            try
            {
                return _contentRepository.FindContentByAuthor(author);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to search content by author: {ex.Message}", ex);
            }
        }

        public bool StartSession(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Content code cannot be empty", nameof(code));

            try
            {
                return _contentRepository.StartSession(code);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to start session: {ex.Message}", ex);
            }
        }

        public bool EndSession(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Content code cannot be empty", nameof(code));

            try
            {
                return _contentRepository.EndSession(code);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to end session: {ex.Message}", ex);
            }
        }

        public bool ResetAllData()
        {
            try
            {
                _contentRepository.ResetAllData();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to reset content data: {ex.Message}", ex);
            }
        }
    }
} 