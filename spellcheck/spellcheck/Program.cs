using System;
using System.IO;
using spellcheckLibrary;

namespace spellcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            LetterTree Words = new LetterTree();

            //Test(Words);

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

            if (PopulateWords(Words, path))

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                Console.WriteLine(Words.Spellcheck(line));
            }
        }

        static void Test(LetterTree Words)
        {
            Words.Add("hi");
            Words.Add("hi");
            Words.Spellcheck("hi");
            Words.Add("sheep");
            Words.Add("people");
            Words.Add("inside");
            Words.Add("job");
            Words.Add("wake");
            Words.Add("conspiracy");

            Words.Spellcheck("sheeeeep");
            Words.Spellcheck("peepple");
            Words.Spellcheck("sheeple");
            Words.Spellcheck("inSIDE");
            Words.Spellcheck("jjoobbb");
            Words.Spellcheck("CUNsperrICY");
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
