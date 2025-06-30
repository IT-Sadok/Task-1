using System;
using System.Collections.Generic;
using TextContentManagement2;

class Program
{
    static void Main()
    {
        // 1. Initializing Storage Selector.
        var storagePath = new StoragePath();
        storagePath.StorageOption();

        if (storagePath.ChosenStoragePath == null)
        {
            Console.WriteLine("Storage Set-up Failed.");
            return;
        }

        // 2. Initialize Storage.
        var storage = new Storage(storagePath.ChosenStoragePath);
        List<TextContent> records = storage.LoadTextContent();

        // 3. Start UI.
        var ui = new UI(records, storage);
        ui.MainMenu();
    }
}