using System;
using System.Threading;

namespace TASK_1.UI
{
    public class LogoDisplay
    {
        private const string LOGO = @"
        
████████  █████  ███████ ██   ██     ██
   ██    ██   ██ ██      ██  ██      ██
   ██    ███████ ███████ █████       ██
   ██    ██   ██      ██ ██  ██      ██
   ██    ██   ██ ███████ ██   ██▄▄▄▄▄██

        LIBRARY MANAGEMENT - APRIL 2025
";

        public void DisplayLogo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(LOGO);
            Console.ResetColor();
            
            // Add a 2-second delay to make the logo visible
            Thread.Sleep(2000);
        }
    }
} 