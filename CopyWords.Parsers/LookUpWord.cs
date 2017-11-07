using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using CopyWords.Parsers.Models;
using System.Threading.Tasks;
using System.Text;
using System.Linq;

namespace CopyWords.Parsers
{
    public class LookUpWord
    {
        private readonly HttpClient _httpClient = new HttpClient();

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
            string ddoPageHtml = await DownloadPageAsync(ddoUrl, Encoding.UTF8);
            if (string.IsNullOrEmpty(ddoPageHtml))
            {
                return null;
            }

            DDOPageParser ddoPageParser = new DDOPageParser();
            ddoPageParser.LoadHtml(ddoPageHtml);

            WordModel wordModel = new WordModel();
            wordModel.Word = ddoPageParser.ParseWord();
            wordModel.Endings = ddoPageParser.ParseEndings();
            wordModel.Pronunciation = ddoPageParser.ParsePronunciation();
            wordModel.Sound = ddoPageParser.ParseSound();
            wordModel.Definitions = ddoPageParser.ParseDefinitions();
            wordModel.Examples = ddoPageParser.ParseExamples();

            // Download and parse a page from Slovar.dk
            string slovardkUrl = GetSlovardkUri(wordToLookUp);

            string slovardkPageHtml = await DownloadPageAsync(slovardkUrl, Encoding.GetEncoding(1251));
            SlovardkPageParser slovardkPageParser = new SlovardkPageParser();
            slovardkPageParser.LoadHtml(slovardkPageHtml);

            var translations = slovardkPageParser.ParseWord();
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

        private async Task<string> DownloadPageAsync(string url, Encoding encoding)
        {
            string content = null;

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                byte[] bytes = await response.Content.ReadAsByteArrayAsync();
                content = encoding.GetString(bytes, 0, bytes.Length - 1);
            }
            else
            {
                if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    throw new ServerErrorException("Server returned " + response.StatusCode);
                }
            }

            return content;
        }
    }
}
