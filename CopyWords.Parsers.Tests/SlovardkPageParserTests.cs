using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyWords.Parsers.Tests
{
    [TestClass]
    public class SlovardkPageParserTests
    {
        private static string _path;
        private static Encoding _encoding1251;

        [ClassInitialize]
        public static void ClassInitialze(TestContext context)
        {
            _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _encoding1251 = CodePagesEncodingProvider.Instance.GetEncoding(1251);
        }

        [TestMethod]
        public void LoadStream_ReadsInCorrectEncoding()
        {
            string fileContent = Helpers.GetSimpleHTMLPage(GetTestFilePath("AfgørelsePage.html"), _encoding1251);

            Assert.IsTrue(fileContent.Contains("решение, улаживание"));
        }

        [TestMethod]
        public void ParseWord_ReturnsSingleTranslation_ForAfgørelsePage()
        {
            string content = Helpers.GetSimpleHTMLPage(GetTestFilePath("AfgørelsePage.html"), _encoding1251);

            SlovardkPageParser parser = new SlovardkPageParser();
            parser.LoadHtml(content);

            var translations = parser.ParseWord();
            Assert.AreEqual(1, translations.Count);
            Assert.AreEqual("afgørelse", translations[0].DanishWord);
            Assert.AreEqual("решение, улаживание", translations[0].Translation);
        }

        [TestMethod]
        public void ParseWord_ReturnsMultipleTranslations_ForKæmpePage()
        {
            string filePath = GetTestFilePath("KæmpePage.html");
            string content = Helpers.GetSimpleHTMLPage(filePath, _encoding1251);

            SlovardkPageParser parser = new SlovardkPageParser();
            parser.LoadHtml(content);

            var translations = parser.ParseWord();

            Assert.AreEqual(3, translations.Count);

            Assert.AreEqual("kæmpe", translations[0].DanishWord);
            Assert.AreEqual(
                $"1) богатырь, великан{System.Environment.NewLine}2) бороться, биться, сражаться",
                translations[0].Translation);

            Assert.AreEqual("kæmpemæssig", translations[1].DanishWord);
            Assert.AreEqual("гигантский", translations[1].Translation);

            Assert.AreEqual("kæmpestor", translations[2].DanishWord);
            Assert.AreEqual("огромный", translations[2].Translation);
        }

        private string GetTestFilePath(string fileName)
        {
            return Path.Combine(_path, "TestPages", "slovardk", fileName);
        }
    }
}
