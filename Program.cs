using System;
using System.Collections.Generic;
using System.Linq;

namespace TestConsole
{
    class Program
    {

        static Dictionary<string, ReplCommand> _commands = new Dictionary<string, ReplCommand>(StringComparer.CurrentCultureIgnoreCase);

        static void Main(string[] args)
        {
            // Create Commands
            PopulateCommands();

            // Run continuously until "quit" is entered
            while (true)
            {
                // Ask the user to enter their command
                Console.WriteLine("Please input your command and hit enter");
                // Capture the input
                string sInput = Console.ReadLine();
                // Search the input from within the commands
                if (_commands.TryGetValue(sInput, out ReplCommand c))
                {
                    // Found the command. Let's execute it
                    c.MethodToCall(sInput);
                }
                else
                {
                    // Command was not found, trigger the help text
                    PrintHelp(sInput);
                }
            }


        }

        static void MyCommand(string input)
        {
            Console.WriteLine($"MyCommand has been executed by the input '{input}'");
        }

        static void PrintHelp(string input)
        {
            // Unless the input that got us here is 'help', display the (wrong) command that was
            // entered that got us here
            if (input?.ToLowerInvariant() != "help")
            {
                // Display the wrong command
                Console.WriteLine($"Command '{input}' not recognized. See below for valid commands");
            }

            // Loop through each command from a list sorted by the HelpSortOrder
            foreach (ReplCommand c in _commands.Values.OrderBy(o => o.HelpSortOrder))
            {
                // Print the command and its associated HelpText
                Console.WriteLine($"{c.Command}:\t{c.HelpText}");
            }
        }

        static void Quit(string input)
        {
            System.Environment.Exit(0);
        }

        static void PopulateCommands()
        {
            // Add your commands here
            AddCommand(new ReplCommand
            {
                Command = "MyCommand", // The command that the user will enter (case insensitive)
                HelpText = "This is the help text of my command", // Help text
                MethodToCall = MyCommand, // The actual method that we will trigger
                HelpSortOrder = 1 // The order in which the command will be displayed in the help
            });

            // Default Commands
            AddCommand(new ReplCommand
            {
                Command = "help",
                HelpText = "Prints usage information",
                MethodToCall = PrintHelp,
                HelpSortOrder = 100
            });
            AddCommand(new ReplCommand
            {
                Command = "quit",
                HelpText = "Terminates the console application",
                MethodToCall = Quit,
                HelpSortOrder = 101
            });

        }

        static void AddCommand(ReplCommand replCommand)
        {
            // Add the command into the dictionary to be looked up later
            _commands.Add(replCommand.Command, replCommand);
        }

    }

    // A class we will use to capture the REPL Command information
    class ReplCommand
    {

        public string Command { get; set; }
        public string HelpText { get; set; }
        public Action<string> MethodToCall { get; set; }
        public int HelpSortOrder { get; set; }

    }

}



