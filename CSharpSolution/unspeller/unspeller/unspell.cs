using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace unspeller
{
    public struct CharacterChangeData
    {
        public int LetterIndex;
        public char NewCharacter;
    }

    public struct CharacterRepeatData
    {
        public int LetterIndex;
        public int TimesToRepeat;
    }

    public class unspell
    {
        static Random rand = new Random();
        public static char[] Vowels = { 'a', 'e', 'i', 'o', 'u', 'y' };

        public static int MAX_TIMES_TO_REPEAT_CHAR = 2;

        public static char ChangeCase(char letter)
        {
            if (('a' <= letter && letter <= 'z') || ('A' <= letter && letter <= 'Z'))
            {
                if (char.IsLower(letter))
                    letter = char.ToUpper(letter);
                else
                    letter = char.ToLower(letter);
            }
            return letter;

        }

        public static string ChangeCharacter(string word, int letterIndex, char newCharacter)
        {
            StringBuilder newWord = new StringBuilder(word);
            newWord[letterIndex] = newCharacter;
            return newWord.ToString();
        }

        public static string ChangeVowels(string word, bool testMode = false)
        {
            StringBuilder newWord = new StringBuilder(word);
            int vowelToUse = -1;
            bool changeVowel = false;

            for (int i = 0; i < word.Length; i++)
            {
                if (IsVowel(word[i]))
                {
                    if (testMode)
                    {
                        vowelToUse += 1;
                        vowelToUse %= Vowels.Length;
                        changeVowel = true;
                    }
                    else
                    {
                        // Make it so it doesn't always change vowels
                        if (rand.Next(2) == 0)
                            changeVowel = false;
                        else
                        {
                            vowelToUse = rand.Next(Vowels.Length);
                            changeVowel = true;
                        }
                    }

                    if (changeVowel)
                    {
                        if (Char.IsUpper(newWord[i]))
                            newWord[i] = Char.ToUpper(Vowels[vowelToUse]);
                        else
                            newWord[i] = Vowels[vowelToUse];
                    }
                }
            }
            return newWord.ToString();
        }

        public static string RepeatCharacters(string word, bool testMode = false)
        {
            StringBuilder newWord = new StringBuilder(word);
            int timesToRepeat = word.Length;
            char charToRepeat;

            for (int i = word.Length - 1; i >= 0; i--)
            {
                if (testMode)
                    timesToRepeat -= 1;
                else
                    timesToRepeat = rand.Next(MAX_TIMES_TO_REPEAT_CHAR);
                
                charToRepeat = newWord[i];
                String repeatedChars = new String(charToRepeat, timesToRepeat);
                newWord.Insert(i, repeatedChars);
            }
            return newWord.ToString();
        }

        public static string ChangeCapatlizationOnWord(string word, bool testMode = false)
        {
            StringBuilder newWord = new StringBuilder(word);
            bool changeCase;

            for (int i = 0; i < newWord.Length; i++)
            {
                if (testMode)
                    changeCase = true;
                else
                {
                    if (rand.Next(2) == 0)
                        changeCase = false;
                    else
                        changeCase = true;
                }
                if (changeCase)
                    newWord[i] = ChangeCase(newWord[i]);
            }
            return newWord.ToString();
        }

        public static string UnspellWord(string word, bool testMode = false)
        {
            word = ChangeVowels(word, testMode);
            word = RepeatCharacters(word, testMode);
            word = ChangeCapatlizationOnWord(word, testMode);
            return word;
        }

        // The vowel related stuff is close to identical in both applications.
        // Normally I wouldn't copy and paste code from one spot to another.
        // However, since these are seperate applications, I didn't want to
        // have them linked together and I didn't want to create a library
        // of one to two functions.
        private static bool IsVowel(char character)
        {
            foreach (char vowel in Vowels)
            {
                if (char.ToLower(character) == vowel)
                    return true;
            }
            return false;
        }
    }
}
