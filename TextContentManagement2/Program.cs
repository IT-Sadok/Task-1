using System;
using System.Collections.Generic;
using TextContentManagement2;

namespace TextContentManagement2 // refering to the program's location for all .cs files in the folder.
{
    class Program // no "UI" name for simplicity.
    {
        static TextContentRepository repo = new TextContentRepository(); /* creating only one (via "static") instance of "TextContentRepository", where "repo" is an instance of the class. Also called as Singleton P.attern
                                                                          later "repo" instance is used in Add, Search, Delete methods" */
        static void MainUI() // or one instance of method called "MainUI" used for the UI that does not return any value.
        {
            while (true) // running the MainUI until it is terminated.
            {
                Console.Clear(); // clearing the console's UI when an option from 1 - 4 is selected.
                Console.WriteLine("[1] Search\n[2] Add\n[3] Delete\n[4] Exit"); // prints the menu.
                var choice = Console.ReadLine(); // reading what user has inputed ("var").

                switch (choice) // switch statement evaluates the value of choice and executes the corresponding block of code.
                {
                    case "1":         // from 1-4 selection calling the methods from TextContentRepository.
                        SearchMenu(); // calling for SeachMenu() method that will be declared in the future (which also contains its UI).
                        break;        // returning to MainUI().
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
            var results = repo.Search(Console.ReadLine());                      // Calling the Search method on the repo object (which is an instance of TextContentRepository) and passes the user's input as the search term.
            Console.WriteLine("\nResults:");                                    // from previously created results var (variable) 
            results.ForEach(c => Console.WriteLine($"{c.Title} ({c.Year})"));   // lambda expression that defines an action to be performed on each item in the results collection. It prints the title and year of the current item c
            Console.ReadKey();                                                  // pausing the console until any key pressed.
        }

        static void AddMenu()
        {
            Console.Write("Title: ");
            var title = Console.ReadLine();

            Console.Write("Author: ");
            var author = Console.ReadLine();

            Console.Write("Serial Number: ");
            var sn = Console.ReadLine();

            Console.Write("Year: ");
            int year = int.Parse(Console.ReadLine());

            repo.Add(new TextContent(title, author, sn, year));
        }

        static void DeleteMenu()
        {
            Console.Write("Enter title/serial to delete: ");
            repo.Delete(Console.ReadLine());
            Console.WriteLine("Item deleted");
            Console.ReadKey();
        }
    }
}