using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using unspeller;

namespace unspellerUnitTests
{
    [TestFixture]
    public class unspellerTests
    {
        [Test]
        public void RepeatNoLetterTest()
        {
            Assert.AreEqual("sheep", unspell.RepeatLetter("sheep", 0, 0));
        }

        [Test]
        public void RepeatLetterTest()
        {
            Assert.AreEqual("sheeeeep", unspell.RepeatLetter("sheep", 2, 3));
        }

        [Test]
        public void RepeatMultipleLettersTest()
        {
            var actual = unspell.RepeatLetter("job", 0, 1);    // jjob
            actual = unspell.RepeatLetter(actual, 2, 1);       // jjoob
            actual = unspell.RepeatLetter(actual, 4, 2);       // jjoobbb
            Assert.AreEqual("jjoobbb", actual);
        }

        [Test]
        public void ChangeCaseFromLowerToUpperTest()
        {
            Assert.AreEqual("Inside", unspell.ChangeCase("inside", 0));
        }

        [Test]
        public void ChangeCaseFromUpperToLowerTest()
        {
            Assert.AreEqual("insIDE", unspell.ChangeCase("inSIDE", 2));
        }

        [Test]
        public void ChangeCaseOfDifferentCultureTest() 
        {
            // They are in my dictionary, so I figured
            // I would see how changing the case worked with
            // non-english characters.
            Assert.AreEqual("å", unspell.ChangeCase("Å", 0));
        }

        [Test]
        public void ChangeCharacterTest()
        {
            Assert.AreEqual("weke", unspell.ChangeCharacter("wake", 1, 'e'));
        }
    }
}
