using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spellcheck
{
    public class LetterNode
    {
        public char Letter;
        public List<LetterNode> Nodes;
        public bool End;
    }
}
