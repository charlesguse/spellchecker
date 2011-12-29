using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace unspeller
{
    public struct CharacterToChange
    {
        public int LetterIndex;
        public int NewCharacter;
    }

    public class unspell
    {
        public static char[] Vowels = { 'a', 'e', 'i', 'o', 'u', 'y' };

        static public string RepeatLetter(string word, int letterIndex, int timesToRepeat)
        {
            StringBuilder newWord = new StringBuilder(word);
            char charToRepeat = newWord[letterIndex];
            String repeatedChars = new String(charToRepeat,timesToRepeat);
            newWord.Insert(letterIndex, repeatedChars);

            return newWord.ToString();
        }

        public static string ChangeCase(string word, int letterIndex)
        {
            StringBuilder newWord = new StringBuilder(word);

            if (char.IsLower(newWord[letterIndex]))
                newWord[letterIndex] = char.ToUpper(newWord[letterIndex]);
            else
                newWord[letterIndex] = char.ToLower(newWord[letterIndex]);


            return newWord.ToString();
        }

        public static string ChangeCharacter(string word, int letterIndex, char newCharacter)
        {
            StringBuilder newWord = new StringBuilder(word);
            newWord[letterIndex] = newCharacter;
            return newWord.ToString();
        }

        //public static CharacterToChange FindVowelToChange(string word)
        //{
        //    CharacterToChange vowel;
        //    // If the loop
        //    vowel.LetterIndex = 0;
        //    vowel.NewCharacter = word[0];

        //    for (int i = 0; i < word.Length; i++)
        //    {

        //    }
        //}

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
