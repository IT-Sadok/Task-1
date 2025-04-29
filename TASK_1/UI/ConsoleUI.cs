using TASK_1.Models;
using TASK_1.Services;

namespace TASK_1.UI
{
    public class ConsoleUI
    {
        private readonly ContentService _contentService;

        public ConsoleUI(ContentService contentService)
        {
            _contentService = contentService;
        }

        // Custom input method that returns null if Backspace is pressed
        private string? ReadInputWithBackspace(string prompt)
        {
            Console.Write(prompt);
            string input = "";
            
            while (true)
            {
                var keyInfo = Console.ReadKey(true); // true means don't echo the character
                
                if (keyInfo.Key == ConsoleKey.Backspace && input.Length == 0)
                {
                    // Return null if Backspace is pressed with empty input - signal to go back
                    Console.WriteLine();
                    Console.WriteLine("\nReturning to main menu...");
                    System.Threading.Thread.Sleep(200); // Pause briefly
                    return null;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    // Complete input on Enter
                    Console.WriteLine(); // Move to next line
                    return input;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    // Handle backspace for editing
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    // Add regular characters to input
                    input += keyInfo.KeyChar;
                    Console.Write(keyInfo.KeyChar);
                }
            }
        }

        // Reads an integer with Backspace support
        private int? ReadIntWithBackspace(string prompt)
        {
            string? input = ReadInputWithBackspace(prompt);
            if (input == null) return null; // User pressed Backspace to go back
            
            if (int.TryParse(input, out int result))
                return result;
                
            return -1; // Invalid input
        }

        public void Start()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Text Content Management System ===");
                Console.WriteLine();
                Console.WriteLine("1. Add Text Content");
                Console.WriteLine("2. Delete Text Content");
                Console.WriteLine();
                Console.WriteLine("3. Search Text Content");
                Console.WriteLine("4. Display all Text Content");
                Console.WriteLine();
                Console.WriteLine("5. Access Text Content");
                Console.WriteLine("6. Return Text Content");
                Console.WriteLine();
                Console.WriteLine("7. Reset All");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("0. Exit");
                Console.WriteLine();
                Console.WriteLine("=================");
                Console.Write("\nENTER SELECTION #: ");

                string optionInput = "";
                var keyInfo = Console.ReadKey(true);
                
