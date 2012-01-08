using System;
using System.IO;

namespace spellcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            LetterTree words = new LetterTree();

            string path = "words";
            if (args.Length != 0)
            {
                if (args[0].Trim() == "help" || args[0].Trim() == "/?"
                    || args[0].Trim() == "?")
                {
                    PrintUsage();
                    return;
                }
                else
                    path = args[0];
            }

            if (PopulateWords(words, path))
            {
                string line = string.Empty;
                while (true)
                {
                    Console.Write("> ");

                    do
                    {
                        line = Console.ReadLine();


                        if (line != null)
                        {
                            line = line.Trim();
                            Console.WriteLine(words.Spellcheck(line));
                        }
                    } while (line == null);
                }
            }
        }

        static bool PopulateWords(LetterTree Words, string filePath)
        {
            string line;
            try
            {
                using (StreamReader sr = new StreamReader(Path.GetFullPath(filePath)))
                {
                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        if (line != string.Empty)
                            Words.Add(line);
                    }
                }

                return true;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("\nNo file found at: \"{0}\"\n", filePath);
                PrintUsage();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Welcome to Charlie's Amazing Spellchecker!!");
            Console.WriteLine("");
            Console.WriteLine("Usage:");
            Console.WriteLine("");
            Console.WriteLine("spellcheck <optional file name>");
            Console.WriteLine("If you want to call it without a filename, put the dictionary");
            Console.WriteLine("file in this directory ({0}) and name it words", Environment.CurrentDirectory);
        }
    }
}
