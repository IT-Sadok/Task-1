using ConsoleApp1.Interfaces;
using ConsoleApp1.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleApp1.Repositories
{
    public class JsonTextContentRepository : ITextContentRepository
    {
        private List<TextContent> _contentItems;
        private readonly string _filePath;

        public JsonTextContentRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty", nameof(filePath));

            _filePath = filePath;
            _contentItems = LoadContent();
        }

        private List<TextContent> LoadContent()
        {
            if (!File.Exists(_filePath))
            {
                // Create directory if it doesn't exist
                string? directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                    }
                    catch (Exception ex)
                    {
                        throw new IOException($"Unable to create directory for content storage: {ex.Message}", ex);
                    }
                }
                return new List<TextContent>();
            }

            try
            {
                string json = File.ReadAllText(_filePath);
                var content = JsonConvert.DeserializeObject<List<TextContent>>(json);
                return content ?? new List<TextContent>();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Error parsing content: {ex.Message}", ex);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Error reading content file: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error loading content: {ex.Message}", ex);
            }
        }

        public void SaveChanges()
        {
            try
            {
                string? directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonConvert.SerializeObject(_contentItems, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Error serializing content: {ex.Message}", ex);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Error writing to content file: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error saving content: {ex.Message}", ex);
            }
        }

        public List<TextContent> GetAllContent()
        {
            return _contentItems.ToList();
        }

        [return: MaybeNull]
        public TextContent? GetContentByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Content code cannot be empty", nameof(code));

            return _contentItems.FirstOrDefault(c => c.Code == code);
        }

        public List<TextContent> FindContentByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            return _contentItems.Where(c => c.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<TextContent> FindContentByAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be empty", nameof(author));

            return _contentItems.Where(c => c.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public void AddContent(TextContent content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            if (string.IsNullOrWhiteSpace(content.Title))
                throw new ArgumentException("Content title cannot be empty");

            if (string.IsNullOrWhiteSpace(content.Author))
                throw new ArgumentException("Content author cannot be empty");

            if (string.IsNullOrWhiteSpace(content.Code))
                throw new ArgumentException("Content code cannot be empty");

            if (_contentItems.Any(c => c.Code == content.Code))
            {
                throw new InvalidOperationException($"Content with code {content.Code} already exists");
            }
            _contentItems.Add(content);
            SaveChanges();
        }

        public bool DeleteContent(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Content code cannot be empty", nameof(code));

            TextContent? content = GetContentByCode(code);
            if (content == null)
            {
                return false;
            }

            _contentItems.Remove(content);
            SaveChanges();
            return true;
        }

        public bool StartSession(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Content code cannot be empty", nameof(code));

            TextContent? content = GetContentByCode(code);
            if (content == null || content.Status == TextStatus.InSession)
            {
                return false;
            }

            content.Status = TextStatus.InSession;
            SaveChanges();
            return true;
        }

        public bool EndSession(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Content code cannot be empty", nameof(code));

            TextContent? content = GetContentByCode(code);
            if (content == null || content.Status == TextStatus.Available)
            {
                return false;
            }

            content.Status = TextStatus.Available;
            SaveChanges();
            return true;
        }

        public void ResetAllData()
        {
            try
            {
                _contentItems.Clear();
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to reset content: {ex.Message}", ex);
            }
        }
    }
} 