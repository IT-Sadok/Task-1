using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;

namespace TextContentManagement2
{
    public class UI
    {
        private readonly List<TextContent> _record;
        private readonly Storage _storage;

        public UI(List<TextContent> record, Storage storage)
        {
            _record = record;
            _storage = storage;
        }


        public void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("\nText Content Records");
                Console.WriteLine("1 - Add New;");
                Console.WriteLine("2 - Access;");
                Console.WriteLine("3 - View All;");
                Console.WriteLine("4 - Search;");
                Console.WriteLine("5 - Delete;");
                Console.WriteLine("6 - Save & Exit.");
                Console.Write("on choosing # press ENTER to proceed: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": AddTextContent(); break;
                    case "2": AccessTextContent(); break;
                    case "3": ViewTextContent(); break;
                    case "4": SearchTextContent(); break;
                    case "5": DeleteTextContent(); break;
                    case "6": SaveAndExit(); return;
                    default: Console.WriteLine("Choose from 1 - 6 to proceed"); break;
                }
            }
        }

        private void AddTextContent()
        {
            Console.Write("Enter Title: ");
            string title = Console.ReadLine()?.Trim() ?? "";                    // Check if not null "?." before proceeding, remove "Trim()" white spaces after input, assign "??" ""(null) if left side is empty.

            Console.Write("Enter Author: ");
            string author = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter Serial Number: ");
            string serialNumber = Console.ReadLine()?.Trim() ?? "";


            // Correct year input.
            int? year = null;
            bool yearCheck = false;
            while (!yearCheck)
            {
                Console.Write("Enter Year (negative numbers for BC, e.g. -10 = 10 BC): ");
                string yearInput = Console.ReadLine()?.Trim() ?? ""; // if "??" null, converts to an empty string.

                // blank to "NO ENTRY".
                if (string.IsNullOrWhiteSpace(yearInput))
                {
                    year = null;
                    yearCheck = true;
                }
                // Valid number check.
                else if (int.TryParse(yearInput, out int parsedYear))
                {
                    year = parsedYear;
                    yearCheck = true;

                }
                else
                {
                    Console.WriteLine("Enter Valid Year. ");
                }

            }

            // Rejection if all fields are empty.
            if (string.IsNullOrWhiteSpace(title) &&
                string.IsNullOrWhiteSpace(author) &&
                string.IsNullOrWhiteSpace(serialNumber) &&
                year == null)
            {
                Console.WriteLine("Entry Rejected. ");
                return; // Exit without saving.
            }
            
            _record.Add(new TextContent(title, author, serialNumber, year));
            Console.WriteLine("Text Content Added.");
        }

        private void AccessTextContent()
        {
            var item = SelectItem("Access");
            if (item == null) return;

            if (!item.Availability)
            {
                Console.WriteLine("Already in Access.");
                return;
            }

            item.Availability = false;
            Console.WriteLine($"Accessed: {item.Title}");
        }

        private void ViewTextContent()
        {
            if (!_record.Any())
            {
                Console.WriteLine("Not Found.");
                return;
            }

            Console.WriteLine("\nSort by:");
            Console.WriteLine("1 - Title (A-Z)");
            Console.WriteLine("2- Title (Z-A)");
            Console.WriteLine("3 - Year (Newest)");
            Console.WriteLine("4 - Year (Oldest)");

            // Display options for all TextContent.
            List<TextContent> sorted = Console.ReadLine() switch
            {
                "1" => _record.OrderBy(item => item.Title).ToList(),              // key assigns "=>" list collection "_record" to order every "item" by "item.Title", converting "ToList()" to a list
                "2" => _record.OrderByDescending(item => item.Title).ToList(),
                "3" => _record.OrderByDescending(item => item.YearValue).ToList(),
                "4" => _record.OrderBy(item => item.YearValue).ToList(),
                _ => _record                                                     // returning "_ =>" Text Contents "_record" once sorting option is chosen.           
            };                                                                   // need ";" after a lambda expression.
            // Display (method below) "sorted" items.
            DisplayItems(sorted);
        }

        // Search with wide match for user freedom enabled.
        private void SearchTextContent()
        {
            Console.Write("Enter Search Term: ");
            string term = Console.ReadLine()?.Trim().ToLower() ?? ""; // If not null "?." before proceeding, remove white spaces after input "Trim()", make searched term non-case sensitive "ToLower()". Assign ""(null)" if left empty "??".

            if (string.IsNullOrWhiteSpace(term))
            {
                Console.WriteLine("Enter Search Term: ");
                return;
            }

            var results = _record.Where(item =>                                      // Declaring "var results =", holding expressions from "_record", based on "Where(...)" searched item.
            item.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||         // Check if an item in a Text Content Tilte "item.Tite" contains ".Contains", checking if input matches "term,", case-insensitive "String.Comparison.OrdinalIgnoreCase". OR "||".
            item.Author.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            item.SerialNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            item.YearDisplay.Contains(term)
            ).ToList();                                                              // Calling filtered resulsts to the list. Expressin closed ";".

            DisplayItems(results);
        }

        private void DeleteTextContent()
        {
            var item = SelectItem("Delete");
            if (item == null) return;

            _record.Remove(item);
            Console.WriteLine($"Deleted: {item.Title}");
        }

        private void SaveAndExit()
        {
            _storage.SaveTextContent(_record);
            Console.WriteLine("Text Content saved. Exiting ...");
            Console.ResetColor(); // Accessible runtime to reset the console's color.
            Environment.Exit(0);
        }

        // DisplayItems() and SelectItem() helper methods.
        private void DisplayItems(List<TextContent> items)
        {
            if (!items.Any())
            {
                Console.WriteLine("Not Found.");
                return;
            }

            Console.WriteLine("\nText Content Status");
            Console.WriteLine("--------------------------------------");

            // A loop itirating over "items", where "i" is the collection's count, incrementing by 1 "i++" until reaching last item "<items.Count;"
            for (int i = 0; i < items.Count; i++)
            {
                // The loop.
                var item = items[i];
                Console.WriteLine($"{i + 1}. {item.Title} ({item.YearDisplay}) by {item.Author}, Serial Number: {item.SerialNumber}; " // "{i + 1}" displays next item, showing "?" availability.
                + $"{(item.Availability ? "Available" : "Not Available")}");
            }
        }

        // Method of selecting Text Content item.
        private TextContent? SelectItem(string action)
        {
            Console.Write($"Enter Title to {action}: ");               // "action" is a parameter passed to "Add", "Search", etc.
            string term = Console.ReadLine()?.Trim().ToLower() ?? "";

            var matches = _record
            .Where(item => item.Title.Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToList();

            if (!matches.Any())
            {
                Console.WriteLine("Not Found.");
                return null;
            }

            // Method to display matching items search and display sorted Text Contents (record).
            DisplayItems(matches);

            if (matches.Count == 1) return matches[0];                                                   // if only one item in the list "matches.Count == 1", "return" the item item "matches[0];

            Console.Write("Enter Item Number.");

            // Displaying items in a list for user to select a match.
            if (int.TryParse(Console.ReadLine(), out int match) && match > 0 && match <= matches.Count) // Parsing list option to int "int.TryParse(..., our int match). Checking if match is within the list options "match > 0 && matches.Count".
                return matches[match - 1];                                                              // List starts from 0. If user selects 1, need to substract to match it.

            Console.WriteLine("Invalid Term.");
            return null;
        }
    }
}