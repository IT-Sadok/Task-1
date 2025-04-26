using ConsoleApp1.Models;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleApp1.Interfaces
{
    public interface ITextContentRepository
    {
        List<TextContent> GetAllContent();
        [return: MaybeNull]
        TextContent? GetContentByCode(string code);
        List<TextContent> FindContentByTitle(string title);
        List<TextContent> FindContentByAuthor(string author);
        void AddContent(TextContent content);
        bool DeleteContent(string code);
        bool StartSession(string code);
        bool EndSession(string code);
        void SaveChanges();
        void ResetAllData();
    }
} 