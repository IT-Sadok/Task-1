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
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\n========================================================================");
                Console.WriteLine("Text Content Records                                                    ");
                Console.WriteLine("\n1 - Add New;                                                          ");
                Console.WriteLine("2 - Access;                                                             ");
                Console.WriteLine("3 - Sort by;                                                            ");
                Console.WriteLine("4 - Search;                                                             ");
                Console.WriteLine("5 - Delete;                                                             ");
                Console.WriteLine("\n6 - Save & Exit.                                                      ");
                Console.WriteLine("\non choosing # press ENTER to proceed:                                 ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": AddTextContent(); break;
                    case "2": AccessTextContent(); break;
                    case "3": SortTextContent(); break;
                    case "4": SearchTextContent(); break;
                    case "5": DeleteTextContent(); break;
                    case "6": SaveAndExit(); return;
                    default: Console.WriteLine("Choose from 1 - 6 to proceed"); break;
                }
            }
        }

        private void AddTextContent()
        {
            Console.WriteLine("\n====================================================");
            Console.WriteLine("Enter Title: ");
            string title = Console.ReadLine()?.Trim() ?? "";                    // Check if not null "?." before proceeding, remove "Trim()" white spaces after input, assign "??" ""(null) if left side is empty.

            Console.WriteLine("Enter Author: ");
            string author = Console.ReadLine()?.Trim() ?? "";

            Console.WriteLine("Enter Serial Number: ");
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
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Enter Valid Year. ");
                }
            }

            // Rejection if all fields are empty.
            if (string.IsNullOrWhiteSpace(title) &&
                string.IsNullOrWhiteSpace(author) &&
                string.IsNullOrWhiteSpace(serialNumber) &&
                year == null)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Entry Rejected. ");
                return; // Exit without saving.
            }
            
            _record.Add(new TextContent(title, author, serialNumber, year));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Text Content Added.");
        }

        private void AccessTextContent()
        {
            var item = SelectItem("Access");
            if (item == null) return;

            if (!item.Availability)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Not Available.");
                return;
            }

            item.Availability = false;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Accessed: {item.Title}");
        }

        private void SortTextContent()
        {
            if (!_record.Any())
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Not Found.");
                return;
            }
            Console.WriteLine("\n============================");
            Console.WriteLine("Sort by:");
            Console.WriteLine("\n1 - Title (A-Z);");
            Console.WriteLine("2 - Title (Z-A);");
            Console.WriteLine("\n3 - Year (Newest);");
            Console.WriteLine("4 - Year (Oldest);");
            Console.WriteLine("\n5 - Group by Author;");
            Console.WriteLine("6 - Group by Year.");

            // "=>" was for returning values, ":" is for performing action (because of GroupBy introduction), needing "break" to contain condition;
            // LINQ use in OrderBy and ToList extension methods (present before grouping methods).
            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                Console.WriteLine("\n========================================================================");
                    DisplayItems(_record.OrderBy(x => x.Title).ToList()); // Descending letters, after outputing in a list format.
                    Console.WriteLine("");
                    break;
                case "2":
                Console.WriteLine("\n========================================================================");
                    DisplayItems(_record.OrderByDescending(x => x.Title).ToList());
                    Console.WriteLine("");
                    break;
                case "3":
                Console.WriteLine("\n========================================================================");
                    DisplayItems(_record.OrderByDescending(x => x.YearValue).ToList()); // Descending years.
                    Console.WriteLine("");
                    break;
                case "4":
                Console.WriteLine("\n========================================================================");
                    DisplayItems(_record.OrderBy(x => x.YearValue).ToList());
                    Console.WriteLine("");
                    break;
                case "5":
                    GroupByAuthor();
                    break;
                case "6":
                    GroupByYear();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Choose from 1 - 6 to proceed");
                    break;
            }
            ;                                                                   
        }

        // LINQ GroupBy introduced.
        private void GroupByAuthor()
        {
            Console.WriteLine("\n========================================================================");
            var byAuthor = _record
            .GroupBy(item => item.Author) // First grouping.
            .OrderBy(g => g.Key);         // Sorting after grouping (similar to ToList).

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nTotal Authors: {byAuthor.Count()}");

            foreach (var group in byAuthor)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"\nAuthor: {group.Key}, {group.Count()} record(s):"); // Output format;
                DisplayItems(group.ToList());                                            // Using the display method of TextContent's parameters.
            }
            Console.WriteLine();
        }

        private void GroupByYear()
        {
            Console.WriteLine("\n========================================================================");
            var byYear = _record
            .GroupBy(item => item.YearValue)
            .OrderByDescending(g => g.Key);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nYear Records: {byYear.Count()}");

            foreach (var group in byYear)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                string yearDisplay = group.First().YearDisplay;
                Console.WriteLine($"\nYear: {yearDisplay}, {group.Count()} record(s):");
                DisplayItems(group.ToList());
            }
            Console.WriteLine("");
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

            Console.ForegroundColor = ConsoleColor.White;
            DisplayItems(results);
        }

        private void DeleteTextContent()
        {
            var item = SelectItem("Delete");
            if (item == null) return;

            _record.Remove(item);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Deleted: {item.Title}");
        }

        private void SaveAndExit()
        {
            _storage.SaveTextContent(_record);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Text Content saved. Exiting ...");
            Console.WriteLine("");
            Console.ResetColor(); // Accessible runtime to reset the console's color.
            Environment.Exit(0);
        }

        // DisplayItems() and SelectItem() helper methods.
        private void DisplayItems(List<TextContent> items)
        {
            if (!items.Any())
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Not Found.");
                return;
            }

            // A loop itirating over "items", where "i" is the collection's count, incrementing by 1 "i++" until reaching last item "<items.Count;"
            for (int i = 0; i < items.Count; i++)
            {
                // The loop.
                var item = items[i];
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\n{i + 1}. {item.Title} ({item.YearDisplay}) by {item.Author}, Serial Number: {item.SerialNumber}; " // "{i + 1}" displays next item, showing "?" availability.
                + $"Status: {(item.Availability ? "Available" : "Not Available")}");
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
                Console.ForegroundColor = ConsoleColor.White;
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

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Invalid Term.");
            return null;
        }
    }
}