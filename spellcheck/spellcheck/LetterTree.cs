using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace spellcheck
{
    public struct TraversalData
    {
        public int Location;
        public LetterNode CurrentNode;
        public Dictionary<char,LetterNode> Nodes;
    }

    public class LetterTree
    {
        private const string NO_SUGGESTION_TEXT = "NO SUGGESTION";

        public Dictionary<char, LetterNode> Tree { get; private set; }

        public LetterTree()
        {
            Tree = new Dictionary<char,LetterNode>();
        }

        public TraversalData GetRoot()
        {
            TraversalData treeData = new TraversalData();
            treeData.Location = -1;
            treeData.CurrentNode = null;
            treeData.Nodes = Tree;

            return treeData;
        }

        public void Add(string word)
        {
            var traversal = Traverse(word, GetRoot());
            AddAtLocation(word, traversal);
        }

        private void AddAtLocation(string word, TraversalData traversal)
        {
            LetterNode node;

            if (traversal.Location < word.Length - 1)
            {
                node = new LetterNode();
                node.Parent = traversal.CurrentNode;
                node.Letter = word[++traversal.Location];
                traversal.Nodes.Add(node.Letter,node);

                if (traversal.Location + 1 < word.Length)
                {
                    traversal.CurrentNode = node;
                    traversal.Nodes = node.Nodes;
                    AddAtLocation(word, traversal);
                }
                else
                    node.End = true;
            }
            else
                traversal.CurrentNode.End = true;
        }

        // Travels as far as it can before it either comes to the wrong letter
        // or comes to the end of the tree
        private TraversalData Traverse(string word, TraversalData traversal)
        {
            TraversalData oldSpot;
            TraversalData newSpot = traversal;

            do
            {
                oldSpot = newSpot;
                newSpot = TraverseOneStep(word, oldSpot);

            } while (oldSpot.Location != newSpot.Location);

            return newSpot;
        }

        private TraversalData TraverseOneStep(string word, TraversalData traversal)
        {
            var currentNode = TraverseNode(word, traversal);

            if (currentNode != null)
            {
                traversal.CurrentNode = currentNode;
                traversal.Nodes = currentNode.Nodes;
                traversal.Location++;
            }

            return traversal;
        }


        private LetterNode TraverseNode(string word, TraversalData traversal)
        {
            if (word.Length > 0 && traversal.Location < word.Length - 1)
            {
                var potentialKey = word[traversal.Location + 1];

                // Check TraversalData.Nodes instead of TraversalData.CurrentNode
                // because there is not a single tree structure containing all of
                // the nodes. Instead it is a hash of nodes based on the first letter
                if (traversal.Nodes.ContainsKey(potentialKey))
                    return traversal.Nodes[potentialKey];
            }
            
            return null;
        }

        public string Spellcheck(string word)
        {
            return Spellcheck(word, GetRoot());
        }

        // This function is called recursively from within itself and from within
        // each letter changing case (including case change)
        // The order is as follows:
        // Go to the next letter in the word.
        // If the letter is good, go to the next letter.
        // Otherwise, change the casing and and try that.
        // If that doesn't work, check for repeating letters.
        // Finally, try changing the vowel if it is one.
        public string Spellcheck(string word, TraversalData traversal, 
                                 bool checkingVowel = false, bool checkingCase = false)
        {
            if (traversal.CurrentNode != null)
            {
                var currentWord = traversal.CurrentNode.GetWord();
                if (traversal.CurrentNode.End == true && currentWord == word)
                    return currentWord;
            }
            var nextLocation = TraverseOneStep(word, traversal);
            if (traversal.Location != nextLocation.Location)
            {
                var returnedWord = Spellcheck(word, nextLocation);

                if (returnedWord != NO_SUGGESTION_TEXT)
                    return returnedWord;
            }

            // If you already changed the case, don't try checking it again.
            if (!checkingCase && 0 <= traversal.Location + 1 && traversal.Location + 1 < word.Length)
            {
                var changedCasing = CheckForImpropperCasing(word, traversal);

                if (changedCasing != NO_SUGGESTION_TEXT)
                    return changedCasing;
            }

            if (0 <= traversal.Location && word.Length > 1 && traversal.Location + 1 < word.Length
                && word[traversal.Location] == word[traversal.Location + 1])
            {
                var processedWord = ProcessRepeatedLetter(word, traversal);

                if (processedWord != NO_SUGGESTION_TEXT)
                    return processedWord;
            }

            // If you are still checking vowels, so don't try checking them again.
            if (!checkingVowel && 0 <= traversal.Location + 1 
                && traversal.Location + 1 < word.Length
                && IsVowel(word[traversal.Location + 1]))
            {
                var changedVowel = CheckForIncorrectVowel(word, traversal);

                if (changedVowel != NO_SUGGESTION_TEXT)
                    return changedVowel;
            }

            return NO_SUGGESTION_TEXT;
        }

        private string CheckForImpropperCasing(string word, TraversalData traversal)
        {
            StringBuilder newWord = new StringBuilder(word);

            if (char.IsLower(word[traversal.Location + 1]))
                newWord[traversal.Location + 1] = char.ToUpper(newWord[traversal.Location + 1]);
            else
                newWord[traversal.Location + 1] = char.ToLower(newWord[traversal.Location + 1]);

            return Spellcheck(newWord.ToString(), traversal, true, checkingCase: true);
        }

        private string CheckForIncorrectVowel(string word, TraversalData traversal)
        {
            StringBuilder newWord = new StringBuilder(word);

            foreach (char vowel in GetVowel())
            {
                if (vowel != char.ToLower(word[traversal.Location+1]))
                {
                    newWord[traversal.Location+1] = vowel;
                    var spellcheckReturn = Spellcheck(newWord.ToString(), traversal, checkingVowel: true);

                    if (spellcheckReturn != NO_SUGGESTION_TEXT)
                        return spellcheckReturn;
                }
            }
            return NO_SUGGESTION_TEXT;
        }

        private string ProcessRepeatedLetter(string word, TraversalData traversal)
        {
            // Strip off the repeating letter and run it through spellcheck again
            StringBuilder newWord = new StringBuilder(word);

            if (traversal.Location >= 0
                   && newWord[traversal.Location] == newWord[traversal.Location + 1])
            {
                newWord = newWord.Remove(traversal.Location, 1);

                if (traversal.CurrentNode.End
                    && traversal.CurrentNode.GetWord() == newWord.ToString())
                    return traversal.CurrentNode.GetWord();

                // Try to backtrack one because if you delete on and it lands
                // on a vowel, you will need to be able switch it out in the
                // spellcheck function
                if (traversal.CurrentNode.Parent != null)
                {
                    traversal.CurrentNode = traversal.CurrentNode.Parent;
                    traversal.Nodes = traversal.CurrentNode.Nodes;
                    traversal.Location--;
                }
                var spellCheckedWord = Spellcheck(newWord.ToString(), traversal);

                if (spellCheckedWord != NO_SUGGESTION_TEXT)
                    return spellCheckedWord;

            }
            return NO_SUGGESTION_TEXT;
        }

        private bool IsVowel(char character)
        {
            foreach (char vowel in GetVowel())
            {
                if (char.ToLower(character) == vowel)
                    return true;
            }
            return false;
        }

        // I used an array in the unspeller because I needed vowels
        // in a different way there. I could have used an array here
        // but I thought it would be fun trying the yield keyword
        // since I haven't really found a good use for yield yet.
        private IEnumerable GetVowel()
        {
            yield return 'a';
            yield return 'e';
            yield return 'i';
            yield return 'o';
            yield return 'u';
            yield return 'y';
        }
    }
}
