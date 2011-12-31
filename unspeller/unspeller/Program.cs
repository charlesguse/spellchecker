using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace unspeller
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!ArgsAreCorrect(args))
                return;


            int timesToRun;
            timesToRun = int.Parse(args[0]);

            string path = "words";
            if (args.Length == 2)
                path = args[1];

            string[] words;
            words = PopulateWords(path);

            if (words.Length > 0)
            {
                StringBuilder wordsUnspelled = new StringBuilder();
                Random rand = new Random();
                string wordToUnspell;

                while (timesToRun > 0)
                {
                    wordToUnspell = "Esterházy's";//words[rand.Next(words.Length)];

                    wordsUnspelled.Append(wordToUnspell);
                    wordsUnspelled.Append('\n');

                    Console.WriteLine(unspell.UnspellWord(wordToUnspell));
                    timesToRun--;
                }

                System.IO.File.WriteAllText("unspelledOutput.txt", wordsUnspelled.ToString());
            }
        }

        static bool ArgsAreCorrect(string[] args)
        {
            // Make sure there is only one or two args passed in
            if (args.Length != 1 && args.Length != 2)
            {
                PrintUsage();
                return false;
            }

            if (args[0].Trim() == "help" || args[0].Trim() == "/?"
                || args[0].Trim() == "?")
            {
                PrintUsage();
                return false;
            }

            // First arg needs to be the number of times the app is run.
            try
            {
                int.Parse(args[0]);
            }
            catch
            {
                PrintUsage();
                return false;
            }

            return true;
        }

        static string[] PopulateWords(string filePath)
        {
            List<string> words = new List<string>();
            string line;
            try
            {
                using (StreamReader sr = new StreamReader(Path.GetFullPath(filePath)))
                {
                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        if (line != string.Empty)
                            words.Add(line);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("\nNo file found at: \"{0}\"\n", filePath);
                PrintUsage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return words.ToArray();
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Welcome to Charlie's Amazing Unspeller!!");
            Console.WriteLine("");
            Console.WriteLine("Usage:");
            Console.WriteLine("");
            Console.WriteLine("unspell <# of times to run> <optional file name>");
            Console.WriteLine("If you want to call it without a filename, put the dictionary");
            Console.WriteLine("file in this directory ({0}) and name it words", Environment.CurrentDirectory);
        }
    }
}
