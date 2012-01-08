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
        public void ChangeCaseFromLowerToUpper()
        {

            Assert.AreEqual('I', unspell.ChangeCase('i'));
        }

        [Test]
        public void ChangeCaseFromUpperToLower()
        {
            Assert.AreEqual('i', unspell.ChangeCase('I'));
        }

        [Test]
        public void DontChangeCaseOfDifferentCulture() 
        {
            // DOS window can't handle all culture's letters
            // like Á specifically. It will become a regular A
            // and make the unspeller not realize the cultural
            // difference.
            Assert.AreEqual('Á', unspell.ChangeCase('Á'));
        }

        [Test]
        public void ChangeCharacter()
        {
            Assert.AreEqual("weke", unspell.ChangeCharacter("wake", 1, 'e'));
        }

        [Test]
        public void ChangeNoVowelWord()
        {
            var actual = unspell.ChangeVowels("blrg");

            Assert.AreEqual("blrg", actual);
        }

        [Test]
        public void ChangeAllVowels()
        {
            // Not a practical example, but gets the job done
            var actual = unspell.ChangeVowels("eiouya", true);

            Assert.AreEqual("aeiouy", actual);
        }

        [Test]
        public void ChangeCapitalVowel()
        {
            // Not a practical example, but gets the job done
            var actual = unspell.ChangeVowels("E", true);

            Assert.AreEqual("A", actual);
        }

        [Test]
        public void ChangeWord()
        {
            var actual = unspell.ChangeVowels("sheep", true);

            Assert.AreEqual("shaep", actual);
        }

        [Test]
        public void GetRepeatingCharData()
        {
            var actual = unspell.RepeatCharacters("job", true);

            Assert.AreEqual("joobbb", actual);
        }

        [Test]
        public void ChangeCapatalizeationOnWord()
        {
            var actual = unspell.ChangeCapatlizationOnWord("yUp", true);

            Assert.AreEqual("YuP", actual);
        }

        [Test]
        public void UnspellWord()
        {
            var actual = unspell.UnspellWord("yUp", true);

            Assert.AreEqual("AeePPP", actual);
        }
    }
}
