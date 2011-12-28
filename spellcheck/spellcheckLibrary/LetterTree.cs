using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace spellcheckLibrary
{
    public struct TraversalData
    {
        public int Location;
        public LetterNode CurrentNode;
        public List<LetterNode> Nodes;
    }

    public class LetterTree
    {
        private const string NO_SUGGESTION_TEXT = "NO SUGGESTION";

        public List<LetterNode> Tree { get; private set; }

        public LetterTree()
        {
            Tree = new List<LetterNode>();
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
                traversal.Nodes.Add(node);

                if (traversal.Location + 1 < word.Length)
                {
                    //traversal.Location++;
                    traversal.CurrentNode = node;
                    traversal.Nodes = node.Nodes;
                    AddAtLocation(word, traversal);
                }
                else
                {
                    node.End = true;
                    node.Word = word;
                }
            }
            else
            {
                traversal.CurrentNode.Word = word;
                traversal.CurrentNode.End = true;
            }
        }

        // Travels as far as it can before it either comes to the wrong letter
        // or comes to the end of the tree
        private TraversalData Traverse(string word, TraversalData traversal)
        {
            var nextSpot = TraverseOneStep(word, traversal);

            while (traversal.Location != nextSpot.Location)
            {
                traversal = nextSpot;
                nextSpot = TraverseOneStep(word, traversal);
            }

            return traversal;
        }

        private TraversalData TraverseOneStep(string word, TraversalData traversal)
        {
            var currentNode = TraverseNode(word, traversal);

            if (currentNode != null)
            {
                traversal.CurrentNode = currentNode;
                traversal.Nodes = currentNode.Nodes;
                traversal.Location++;
                //currentNode = TraverseNode(word, traversal);
            }

            return traversal;
        }

        private LetterNode TraverseNode(string word, TraversalData traversal)
        {
            foreach (var node in traversal.Nodes)
            {
                if (traversal.Location < word.Length - 1
                    && char.ToLower(node.Letter) == char.ToLower(word[traversal.Location+1]))
                    return node;
            }

            return null;
        }

        public string Spellcheck(string word)
        {
            return Spellcheck(word, GetRoot());
        }

        public string Spellcheck(string word, TraversalData traversal, bool checkingVowel = false)
        {
            // If the exit node is not null, it stopped somewhere before the end
            // Check why it stopped before the end.
            if (traversal.CurrentNode != null && traversal.CurrentNode.End == true
                && traversal.CurrentNode.Word.ToLower() == word.ToLower())
                // If the words are the same in lower form, return the dictionary form
                // to handle the case rule
                return traversal.CurrentNode.Word;

            var nextLocation = TraverseOneStep(word, traversal);
            if (traversal.Location != nextLocation.Location)
            {
                var returnedWord = Spellcheck(word, nextLocation);

                if (returnedWord != NO_SUGGESTION_TEXT)
                    return returnedWord;
            }


            if (0 <= traversal.Location && traversal.Location + 1 < word.Length && word.Length > 1
                && word[traversal.Location] == word[traversal.Location + 1])
            {
                var correctedVowel = ProcessRepeatedLetter(word, traversal);

                if (correctedVowel != NO_SUGGESTION_TEXT)
                    return correctedVowel;
            }

            // If you are still checking vowels, so don't try checking them again.
            if (!checkingVowel && 0 <= traversal.Location + 1 && traversal.Location + 1 < word.Length && IsVowel(word[traversal.Location + 1]))
            {
                var correctedVowel = CheckForIncorrectVowel(word, traversal);

                if (correctedVowel != NO_SUGGESTION_TEXT)
                    return correctedVowel;
            }

            return NO_SUGGESTION_TEXT;
        }

        private string CheckForIncorrectVowel(string word, TraversalData traversal)
        {
            StringBuilder newWord = new StringBuilder(word);

            foreach (char vowel in GetVowel())
            {
                if (vowel != char.ToLower(word[traversal.Location+1]))
                {
                    newWord[traversal.Location+1] = vowel;
                    var nextLocation = TraverseOneStep(newWord.ToString(), traversal);
                    var spellcheckReturn = Spellcheck(newWord.ToString(), nextLocation, true);

                    if (spellcheckReturn != NO_SUGGESTION_TEXT)
                        return spellcheckReturn;
                }
            }
            return NO_SUGGESTION_TEXT;
        }

        private TraversalData ChangeToNewNodeOnSameLevel(char character, TraversalData traversal)
        {
            if (traversal.CurrentNode.Parent != null)
            {
                var parent = traversal.CurrentNode.Parent;

                foreach (var node in parent.Nodes)
                {
                    if (char.ToLower(character) == char.ToLower(node.Letter))
                    {
                        traversal.CurrentNode = node;
                        traversal.Nodes = node.Nodes;
                    }
                }
            }
            return traversal;
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
                    && traversal.CurrentNode.Word.ToLower() == newWord.ToString().ToLower())
                    return traversal.CurrentNode.Word;

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
            return char.ToLower(character) == 'a'
                || char.ToLower(character) == 'e'
                || char.ToLower(character) == 'i'
                || char.ToLower(character) == 'o'
                || char.ToLower(character) == 'u'
/* sometimes */ || char.ToLower(character) == 'y';
        }

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
