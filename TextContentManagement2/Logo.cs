using System;
using System.Threading; // Need for Thread.Sleep();

public class Logo
{
    public static void LogoDisplay()
    {
        Console.ForegroundColor = ConsoleColor.Cyan; // Cyan color.
        Console.BackgroundColor = ConsoleColor.DarkRed;

        Console.WriteLine("========================================================================");
        Console.WriteLine("                                 .                                      ");
        Console.WriteLine("                  TASK 1 - Library Management Console                   ");
        Console.WriteLine("                               a.k.a                                    ");
        Console.WriteLine("                      TextContentManagement2                            ");
        Console.WriteLine("Repository: Bogdan-Novichok-Group-CSharp-Progress/TextContentManagement2");
        Console.WriteLine("                                 .                                      ");
        Console.WriteLine("                           July 3, 2025                                 ");
        Console.WriteLine("========================================================================");
        Thread.Sleep(4000);
}
}