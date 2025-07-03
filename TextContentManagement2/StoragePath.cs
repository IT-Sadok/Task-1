using System;
using System.IO;

namespace TextContentManagement2
{
    public class StoragePath
    {
        public string? ChosenStoragePath { get; private set; } // null enabled.

        public void StorageOption()
        {
            while (true) // run until user exits.
            {
                Console.WriteLine("Choose Text Content storage:");
                Console.WriteLine("1 - New Storage;");
                Console.WriteLine("2 - Load Storage (from JSON file);");
                Console.WriteLine("3 - Use Default Storage;");
                Console.WriteLine("4 - Delete Storage.");
                Console.WriteLine("5 - EXIT");
                Console.WriteLine("on choosing # press ENTER to proceed: ");

                switch (Console.ReadLine()?.Trim())             // null check on imput.
                {
                    case "1":
                        NewStorage();
                        if (ChosenStoragePath != null) return; // Exit after successful setup.
                        break;
                    case "2":
                        LoadStorage();
                        if (ChosenStoragePath != null) return;
                        break;
                    case "3":
                        DefaultStorage();
                        if (ChosenStoragePath != null) return;
                        break;
                    case "4":
                        DeleteStorage();                      // Stays in the loop.
                        break;
                    case "5":
                    Console.WriteLine("Exiting ...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Choose from 1 - 5 to proceed");
                        break;
                }
            }
        }

        private void NewStorage()
        {
            while (true)
            {
                Console.WriteLine("Enter full path for a new JSON file: ");
                Console.WriteLine(" or ");
                Console.WriteLine("Enter 5 to return to the previous menu: ");
                string? input = Console.ReadLine()?.Trim();

                if (string.Equals(input, "5"))
                    return;

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Enter valid path. ");
                    continue;
                }

                try
                {
                    string? directory = Path.GetDirectoryName(input);

                    if (string.IsNullOrEmpty(directory))
                    {
                        Console.WriteLine("Invalid path.");
                        continue;
                    }

                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    // initializing JSON file.
                    File.WriteAllText(input, "[]");
                    ChosenStoragePath = input;
                    Console.WriteLine($"Created new storage at: {ChosenStoragePath}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private void LoadStorage()
        {
            while (true)
            {
                Console.Write("Enter storage path (JSON file location): ");
                Console.WriteLine(" or ");
                Console.WriteLine("Enter 5 to return to the previous menu: ");
                string? input = Console.ReadLine()?.Trim();

                if (string.Equals(input, "5"))
                    return;

                if (string.IsNullOrWhiteSpace(input) || !File.Exists(input))
                {
                    Console.WriteLine("Enter valid path:");
                    continue;
                }

                try
                {
                    // Checking JSON format.
                    string JSONcheck = File.ReadAllText(input);
                    if (!JSONcheck.Trim().StartsWith('[') || !JSONcheck.Trim().EndsWith(']'))
                    {
                        Console.WriteLine("JSON files only.");
                        continue;
                    }
                    ChosenStoragePath = input;
                    Console.WriteLine($"Loading from: {ChosenStoragePath}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }
        }

        private void DefaultStorage()
        {
            ChosenStoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TextContent.json");

            // Initializing the file if it doesn't exist.
            if (!File.Exists(ChosenStoragePath))
            {
                File.WriteAllText(ChosenStoragePath, "[]");
            }
            Console.WriteLine($"Using default storage: {ChosenStoragePath}");
        }

        private void DeleteStorage()
        {
            while (true)
            {
                Console.WriteLine("Enter full path of JSON's file to delete storage:");
                Console.WriteLine("or");
                Console.WriteLine("Enter 5 to return to previous menu:");
                string? input = Console.ReadLine()?.Trim();

                if (string.Equals(input, "5"))
                    return;

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Enter valid path.");
                    continue;
                }
                try
                {
                    if (!File.Exists(input))
                    {
                        Console.WriteLine("File not found.");
                        continue;
                    }

                    File.Delete(input);
                    Console.WriteLine($"Storage file Deleted: {input}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Deletion failed: {ex.Message}");
                } 

            }
        }
    }
}