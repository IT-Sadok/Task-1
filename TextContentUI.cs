using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextContentManagement;

 public class ConsoleUI
 {
     private readonly TextContentRepository _repository = new();

     public void Run() // Creating a method (action) to operate the console.
     {
         while(true) // Command for displaying the console.
         {
             ShowMenu(); // declaration the method for displaying the UI;
             HandleChoice(Console.Readline()); // declaring method for choosing options in the ShowMenu() method.
         }
     }

     private void ShowMenu() // Making the ShowMenu() method to display options for managing the TextContent. Private because other .cs files do not need it;
     {
         Console.Clear();
         Console.WriteLine("1.Add Text Content");
         Console.WriteLine("2.Delete Text Content");
         Console.WriteLine("3.View all Text Content");
         Console.WriteLine("4.Exit");
         Console.WriteLine("Enter option N# to proceed");
     }
     
 } // Investigating how to make adding and deleting Text Content compact
