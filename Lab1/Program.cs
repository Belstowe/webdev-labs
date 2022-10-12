public sealed class Program
{
    private static readonly Dictionary<string, (Func<string[], int> Command, string Help)> commands = new Dictionary<string, (Func<string[], int> Command, string Help)> {
        ["temp"] = (args => TempScale.CLI.Enter(args), "Temperature scale conversion"),
        ["paln"] = (args => Palindrome.CLI.Enter(args), "Palindrome check tool"),
        ["bunbr"] = (args => BunnyBreed.CLI.Enter(args), "Bunny breeding analyzer"),
        ["csv"] = (args => CSVStat.CLI.Enter(args), "CSV stats analyzer"),
        ["help"] = (_ => { Console.Write(Help(null)); return 0; }, "Print this text"),
        ["exit"] = (_ => 1, "Exit the program"),
    };

    private static string Help(string? commandName)
    {
        if (commandName == null) {
            string helpText = "";
            foreach (var command in commands) {
                helpText += $"[{command.Key}]\n\t{command.Value.Help}\n";
            }
            return helpText;
        }
        return $"[{commandName}]\n\t{commands[commandName].Help}\n";
    }

    public static void Main(string[] args)
    {
        if (args.Length != 0) {
            if (Program.commands.TryGetValue(args[0], out var subcmd)) {
                subcmd.Command(args[1..]);
            } else {
                Help(null);
            }
            return;
        }
        Console.Write(Help(null));

        /*
        string[] lineTokens;
        (Func<string[], int> Command, string Help) cmdCall;
        do {
            Console.WriteLine("=====================");
            var line = Console.ReadLine() ?? "";
            lineTokens = line.Split(' ');
            if (!Program.commands.TryGetValue(lineTokens[0], out cmdCall)) {
                cmdCall = Program.commands["help"];
            }
        } while (cmdCall.Command(lineTokens[1..]) == 0);
        Console.WriteLine("Farewell!");
        */
    }
}
