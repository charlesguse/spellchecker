using NUnit.Framework;
using spellcheckLibrary;

namespace spellCheckUnitTests
{
    [TestFixture]
    public class LetterTreePopulationTests
    {
        LetterTree Words;
        
        [SetUp]
        public void SetUp()
        {
            Words = new LetterTree();
        }

        [Test]
        public void PopulateNoWordInTree()
        {
            Assert.AreEqual(0, Words.Tree.Count);
        }

        [Test]
        public void PopulateSingleWordIsInTree()
        {
            Words.Add("hi");
            Assert.AreEqual(1, Words.Tree.Count);
            Assert.AreEqual(1, Words.Tree['h'].Nodes.Count);
            Assert.AreEqual(0, Words.Tree['h'].Nodes['i'].Nodes.Count);

            Assert.AreEqual('h', Words.Tree['h'].Letter);
            Assert.AreEqual('i', Words.Tree['h'].Nodes['i'].Letter);

            Assert.IsFalse(Words.Tree['h'].End);
            Assert.IsTrue(Words.Tree['h'].Nodes['i'].End);

            Assert.AreEqual("hi", Words.Tree['h'].Nodes['i'].GetWord());
        }

        [Test]
        public void PopulateNonOverlappingWordsInTree()
        {
            Words.Add("hi");
            Words.Add("a");

            Assert.AreEqual(2, Words.Tree.Count);
            Assert.AreEqual(1, Words.Tree['h'].Nodes.Count);
            Assert.AreEqual(0, Words.Tree['a'].Nodes.Count);

            Assert.AreEqual('h', Words.Tree['h'].Letter);
            Assert.AreEqual('i', Words.Tree['h'].Nodes['i'].Letter);

            Assert.AreEqual('a', Words.Tree['a'].Letter);

            Assert.IsFalse(Words.Tree['h'].End);
            Assert.IsTrue(Words.Tree['h'].Nodes['i'].End);

            Assert.IsTrue(Words.Tree['a'].End);
        }

        [Test]
        public void PopulateSameWordInTreeTwice()
        {
            Words.Add("a");
            Words.Add("a");

            Assert.AreEqual(1, Words.Tree.Count);
        }

        [Test]
        public void PopulateOverlappingWordsInTree()
        {
            Words.Add("hi");
            Words.Add("ha");

            Assert.AreEqual(1, Words.Tree.Count);
            Assert.AreEqual(2, Words.Tree['h'].Nodes.Count);

            Assert.AreEqual('h', Words.Tree['h'].Letter);
            Assert.AreEqual('i', Words.Tree['h'].Nodes['i'].Letter);

            Assert.AreEqual('a', Words.Tree['h'].Nodes['a'].Letter);

            Assert.IsFalse(Words.Tree['h'].End);
            Assert.IsTrue(Words.Tree['h'].Nodes['i'].End);

            Assert.IsTrue(Words.Tree['h'].Nodes['a'].End);
        }

        [Test]
        public void PopulateSmallerWordUsingSameCharactersAsBiggerWord()
        {
            Words.Add("high");
            Words.Add("hi");
            Assert.AreEqual(1, Words.Tree.Count);
            Assert.AreEqual(1, Words.Tree['h'].Nodes.Count);
            Assert.AreEqual(1, Words.Tree['h'].Nodes['i'].Nodes.Count);
            Assert.AreEqual(1, Words.Tree['h'].Nodes['i'].Nodes['g'].Nodes.Count);
            Assert.AreEqual(0, Words.Tree['h'].Nodes['i'].Nodes['g'].Nodes['h'].Nodes.Count);

            Assert.AreEqual('h', Words.Tree['h'].Letter);
            Assert.AreEqual('i', Words.Tree['h'].Nodes['i'].Letter);
            Assert.AreEqual('g', Words.Tree['h'].Nodes['i'].Nodes['g'].Letter);
            Assert.AreEqual('h', Words.Tree['h'].Nodes['i'].Nodes['g'].Nodes['h'].Letter);

            Assert.IsFalse(Words.Tree['h'].End);
            Assert.IsTrue(Words.Tree['h'].Nodes['i'].End);
            Assert.IsFalse(Words.Tree['h'].Nodes['i'].Nodes['g'].End);
            Assert.IsTrue(Words.Tree['h'].Nodes['i'].Nodes['g'].Nodes['h'].End);

            // The full word will be on the ending node
            //Assert.IsEmpty(Words.Tree['h'].Word);
            Assert.AreEqual("hi", Words.Tree['h'].Nodes['i'].GetWord());
            //Assert.IsEmpty(Words.Tree['h'].Nodes['i'].Nodes['g'].Word);
            Assert.AreEqual("high", Words.Tree['h'].Nodes['i'].Nodes['g'].Nodes['h'].GetWord());
        }
    }

