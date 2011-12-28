using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spellcheckLibrary
{
    public class LetterNode
    {
        public char Letter;
        public LetterNode Parent;
        public List<LetterNode> Nodes;
        public bool End;
        public string Word;

        public LetterNode()
        {
            Word = string.Empty;
            Nodes = new List<LetterNode>();
        }
    }
}
