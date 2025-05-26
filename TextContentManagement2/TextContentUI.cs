using System;
using System.Collections.Generic;
using TextContentManagement2;

namespace TextContentManagement2
{
    class UI // CHANGE #11 moved from "class Program static void Main {} to class UI public (public for class Program to access) static void StartMenu() {}
    {
        static readonly TextContentRepository repo = new(); /* creating only one (via "static") instance of "TextContentRepository", where "repo" is an instance of the class. Also called as Singleton P.attern
                                                                          later "repo" instance is used in Add, Search, Delete methods" */
                                                           // CHANGE #7: simplified to "= new ();"
                                                           // CHANGE #9: "static readonly (because value is assigned only once). 
        public static void StartMenu() // One instance of method called "Main" used for the UI that does not return any value.
                           // CHANGE #1 - renamed from MainUI to Main because in C# it is an entry point in application!!! 
                           
        {
            while (true) // running the Main until it is terminated.
            {
                Console.Clear(); // clearing the console's UI when an option from 1 - 4 is selected.
                Console.WriteLine("[1] Search\n[2] Add\n[3] Delete\n[4] Exit"); // prints the menu.
                var choice = Console.ReadLine(); // reading what user has inputed ("var").

                switch (choice) // switch statement evaluates the value of choice and executes the corresponding block of code.
                {
                    case "1":         // from 1-4 selection calling the methods from TextContentRepository.
                        SearchMenu(); // calling for SeachMenu() method that will be declared in the future (which also contains its UI).
                        break;        // returning to Main().
                    case "2":
                        AddMenu();
                        break;
                    case "3":
                        DeleteMenu();
                        break;
                    case "4":
                        return;
                }
            }
        }

        static void SearchMenu()
        {
            Console.Write("Enter search term: ");
            var nullSearch = Console.ReadLine();

            if (string.IsNullOrEmpty(nullSearch)) // CHANGE #8 search null check.
            {
                Console.WriteLine("Enter search term");
                return;
            }

            var results = repo.Search(nullSearch); // also CHANGE #8    // Calling the Search method on the repo object (which is an instance of TextContentRepository) and passes the user's input as the search term.
            Console.WriteLine("\nResults:");                                    // from previously created results var (variable) 
            results.ForEach(c => Console.WriteLine($"{c.Title} ({c.Year})"));   // lambda expression that defines an action to be performed on each item in the results collection. It prints the title and year of the current item c
            Console.ReadKey();                                                  // pausing the console until any key pressed.
        }

        static void AddMenu()
        {
            Console.Write("Title: ");
            var title = Console.ReadLine() ?? string.Empty; // CHANGE #3 assigning "" to the string if no input (to avoid null) with ?? string.Empty.

            Console.Write("Author: ");
            var author = Console.ReadLine() ?? string.Empty;

            Console.Write("Serial Number: ");
            var serialNumber = Console.ReadLine() ?? string.Empty; // CHANGE #2 renamed "sn" to "serialNumber"

            Console.Write("Year: ");
            var year = Console.ReadLine() ?? string.Empty;

            repo.Add(new TextContent(title, author, serialNumber, year));
        }

        static void DeleteMenu()
        {
            Console.Write("Enter Title or Serial Number to delete");
            var nullDelete = Console.ReadLine();

            if (string.IsNullOrEmpty(nullDelete)) // CHANGE #8 delete null check
            {
                Console.WriteLine("Enter Title or Serial Number to delete");
                return;
            }

            repo.Delete(nullDelete);
            Console.WriteLine("Item deleted");
            Console.ReadKey();
        }
    }
}


