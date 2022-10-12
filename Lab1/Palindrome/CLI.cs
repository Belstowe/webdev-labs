namespace Palindrome
{
    public sealed class CLI
    {
        public static bool IsPalindrome(string word)
        {
            var cleanWord = word.ToLower().Where(c => (c >= 'a' && c <= 'z') || (c >= 'а' && c <= 'я') || (c >= '0' && c <= '9'));
            return Enumerable.SequenceEqual(cleanWord, cleanWord.Reverse());
        }
        public static int Enter(string[] args)
        {
            string inputWord;
            if (args.Length < 1) {
                Console.WriteLine("Please input your word to check if it is a palindrome.");
                inputWord = Console.ReadLine() ?? "";
            } else {
                inputWord = args[0];
            }

            if (IsPalindrome(inputWord)) {
                Console.WriteLine($"Word '{inputWord}' is a palindrome.");
            } else {
                Console.WriteLine($"Word '{inputWord}' is NOT a palindrome.");
            }
            return 0;
        }
    }
}
