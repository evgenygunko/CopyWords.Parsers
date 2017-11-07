using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyWords.Parsers.Tests
{
    [TestClass]
    public class LookUpWordTests
    {
        [TestMethod]
        public void GetSlovardkUri_ReturnsUriForKigge()
        {
            string result = LookUpWord.GetSlovardkUri("kigge");
            Assert.AreEqual("http://www.slovar.dk/tdansk/kigge/?", result);
        }

        [TestMethod]
        public void GetSlovardkUri_ReturnsUriForAfgørelse()
        {
            string result = LookUpWord.GetSlovardkUri("afgørelse");
            Assert.AreEqual("http://www.slovar.dk/tdansk/afg'oerelse/?", result);
        }

        [TestMethod]
        public void GetSlovardkUri_ReturnsUriForÅl()
        {
            string result = LookUpWord.GetSlovardkUri("ål");
            Assert.AreEqual("http://www.slovar.dk/tdansk/'aal/?", result);
        }

        [TestMethod]
        public void GetSlovardkUri_ReturnsUriForKæmpe()
        {
            string result = LookUpWord.GetSlovardkUri("Kæmpe");
            Assert.AreEqual("http://www.slovar.dk/tdansk/k'aempe/?", result);
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsFalse_ForUrl()
        {
            var lookupWord = new LookUpWord();
            (bool isValid, string errorMessage) = lookupWord.CheckThatWordIsValid(@"http://ordnet.dk/ddo/ordbog");

            Assert.IsFalse(isValid, errorMessage);
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsFalse_ForQuote()
        {
            var lookupWord = new LookUpWord();
            (bool isValid, string errorMessage) = lookupWord.CheckThatWordIsValid(@"ordbo'g");

            Assert.IsFalse(isValid, errorMessage);
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsTrue_ForWord()
        {
            var lookupWord = new LookUpWord();
            (bool isValid, string errorMessage) = lookupWord.CheckThatWordIsValid(@"refusionsopgørelse");

            Assert.IsTrue(isValid, errorMessage);
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsTrue_ForTwoWord()
        {
            var lookupWord = new LookUpWord();
            (bool isValid, string errorMessage) = lookupWord.CheckThatWordIsValid(@"rindende vand");

            Assert.IsTrue(isValid, errorMessage);
        }
    }
}
