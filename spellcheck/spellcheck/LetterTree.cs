using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spellcheck
{
    public class LetterTree
    {
        public List<LetterNode> Tree { get; private set; }

        public LetterTree()
        {
            Tree = new List<LetterNode>();
        }
    }
}
