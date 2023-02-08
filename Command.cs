using System;
using System.Collections.Generic;
using System.Threading;

namespace NetworkApplication
{
    class Command
    {
        private delegate void ConsoleInput(string[] arguments);
        private struct ConsoleCommand
        {
            public ConsoleInput action;
            public string[] description;

            public ConsoleCommand(ConsoleInput action, params string[] description)
            {
                this.action = action;
                this.description = description;
            }
        }

        private static Dictionary<string, ConsoleCommand> commands;

        public static void InitializeThread()
        {
            InitializeCommands();

            while (true)
            {
                try
                {
                    bool commandSuccess = false;

                    string input = Console.ReadLine();
                    string[] segments = input.Split(' ');
                    foreach(KeyValuePair<string, ConsoleCommand> command in commands)
                    {
                        if(command.Key.ToLower() == segments[0].ToLower())
                        {
                            command.Value.action(segments);
                            commandSuccess = true;
                            break;
                        }
                    }

                    if (commandSuccess)
                        continue;

                    Console.WriteLine("Command failed: Command does not exist. Use command \"cmds\" or \"help\" for a list of commands.");
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Command failed: {exception}.");
                }
            }
        }

        private static void InitializeCommands()
        {
            commands = new Dictionary<string, ConsoleCommand>()
            {
                { "help", new ConsoleCommand(Commands.Help, "Lists all commands.", "No arguments expected.") },
                { "cmds", new ConsoleCommand(Commands.Help, "Lists all commands.", "No arguments expected.") },
                { "shut", new ConsoleCommand(Commands.CloseServer, "Shuts down the server.", "No arguments expected.") },
                { "kick", new ConsoleCommand(Commands.KickClient, "Sends request to client to close connection to server.",
                    "When request is handled, confirmation message will be sent back to server.", "Argument expected: Client ID.") },
            };
        }

        private class Commands
        {
            public static void Help(string[] args)
            {
                foreach (KeyValuePair<string, ConsoleCommand> command in commands)
                {
                    Console.Write($"{command.Key} -> ");
                    for (int i = 0; i < command.Value.description.Length; i++)
                    {
                        string line = command.Value.description[i];

                        string filler = "";
                        if (i > 0) {
                            filler = "    ";
                            for (int j = 0; j < command.Key.Length; j++)
                                filler += " ";
                        }
                        
                        Console.Write($"{filler}{line}\n");
                    }
                }
            }

            public static void CloseServer(string[] args)
            {
                Console.WriteLine("Are you sure you want to close the server? Y/N");

                //Check for confirmation
                string confirmation = Console.ReadLine().Split(' ')[0];
                while (confirmation.ToUpper() != "Y" && confirmation.ToUpper() != "N")
                {
                    Console.WriteLine("Do you want to shut down the server? Y/N");
                    confirmation = Console.ReadLine().Split(' ')[0];
                }

                if (confirmation.ToUpper() == "Y")
                {
                    Console.WriteLine("Shutting down server!");
                    Environment.Exit(0);
                }
                Console.WriteLine("Shutdown cancelled.");
            }

            public static void KickClient(string[] args)
            {
                if (args.Length <= 1)
                {
                    Console.WriteLine("More arguments expected!");
                    return;
                }
                if (!int.TryParse(args[1], out int clientId))
                {
                    Console.WriteLine($"First argument should contain an integer, contained: {args[1]}!");
                    return;
                }
                if (!Server.clients.ContainsKey(clientId))
                {
                    Console.WriteLine($"No client has ID {clientId}!");
                    Console.WriteLine($"Available IDs: 0..{Server.maxPlayers}.");
                    return;
                }
                if (Server.clients[clientId].tcp.socket == null)
                {
                    Console.WriteLine($"No connected client has ID {clientId}!");
                    return;
                }

                Console.WriteLine($"Request to close connection to client {clientId} has been sent.");
                ServerSend.KickClient(clientId);
            }
        }
    }
}
