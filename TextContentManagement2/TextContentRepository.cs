using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;        // need to install Newtonsoft.Json package by clicking on Solution 'TextContentManagement2' under Search Solution Explorer, then choose Manage NuGet Packages, search Newton.Json (JavaScript Object Notation),and install.
using TextContentManagement2; // using class from TextContent.cs without the need for "qualifying" (or more typing).

namespace TextContentManagement2 
{
    public class TextContentRepository
    {
        private readonly string _filePath = "TextContent.json"; // A path to where TextContent.json will be stored. Can do manually by private string _filePath = "path/to/your/file.json".
        private List<TextContent> _contents; // a "List" of <TextContent" that is contacted my Add, Delete, and Search methods.
                                            

        public TextContentRepository() // Creating a constructor like public TextContent.
        {
            if (File.Exists(_filePath)) // reading TextContentRepository() if it exists. 
                _contents = JsonConvert.DeserializeObject<List<TextContent>>(File.ReadAllText(_filePath)); // converts (from .json to readable C# format) the JSON string into a List<TextContent>.
            else
                _contents = new List<TextContent>(); // iff the file does not exist, a new empty List<TextContent> is created and assigned to the _contents field.
        }

        public void Add(TextContent content) // Defining (how it looks like) "Method", or action, for adding Text Content. Takes values from TextContent class in namespace TextContentManagement2.
                                             // using "void" because Add() primary purpose is to modify the _contents collection and save the changes.
        {
            _contents.Add(content); // Method is defined.
            Save(); // Refering to another method
        }

        public List<TextContent> Search(string keyword) => // => is an Expression-Bodied Method, used for finding a value (Title, Author, Serial Number) Using Lambdas for shorter code. For example: "public int Add(int a, int b) => a + b;".
            _contents.FindAll(c =>                         // Alternatively instead of => write: return _contents.FindAll(...); 
                c.Title.Contains(keyword) ||               // "c" is a single TextContent object from "_contents" list in a Lambda expression;
                c.Author.Contains(keyword) ||              // .Contains() is key sensitive. Chosen for simplicity. 
                c.SerialNumber.Contains(keyword));

          /* For each TextContent item (c) in _contents:
             if (c.Title contains keyword) OR
             (c.Author contains keyword) OR
             (c.SerialNumber contains keyword)
             then include it in the results */

                                                      /* ALTERNITIVELY (longer code):
                                                       
                                                             public List<TextContent> Search(string keyword)
                                                             {
                                                                 return _contents.FindAll(MatchesKeyword);

                                                                 bool MatchesKeyword(TextContent c)
                                                                 {
                                                                      return c.Title.Contains(keyword) || 
                                                                             c.Author.Contains(keyword) || 
                                                                             c.SerialNumber.Contains(keyword);
                                                                 }
                                                             }                                          
                                                     */

public void Delete(string identifier) // Method for deleting, initializing "identifier".
{
   _contents.RemoveAll(c => c.Title == identifier || c.SerialNumber == identifier);  // "c" - a parameter in the Lambda collection for every item. "==" checks if .Title or .SerialNumber match. 
   Save();                                                                           // "||" is or in Lambda. 
}

private void Save() =>                                                   // "=>" is separating method's signature (its definition from its body) to separate Creating .json file where Text Content is stored.
   File.WriteAllText(_filePath, JsonConvert.SerializeObject(_contents)); // Convert TextContent object to JSON (string).
}
}