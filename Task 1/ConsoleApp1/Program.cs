using ConsoleApp1.Interfaces;
using ConsoleApp1.Repositories;
using ConsoleApp1.Services;
using ConsoleApp1.UI;
using System.IO;

try
{
    // Finding the Task 1 folder regardless of where the executable is located
    string currentDir = AppContext.BaseDirectory;
    string task1Folder = currentDir;

    // Locating "Task 1" folder by searching up the directory tree
    while (!string.IsNullOrEmpty(task1Folder) && 
           !Path.GetFileName(task1Folder).Equals("Task 1", StringComparison.OrdinalIgnoreCase) && 
           Directory.GetParent(task1Folder) != null)
    {
        DirectoryInfo? parent = Directory.GetParent(task1Folder);
        if (parent == null) break;
        task1Folder = parent.FullName;
    }

    // If can't find "Task 1" folder, fall back to executable's directory
    if (!Path.GetFileName(task1Folder).Equals("Task 1", StringComparison.OrdinalIgnoreCase))
    {
        task1Folder = currentDir;
    }

    // Define the path for storing content data directly in the Task 1 folder
    string dataFilePath = Path.Combine(task1Folder, "content.json");

    try
    {
        // Create the repository, service, and UI components
        ITextContentRepository contentRepository = new JsonTextContentRepository(dataFilePath);
        ContentService contentService = new ContentService(contentRepository);
        ConsoleUI userInterface = new ConsoleUI(contentService);

        // Start the application
        Console.WriteLine("Starting Text Content Management System...");
        Console.WriteLine($"Content data is stored in: {dataFilePath}");
        userInterface.Start();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error starting the application: {ex.Message}");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Critical error: {ex.Message}");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
