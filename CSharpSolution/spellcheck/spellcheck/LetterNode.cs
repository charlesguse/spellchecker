using System.Collections.Generic;
using System.Text;

namespace spellcheck
{
    public class LetterNode
    {
        public char Letter;
        public LetterNode Parent;
        public Dictionary<char, LetterNode> Nodes;
        public bool End;

        public LetterNode()
        {
            Nodes = new Dictionary<char, LetterNode>();
        }

        public string GetWord()
        {
            StringBuilder word = new StringBuilder();
            var traverser = this;

            do
            {
                word.Insert(0, traverser.Letter);
                traverser = traverser.Parent;
            }
            while (traverser != null);

            return word.ToString();
        }
    }
}
