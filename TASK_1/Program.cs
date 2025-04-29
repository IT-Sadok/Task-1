using TASK_1.Interfaces;
using TASK_1.Repositories;
using TASK_1.Services;
using TASK_1.UI;
using System.IO;

try
{
    // Finding the Task 1 folder regardless of where the executable is located
    string currentDir = AppContext.BaseDirectory;
    string task1Folder = currentDir;

    // Try to locate "Task 1" folder by searching up the directory tree
    while (!string.IsNullOrEmpty(task1Folder) && 
           !Path.GetFileName(task1Folder).Equals("Task 1", StringComparison.OrdinalIgnoreCase) && 
           Directory.GetParent(task1Folder) != null)
    {
        DirectoryInfo? parent = Directory.GetParent(task1Folder);
        if (parent == null) break;
        task1Folder = parent.FullName;
    }

    // If we couldn't find "Task 1" folder, fall back to executable's directory
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

        // Display logo before starting the application
        LogoDisplay logoDisplay = new LogoDisplay();
        logoDisplay.DisplayLogo();

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
