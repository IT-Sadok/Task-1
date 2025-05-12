using System;
using System.Collections.Generic;
using System.Linq;
using TextContentManagement;


namespace TextContentManagement // No need for TextContentManagement.data because it is in the main (root) folder (.data is for files in folders).
{
    public class TextContentRepository
    {
        private readonly List<TextContent> _textContents = new();   // QUESTION ON WHY "Collection initialization can be simplified"

        // Methods for accessing and managing TextContent 

        public void AddTextContent(TextContent content) // Add TextContent
        {
            _textContents.Add(content);
        }

       
        public List<TextContent> GetAll()  // Get all TextContents  
        {
            return _textContents.ToList();
        }


        public List<TextContent> SearchByTitle(string keyword)
        {
            return _textContents
                .Where(c => c.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }


        // Delete by Title, no case-sensitive.
        public bool DeleteByTitle(string Title)
        {
            var content = _textContents.FirstOrDefault(c => c.SerialNumber == Title);
            if (content != null)
            {
                return _textContents.Remove(content);
            }
            else
            {
                // If no TextContent is found - returning false
                return false;
            }
        }
    }
}