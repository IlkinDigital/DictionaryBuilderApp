using System;

namespace DictionaryBuilderApp
{
    internal static class ConsoleUtils
    {
        public static string Ask(string prompt)
        {
            string? res;
            while (true)
            {
                Console.Write(prompt);
                res = Console.ReadLine();

                if (res == null)
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }
                else
                    break;
            }

            return res;
        }

        public static string AskOptions(string prompt, string[] options)
        {
            string? res;
            while (true)
            {
                Console.Write(prompt);
                res = Console.ReadLine();

                if (res == null || !options.Contains(res))
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }
                else
                    break;
            }

            return res;
        }
    }
}