using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CopyWords.Parsers;
using CopyWords.Parsers.Models;
using CopyWords.Parsers.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

#pragma warning disable RCS1046 // Add suffix 'Async' to asynchronous method name

namespace CopyWords.Parsers.Tests
{
    [TestClass]
    public class LookUpWordTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

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
            var lookupWord = CreateLookUpWord();
            (bool isValid, string errorMessage) = lookupWord.CheckThatWordIsValid(@"http://ordnet.dk/ddo/ordbog");

            Assert.IsFalse(isValid, errorMessage);
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsFalse_ForQuote()
        {
            var lookupWord = CreateLookUpWord();
            (bool isValid, string errorMessage) = lookupWord.CheckThatWordIsValid(@"ordbo'g");

            Assert.IsFalse(isValid, errorMessage);
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsTrue_ForWord()
        {
            var lookupWord = CreateLookUpWord();
            (bool isValid, string errorMessage) = lookupWord.CheckThatWordIsValid(@"refusionsopgørelse");

            Assert.IsTrue(isValid, errorMessage);
        }

        [TestMethod]
        public void CheckThatWordIsValid_ReturnsTrue_ForTwoWord()
        {
            var lookupWord = CreateLookUpWord();
            (bool isValid, string errorMessage) = lookupWord.CheckThatWordIsValid(@"rindende vand");

            Assert.IsTrue(isValid, errorMessage);
        }

        [TestMethod]
        public async Task LookUpWordAsync_ReturnsOneVariant_WhenDDOWordHasOneMeaning()
        {
            const int wordsCount = 1;

            var ddoPageParserStub = new Mock<IDDOPageParser>();
            ddoPageParserStub.Setup(x => x.GetWordsCount()).Returns(wordsCount);

            var lookupWord = CreateLookUpWord(ddoPageParserStub.Object);
            WordModel wordModel = await lookupWord.LookUpWordAsync("kold");

            Assert.AreEqual(wordsCount, wordModel.Variants);
        }

        [TestMethod]
        public async Task LookUpWordAsync_ReturnsTwoVariants_WhenDDOWordHasTwoMeanings()
        {
            const int wordsCount = 2;

            var ddoPageParserStub = new Mock<IDDOPageParser>();
            ddoPageParserStub.Setup(x => x.GetWordsCount()).Returns(wordsCount);

            var lookupWord = CreateLookUpWord(ddoPageParserStub.Object);
            WordModel wordModel = await lookupWord.LookUpWordAsync("kold");

            Assert.AreEqual(wordsCount, wordModel.Variants);
        }

        private static LookUpWord CreateLookUpWord()
        {
            return CreateLookUpWord(new Mock<IDDOPageParser>().Object);
        }

        private static LookUpWord CreateLookUpWord(IDDOPageParser ddoPageParser)
        {
            var fileDownloaderStub = new Mock<IFileDownloader>();
            fileDownloaderStub.Setup(x => x.DownloadPageAsync(It.IsAny<string>(), It.IsAny<Encoding>())).ReturnsAsync("Some page here");

            var slovardkPageParserStub = new Mock<ISlovardkPageParser>();

            var lookupWord = new LookUpWord(ddoPageParser, slovardkPageParserStub.Object, fileDownloaderStub.Object);

            return lookupWord;
        }
    }
}
