namespace BunnyBreed
{
    public sealed class CLI
    {
        public static int Enter(string[] args)
        {
            int month;
            if (args.Length < 1) {
                Console.WriteLine("Tell me a month and I'll tell you how many bunnies there will be.");
                while (!Int32.TryParse(Console.ReadLine() ?? "", out month) || (month <= 0)) {
                    Console.WriteLine("I need a positive integer.");
                }
            } else {
                if (!Int32.TryParse(args[0], out month) || (month <= 0)) {
                    Console.WriteLine($"'{args[0]}': not a positive integer");
                    return 0;
                }
            }
            Console.WriteLine($"There will be {Math.Pow(2, month - 1):F0} pair{(month == 1 ? "" : "s")} of bunnies by {month} month.");
            return 0;
        }
    }
}
