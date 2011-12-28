using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using spellcheckLibrary;
using System.IO;

namespace spellcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            LetterTree Words = new LetterTree();

            //var expected = "a";

            //Words.Add(expected);
            //Words.Add("noise");
            //Words.Add("morenoise");
            //Words.Add("even more noise");
            //Words.Add("hihihihihi");

            //var actual = Words.Spellcheck(expected);

            PopulateWords(Words, "words");

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

        static void PopulateWords(LetterTree Words, string filePath)
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
            }
            catch
            { }
        }
    }
}
