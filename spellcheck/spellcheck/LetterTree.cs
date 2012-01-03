using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace spellcheck
{
    public struct TraversalData
    {
        public int Depth;
        public LetterNode CurrentNode;
        public Dictionary<char,LetterNode> Nodes;
    }

    public class LetterTree
    {
        private const string NO_SUGGESTION_TEXT = "NO SUGGESTION";

        public Dictionary<char, LetterNode> Tree { get; private set; }
        public Dictionary<string, HashSet<int>> BadWord { get; private set; }

        public LetterTree()
        {
            Tree = new Dictionary<char,LetterNode>();
            BadWord = new Dictionary<string, HashSet<int>>();
        }

        public TraversalData GetRoot()
        {
            TraversalData treeData = new TraversalData();
            treeData.Depth = -1;
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

            if (traversal.Depth < word.Length - 1)
            {
                node = new LetterNode();
                node.Parent = traversal.CurrentNode;
                node.Letter = word[++traversal.Depth];
                traversal.Nodes.Add(node.Letter,node);

                if (traversal.Depth + 1 < word.Length)
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

            } while (oldSpot.Depth != newSpot.Depth);

            return newSpot;
        }

        private TraversalData TraverseOneStep(string word, TraversalData traversal)
        {
            var currentNode = TraverseNode(word, traversal);

            if (currentNode != null)
            {
                traversal.CurrentNode = currentNode;
                traversal.Nodes = currentNode.Nodes;
                traversal.Depth++;
            }

            return traversal;
        }


        private LetterNode TraverseNode(string word, TraversalData traversal)
        {
            if (word.Length > 0 && traversal.Depth < word.Length - 1)
            {
                var potentialKey = word[traversal.Depth + 1];

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
            var root = GetRoot();
            var newWord = Spellcheck(word, root);
            BadWord.Clear();
            return newWord;
        }

        // This function is called recursively from within itself and from within
        // each letter changing case (including case change)
        // The order is as follows:
        //   Go to the next letter in the word.
        //   If you can traverse to it, recursively call spellcheck at the next node in the tree
        //   Then if that returns no suggestion, check repeating letters, vowels, and improper casing.
        //   If any function returns a word that fits the requirements, stop immediately and return it
        //   If all options are exhausted, return no suggestion.
        public string Spellcheck(string word, TraversalData traversal,
                                 bool changingVowel = false, bool changingCase = false)
        {
            // If a word has been checked from this depth, don't check it again.
            if (!(BadWord.ContainsKey(word) && BadWord[word].Contains(traversal.Depth)))
            {
                if (traversal.CurrentNode != null)
                {
                    var currentWord = traversal.CurrentNode.GetWord();

                    if (traversal.CurrentNode.End == true && currentWord == word)
                        return currentWord;
                }

                var nextLocation = TraverseOneStep(word, traversal);
                if (traversal.Depth != nextLocation.Depth)
                {
                    var returnedWord = Spellcheck(word, nextLocation);

                    if (returnedWord != NO_SUGGESTION_TEXT)
                        return returnedWord;
                }

                // If a vowel was changed to cause the repeating character,
                // don't remove the repeated character.
                if ((traversal.Depth + 1 < word.Length && !IsVowel(word[traversal.Depth + 1]))
                    || !changingVowel)
                {
                    var processedWord = ProcessRepeatedLetter(word, traversal);

                    if (processedWord != NO_SUGGESTION_TEXT)
                        return processedWord;
                }

                // If you are still checking vowels, so don't try checking them again.
                if (!changingVowel)
                {
                    var changedVowel = CheckForIncorrectVowel(word, traversal, changingCase);

                    if (changedVowel != NO_SUGGESTION_TEXT)
                        return changedVowel;
                }

                // If you already changed the case, don't try checking it again.
                if (!changingCase)
                {
                    var changedCasing = CheckForImproperCasing(word, traversal, changingVowel);

                    if (changedCasing != NO_SUGGESTION_TEXT)
                        return changedCasing;
                }

                if (!BadWord.ContainsKey(word))
                    BadWord.Add(word, new HashSet<int>());
                BadWord[word].Add(traversal.Depth);
            }
            return NO_SUGGESTION_TEXT;
        }

        private string CheckForImproperCasing(string word, TraversalData traversal,
                                               bool changingVowel)
        {
            if (0 <= traversal.Depth + 1 && traversal.Depth + 1 < word.Length)
            {
                StringBuilder newWord = new StringBuilder(word);

                if (char.IsLower(word[traversal.Depth + 1]))
                    newWord[traversal.Depth + 1] = char.ToUpper(newWord[traversal.Depth + 1]);
                else
                    newWord[traversal.Depth + 1] = char.ToLower(newWord[traversal.Depth + 1]);

                return Spellcheck(newWord.ToString(), traversal, changingVowel, true);
            }
            return NO_SUGGESTION_TEXT;
        }

        private string CheckForIncorrectVowel(string word, TraversalData traversal,
                                              bool changingCase)
        {
            if (0 <= traversal.Depth + 1
                && traversal.Depth + 1 < word.Length
                && IsVowel(word[traversal.Depth + 1]))
            {
                StringBuilder newWord = new StringBuilder(word);

                foreach (char vowel in GetVowel())
                {
                    if (vowel != char.ToLower(word[traversal.Depth + 1]))
                    {
                        if (char.IsUpper(word[traversal.Depth + 1]))
                            newWord[traversal.Depth + 1] = char.ToUpper(vowel);
                        else
                            newWord[traversal.Depth + 1] = vowel;

                        var spellcheckReturn = Spellcheck(newWord.ToString(), traversal, true, changingCase);

                        if (spellcheckReturn != NO_SUGGESTION_TEXT)
                            return spellcheckReturn;
                    }
                }
            }
            return NO_SUGGESTION_TEXT;
        }

        private string ProcessRepeatedLetter(string word, TraversalData traversal)
        {
            // Strip off the repeating letter and run it through spellcheck again
            StringBuilder newWordBuilder = new StringBuilder(word);

            while (traversal.Depth + 2 < newWordBuilder.Length
                && newWordBuilder[traversal.Depth + 1] == newWordBuilder[traversal.Depth + 2])
            {
                newWordBuilder = newWordBuilder.Remove(traversal.Depth + 1, 1);

                string newWord = newWordBuilder.ToString();

                if (traversal.CurrentNode != null)
                {
                    var newTraversal = traversal;
                    newTraversal = TraverseOneStep(newWord, newTraversal);
                    newTraversal = TraverseOneStep(newWord, newTraversal);

                    if (traversal.Depth + 2 == newTraversal.Depth && newTraversal.CurrentNode.End
                        && newTraversal.CurrentNode.GetWord() == newWord)
                        return newTraversal.CurrentNode.GetWord();
                }
                var spellcheckReturn = Spellcheck(newWord, traversal);

                if (spellcheckReturn != NO_SUGGESTION_TEXT)
                    return spellcheckReturn;
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
