using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TextContentManagement2
{
    public class Storage
    {
        private readonly string _filePath;

        public Storage(string filePath)
        {
            _filePath = filePath;
        }

        public List<TextContent> LoadTextContent()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<TextContent>();

                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<TextContent>>(json)
                ?? new List<TextContent>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Load Error: {ex.Message}");
                return new List<TextContent>();
            }
        }

        public void SaveTextContent(List<TextContent> contents)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(contents, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save Error: {ex.Message}");
            }
        }
    }
}