    [TestFixture]
    public class LetterTreeSpellCheckTests
    {
        LetterTree Words;

        [SetUp]
        public void Foo()
        {
            Words = new LetterTree();
        }

        [Test]
        public void SpellcheckNoSuggestionWord()
        {
            var actual = Words.Spellcheck("ohaithar");

            Assert.AreEqual("NO SUGGESTION", actual);
        }

        [Test]
        public void SpellcheckProperlySpelled()
        {
            var expected = "hi";

            Words.Add(expected);
            Words.Add("noise");
            Words.Add("morenoise");
            Words.Add("even more noise");
            Words.Add("hihihihihi");

            var actual = Words.Spellcheck(expected);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SpellcheckProperlySpelledOneLetterWord()
        {
            var expected = "a";

            Words.Add(expected);
            Words.Add("noise");
            Words.Add("morenoise");
            Words.Add("even more noise");
            Words.Add("hihihihihi");

            var actual = Words.Spellcheck(expected);

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SpellcheckMismatchedCase()
        {
            Words.Add("inside");

            var actual = Words.Spellcheck("inSIDE");

            Assert.AreEqual("inside", actual);
        }

        [Test]
        public void SpellcheckIncorrectVowel()
        {
            Words.Add("wake");

            var actual = Words.Spellcheck("weke");

            Assert.AreEqual("wake", actual);
        }

        [Test]
        public void SpellcheckRepeatedLetters()
        {
            Words.Add("job");

            var actual = Words.Spellcheck("jjoobbb");

            Assert.AreEqual("job", actual);
        }

        [Test]
        public void SpellcheckExampleOnWebsite()
        {
            Words.Add("sheep");
            Words.Add("people");
            Words.Add("inside");
            Words.Add("job");
            Words.Add("wake");
            Words.Add("conspiracy");

            Assert.AreEqual("sheep", Words.Spellcheck("sheeeeep"));
            Assert.AreEqual("people", Words.Spellcheck("peepple"));
            Assert.AreEqual("NO SUGGESTION", Words.Spellcheck("sheeple"));
            Assert.AreEqual("inside", Words.Spellcheck("inSIDE"));
            Assert.AreEqual("job", Words.Spellcheck("jjoobbb"));
            Assert.AreEqual("conspiracy", Words.Spellcheck("CUNsperrICY"));
        }

        [Test]
        public void SpellcheckWordsThatNeedBacktrackingToFind()
        {
            Words.Add("wake");
            Words.Add("west");

            var actual = Words.Spellcheck("weke");

            Assert.AreEqual("wake", actual);
        }

        [Test]
        public void SpellcheckWordsThatHaveMultipleCasings()
        {
            // Actual example from my word list.
            // That is what made me this is not handled properly
            Words.Add("Wake");
            Words.Add("wake");

            var actual = Words.Spellcheck("Wake");
            Assert.AreEqual("Wake", actual);

            actual = Words.Spellcheck("wake");
            Assert.AreEqual("wake", actual);
        }
    }
}