                if (char.IsDigit(keyInfo.KeyChar))
                {
                    Console.WriteLine(keyInfo.KeyChar);
                    optionInput = keyInfo.KeyChar.ToString();
                    
                    if (int.TryParse(optionInput, out int choice))
                    {
                        Console.Clear();
                        switch (choice)
                        {
                            case 1:
                                AddContent();
                                break;
                            case 2:
                                DeleteContent();
                                break;
                            case 3:
                                SearchContent();
                                break;
                            case 4:
                                DisplayAllContent();
                                break;
                            case 5:
                                AccessContent();
                                break;
                            case 6:
                                ReturnContent();
                                break;
                            case 7:
                                ResetData();
                                break;
                            case 0:
                                exit = true;
                                break;
                            default:
                                Console.WriteLine("Invalid option. Press any key to continue...");
                                Console.ReadKey();
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private void AddContent()
        {
            Console.WriteLine("=== Add New Text Content ===\n");
            Console.WriteLine("(press Backspace to return main menu)\n");
            
            string? title = ReadInputWithBackspace("Enter title: ");
            if (title == null) return;
            
            string? author = ReadInputWithBackspace("Enter author: ");
            if (author == null) return;
            
            int? yearInput = ReadIntWithBackspace("Enter year of publication: ");
            if (yearInput == null) return;
            
            if (yearInput == -1)
            {
                Console.WriteLine("Invalid year format. Press any key to continue...");
                Console.ReadKey();
                return;
            }
            
            string? code = ReadInputWithBackspace("Enter unique content code: ");
            if (code == null) return;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || 
                string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("All fields are required. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                if (_contentService.AddContent(title, author, yearInput.Value, code))
                {
                    Console.WriteLine("Text content added successfully! Press any key to continue...");
                }
                else
                {
                    Console.WriteLine("Failed to add text content. Content with this code may already exist. Press any key to continue...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding text content: {ex.Message}. Press any key to continue...");
            }
            
            Console.ReadKey();
        }

        private void DeleteContent()
        {
            Console.WriteLine("=== Delete Text Content ===\n");
            Console.WriteLine("(press Backspace to return main menu)\n");
            
            string? code = ReadInputWithBackspace("Enter the content code to delete: ");
            if (code == null) return;

            if (string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("Content code is required. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                if (_contentService.DeleteContent(code))
                {
                    Console.WriteLine("Text content deleted successfully! Press any key to continue...");
                }
                else
                {
                    Console.WriteLine("Text content not found. Press any key to continue...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting text content: {ex.Message}. Press any key to continue...");
            }
            
            Console.ReadKey();
        }

        private void SearchContent()
        {
            Console.WriteLine("=== Search Text Content ===\n");
            Console.WriteLine("(press Backspace to return main menu)\n");
            
            Console.WriteLine("1. Search by title");
            Console.WriteLine("2. Search by author");
            
            int? option = ReadIntWithBackspace("\nSelect search option: ");
            if (option == null) return;
            
            if (option == -1)
            {
                Console.WriteLine("Invalid input. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                switch (option.Value)
                {
                    case 1:
                        string? title = ReadInputWithBackspace("Enter title to search: ");
                        if (title == null) return;
                        
                        if (string.IsNullOrWhiteSpace(title))
                        {
                            Console.WriteLine("Search term cannot be empty. Press any key to continue...");
                            Console.ReadKey();
                            return;
                        }
                        var titleResults = _contentService.SearchByTitle(title);
                        DisplayContent(titleResults);
                        break;
                        
                    case 2:
                        string? author = ReadInputWithBackspace("Enter author to search: ");
                        if (author == null) return;
                        
                        if (string.IsNullOrWhiteSpace(author))
                        {
                            Console.WriteLine("Search term cannot be empty. Press any key to continue...");
                            Console.ReadKey();
                            return;
                        }
                        var authorResults = _contentService.SearchByAuthor(author);
                        DisplayContent(authorResults);
                        break;
                        
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching text content: {ex.Message}. Press any key to continue...");
                Console.ReadKey();
            }
        }

        private void DisplayAllContent()
        {
            Console.WriteLine("=== All Text Content ===\n");
            Console.WriteLine("(press Backspace to return main menu)\n");
            
            string? input = ReadInputWithBackspace("Press Enter to display all content or Backspace to return: ");
            if (input == null)
            {
                // User pressed Backspace - the ReadInputWithBackspace method already showed the message
                return;
            }
            
            try
            {
                var contentItems = _contentService.GetAllContent();
                DisplayContent(contentItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving text content: {ex.Message}. Press any key to continue...");
                Console.ReadKey();
            }
        }

        private void DisplayContent(List<TextContent> contentItems)
        {
            if (contentItems == null || contentItems.Count == 0)
            {
                Console.WriteLine("No text content found.");
            }
            else
            {
                Console.WriteLine(new string('-', 80));
                Console.WriteLine($"{"CODE",-10} | {"TITLE",-25} | {"AUTHOR",-20} | {"YEAR",-6} | {"STATUS",-10}");
                Console.WriteLine(new string('-', 80));

                foreach (var content in contentItems)
                {
                    Console.WriteLine($"{content.Code,-10} | {content.Title,-25} | {content.Author,-20} | {content.YearOfPublication,-6} | {content.Status,-10}");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void AccessContent()
        {
            Console.WriteLine("=== Access Text Content ===\n");
            Console.WriteLine("(press Backspace to return main menu)\n");
            
            string? code = ReadInputWithBackspace("Enter the content code to access: ");
            if (code == null) return;

            if (string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("Content code is required. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                if (_contentService.StartSession(code))
                {
                    Console.WriteLine("Text Content accessed successfully! Press any key to continue...");
                }
                else
                {
                    Console.WriteLine("Text Content not found or already being accessed. Press any key to continue...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing Text Content: {ex.Message}. Press any key to continue...");
            }
            
            Console.ReadKey();
        }

        private void ReturnContent()
        {
            Console.WriteLine("=== Return Text Content ===\n");
            Console.WriteLine("(press Backspace to return main menu)\n");
            
            string? code = ReadInputWithBackspace("Enter the content code to return: ");
            if (code == null) return;

            if (string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("Content code is required. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                if (_contentService.EndSession(code))
                {
                    Console.WriteLine("Text Content returned successfully! Press any key to continue...");
                }
                else
                {
                    Console.WriteLine("Text Content not found or not currently accessed. Press any key to continue...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error returning Text Content: {ex.Message}. Press any key to continue...");
            }
            
            Console.ReadKey();
        }

        private void ResetData()
        {
            Console.WriteLine("=== Reset All ===\n");
            Console.WriteLine("(press Backspace to return main menu)\n");
            
            Console.WriteLine("WARNING: This will perform the following actions:");
            Console.WriteLine("");
            Console.WriteLine("1. Create a backup of your current data");
            Console.WriteLine("2. Delete all text content (content.json & temporary files)");
            Console.WriteLine("");
            Console.WriteLine("================================================================");
            Console.WriteLine("\nThis cannot be undone!");
            
            string? response = ReadInputWithBackspace("Proceed? (y/n): ");
            if (response == null) return;
            
            if (response.Trim().ToLower() == "y" || response.Trim().ToLower() == "yes")
            {
                // First confirmation code
                Random random = new Random();
                int firstCode = random.Next(1000, 10000);
                
                Console.WriteLine($"\nFirst confirmation: Enter this 4-digit code: {firstCode}");
                string? firstCodeInput = ReadInputWithBackspace("Enter first confirmation code: ");
                if (firstCodeInput == null) return;
                
                if (firstCodeInput.Trim() != firstCode.ToString())
                {
                    Console.WriteLine("Incorrect first confirmation code. Reset cancelled.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                // Second confirmation code
                int secondCode = random.Next(1000, 10000);
                Console.WriteLine($"\nSecond confirmation: Enter this 4-digit code: {secondCode}");
                string? secondCodeInput = ReadInputWithBackspace("Enter second confirmation code: ");
                if (secondCodeInput == null) return;
                
                if (secondCodeInput.Trim() != secondCode.ToString())
                {
                    Console.WriteLine("Incorrect second confirmation code. Reset cancelled.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                try
                {
                    Console.WriteLine("\nStarting reset process...");
                    Console.WriteLine("1. Creating backup...");
                    System.Threading.Thread.Sleep(200);
                    
                    Console.WriteLine("2. Clearing content...");
                    System.Threading.Thread.Sleep(200);
                    
                    Console.WriteLine("3. Removing files...");
                    System.Threading.Thread.Sleep(200);
                    
                    Console.WriteLine("4. Cleaning up...");
                    System.Threading.Thread.Sleep(200);
                    
                    if (_contentService.ResetAllData())
                    {
                        Console.WriteLine("\nReset completed successfully!");
                        Console.WriteLine("A backup of your data has been created with .backup extension.");
                        Console.WriteLine("Press any key to continue...");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to reset data. Press any key to continue...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError during reset: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                }
            }
            else
            {
                Console.WriteLine("Reset cancelled. Press any key to continue...");
            }
            
            Console.ReadKey();
        }
    }
} 