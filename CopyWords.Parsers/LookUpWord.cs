using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CopyWords.Parsers.Models;
using CopyWords.Parsers.Services;

namespace CopyWords.Parsers
{
    public class LookUpWord
    {
        private readonly IDDOPageParser _ddoPageParser;
        private readonly ISlovardkPageParser _slovardkPageParser;
        private readonly IFileDownloader _fileDownloader;

        public LookUpWord(IDDOPageParser ddoPageParser, ISlovardkPageParser slovardkPageParser, IFileDownloader fileDownloader)
        {
            _ddoPageParser = ddoPageParser ?? throw new ArgumentNullException(nameof(ddoPageParser));
            _slovardkPageParser = slovardkPageParser ?? throw new ArgumentNullException(nameof(slovardkPageParser));
            _fileDownloader = fileDownloader ?? throw new ArgumentNullException(nameof(fileDownloader));
        }

        public (bool isValid, string errorMessage) CheckThatWordIsValid(string lookUp)
        {
            Regex regex = new Regex(@"^[\w ]+$");
            bool isValid = regex.IsMatch(lookUp);

            return (isValid, isValid ? null : "Search can only contain alphanumeric characters and spaces.");
        }

        public async Task<WordModel> LookUpWordAsync(string wordToLookUp)
        {
            if (string.IsNullOrEmpty(wordToLookUp))
            {
                throw new ArgumentException("LookUp text can't be null or empty.");
            }

            (bool isValid, string errorMessage) = CheckThatWordIsValid(wordToLookUp);
            if (!isValid)
            {
                throw new ArgumentException(errorMessage, nameof(wordToLookUp));
            }

            string ddoUrl = $"http://ordnet.dk/ddo/ordbog?query={wordToLookUp}&search=S%C3%B8g";

            // Download and parse a page from DDO
            string ddoPageHtml = await _fileDownloader.DownloadPageAsync(ddoUrl, Encoding.UTF8);
            if (string.IsNullOrEmpty(ddoPageHtml))
            {
                return null;
            }

            _ddoPageParser.LoadHtml(ddoPageHtml);

            WordModel wordModel = new WordModel();
            wordModel.Variants = _ddoPageParser.GetWordsCount();
            wordModel.Word = _ddoPageParser.ParseWord();
            wordModel.Endings = _ddoPageParser.ParseEndings();
            wordModel.Pronunciation = _ddoPageParser.ParsePronunciation();
            wordModel.Sound = _ddoPageParser.ParseSound();
            wordModel.Definitions = _ddoPageParser.ParseDefinitions();
            wordModel.Examples = _ddoPageParser.ParseExamples();

            // Download and parse a page from Slovar.dk
            string slovardkUrl = GetSlovardkUri(wordToLookUp);

            string slovardkPageHtml = await _fileDownloader.DownloadPageAsync(slovardkUrl, Encoding.GetEncoding(1251));
            _slovardkPageParser.LoadHtml(slovardkPageHtml);

            var translations = _slovardkPageParser.ParseWord();
            wordModel.Translations = translations;

            return wordModel;
        }

        internal static string GetSlovardkUri(string wordToLookUp)
        {
            wordToLookUp = wordToLookUp.ToLower()
                .Replace("å", "'aa")
                .Replace("æ", "'ae")
                .Replace("ø", "'oe")
                .Replace(" ", "-");

            return $"http://www.slovar.dk/tdansk/{wordToLookUp}/?";
        }
    }
}
