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
        public void ChangeCaseFromLowerToUpperTest()
        {

            Assert.AreEqual('I', unspell.ChangeCase('i'));
        }

        [Test]
        public void ChangeCaseFromUpperToLowerTest()
        {
            Assert.AreEqual('i', unspell.ChangeCase('I'));
        }

        [Test]
        public void ChangeCaseOfDifferentCultureTest() 
        {
            // They are in my dictionary, so I figured
            // I would see how changing the case worked with
            // non-english characters.
            Assert.AreEqual('å', unspell.ChangeCase('Å'));
        }

        [Test]
        public void ChangeCharacterTest()
        {
            Assert.AreEqual("weke", unspell.ChangeCharacter("wake", 1, 'e'));
        }

        [Test]
        public void ChangeNoVowelWordTest()
        {
            var actual = unspell.ChangeVowels("blrg");

            Assert.AreEqual("blrg", actual);
        }

        [Test]
        public void ChangeAllVowelsTest()
        {
            // Not a practical example, but gets the job done
            var actual = unspell.ChangeVowels("eiouya", true);

            Assert.AreEqual("aeiouy", actual);
        }

        [Test]
        public void ChangeCapitalVowelTest()
        {
            // Not a practical example, but gets the job done
            var actual = unspell.ChangeVowels("E", true);

            Assert.AreEqual("A", actual);
        }

        [Test]
        public void ChangeWordTest()
        {
            var actual = unspell.ChangeVowels("sheep", true);

            Assert.AreEqual("shaep", actual);
        }

        [Test]
        public void GetRepeatingCharDataTest()
        {
            var actual = unspell.RepeatCharacters("job", true);

            Assert.AreEqual("joobbb", actual);
        }

        [Test]
        public void ChangeCapatalizeationOnWordTest()
        {
            var actual = unspell.ChangeCapatlizationOnWord("yUp", true);

            Assert.AreEqual("YuP", actual);
        }

        [Test]
        public void UnspellWordTest()
        {
            var actual = unspell.UnspellWord("yUp", true);

            Assert.AreEqual("AeePPP", actual);
        }
    }
}
