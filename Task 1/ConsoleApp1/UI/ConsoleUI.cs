using ConsoleApp1.Models;
using ConsoleApp1.Services;

namespace ConsoleApp1.UI
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
                    System.Threading.Thread.Sleep(500); // Pause briefly
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
            if (input == null) return null; // pressing Backspace to go back
            
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
                Console.WriteLine("7. Reset All Content");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("0. Exit");
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
            
            string? title = ReadInputWithBackspace("Enter Title: ");
            if (title == null) return;
            
            string? author = ReadInputWithBackspace("Enter Author: ");
            if (author == null) return;
            
            int? yearInput = ReadIntWithBackspace("Enter Year of Publication: ");
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
                    Console.WriteLine("Text Content added successfully. Press any key to continue...");
                }
                else
                {
                    Console.WriteLine("Failed to add Text Content. Content with this code may already exist. Press any key to continue...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Text Content: {ex.Message}. Press any key to continue...");
            }
            
            Console.ReadKey();
        }

        private void DeleteContent()
        {
            Console.WriteLine("=== Delete Text Content ===\n");
            Console.WriteLine("(press Backspace to return Main Menu)\n");
            
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
                    Console.WriteLine("Text Content deleted successfully. Press any key to continue...");
                }
                else
                {
                    Console.WriteLine("Text Content not found. Press any key to continue...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Text Content: {ex.Message}. Press any key to continue...");
            }
            
            Console.ReadKey();
        }

        private void SearchContent()
        {
            Console.WriteLine("=== Search Text Content ===\n");
            Console.WriteLine("(press Backspace to return main menu)\n");
            
            Console.WriteLine("1. Search by Title");
            Console.WriteLine("2. Search by Author");
            
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
                Console.WriteLine($"Error searching Text Content: {ex.Message}. Press any key to continue...");
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
                // Pressing Backspace - the ReadInputWithBackspace method already showed the message
                return;
            }
            
            try
            {
                var contentItems = _contentService.GetAllContent();
                DisplayContent(contentItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Text Content: {ex.Message}. Press any key to continue...");
                Console.ReadKey();
            }
        }

        private void DisplayContent(List<TextContent> contentItems)
        {
            if (contentItems == null || contentItems.Count == 0)
            {
                Console.WriteLine("No Text Content found.");
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
            Console.WriteLine("=== Reset All Content ===\n");
            Console.WriteLine("(press Backspace to return main menu)\n");
            
            Console.WriteLine("WARNING: This will delete All Text Content and reset the system to its initial state.");
            
            string? response = ReadInputWithBackspace("Are you sure you want to proceed? (y/n): ");
            if (response == null) return;
            
            if (response.Trim().ToLower() == "y" || response.Trim().ToLower() == "yes")
            {
                // Generate a random 4-digit confirmation code
                Random random = new Random();
                int confirmationCode = random.Next(1000, 10000); // Generates a number between 1000 and 9999
                
                Console.WriteLine($"\nFor security, please enter the following 4-digit code to confirm: {confirmationCode}");
                
                string? codeInput = ReadInputWithBackspace("Enter confirmation code: ");
                if (codeInput == null) return;
                
                if (codeInput.Trim() == confirmationCode.ToString())
                {
                    try
                    {
                        if (_contentService.ResetAllData())
                        {
                            Console.WriteLine("All content has been reset successfully! Press any key to continue...");
                        }
                        else
                        {
                            Console.WriteLine("Failed to reset content. Press any key to continue...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error resetting content: {ex.Message}. Press any key to continue...");
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect confirmation code. Reset operation cancelled. Press any key to continue...");
                }
            }
            else
            {
                Console.WriteLine("Reset operation cancelled. Press any key to continue...");
            }
            
            Console.ReadKey();
        }
    }
} 