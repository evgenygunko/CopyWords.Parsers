﻿using System;
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

        public LookUpWord()
            : this (new DDOPageParser(), new SlovardkPageParser(), new FileDownloader())
        {
        }

        public LookUpWord(IDDOPageParser ddoPageParser, ISlovardkPageParser slovardkPageParser, IFileDownloader fileDownloader)
        {
            _ddoPageParser = ddoPageParser ?? throw new ArgumentNullException(nameof(ddoPageParser));
            _slovardkPageParser = slovardkPageParser ?? throw new ArgumentNullException(nameof(slovardkPageParser));
            _fileDownloader = fileDownloader ?? throw new ArgumentNullException(nameof(fileDownloader));
        }

#pragma warning disable CA1822 // Mark members as static
        public (bool isValid, string errorMessage) CheckThatWordIsValid(string lookUp)
#pragma warning restore CA1822 // Mark members as static
        {
            Regex regex = new Regex(@"^[\w ]+$");
            bool isValid = regex.IsMatch(lookUp);

            return (isValid, isValid ? null : "Search can only contain alphanumeric characters and spaces.");
        }

        public async Task<WordModel> LookUpWordAsync(string wordToLookUp)
        {
            if (string.IsNullOrEmpty(wordToLookUp))
            {
                throw new ArgumentException("LookUp text cannot be null or empty.");
            }

            (bool isValid, string errorMessage) = CheckThatWordIsValid(wordToLookUp);
            if (!isValid)
            {
                throw new ArgumentException(errorMessage, nameof(wordToLookUp));
            }

            string url = $"http://ordnet.dk/ddo/ordbog?query={wordToLookUp}&search=S%C3%B8g";

            WordModel wordModel = await DownloadPageAndParseWordAsync(url);
            return wordModel;
        }

        public async Task<WordModel> GetWordVariationAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url cannot be null or empty.");
            }

            WordModel wordModel = await DownloadPageAndParseWordAsync(url);
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

        private async Task<WordModel> DownloadPageAndParseWordAsync(string url)
        {
            // Download and parse a page from DDO
            string ddoPageHtml = await _fileDownloader.DownloadPageAsync(url, Encoding.UTF8);
            if (string.IsNullOrEmpty(ddoPageHtml))
            {
                return null;
            }

            _ddoPageParser.LoadHtml(ddoPageHtml);

            WordModel wordModel = new WordModel();
            wordModel.VariationUrls = _ddoPageParser.ParseVariationUrls();
            wordModel.Word = _ddoPageParser.ParseWord();
            wordModel.Endings = _ddoPageParser.ParseEndings();
            wordModel.Pronunciation = _ddoPageParser.ParsePronunciation();
            wordModel.Sound = _ddoPageParser.ParseSound();
            wordModel.Definitions = _ddoPageParser.ParseDefinitions();
            wordModel.Examples = _ddoPageParser.ParseExamples();

            // Download and parse a page from Slovar.dk
            string slovardkUrl = GetSlovardkUri(wordModel.Word);

            string slovardkPageHtml = await _fileDownloader.DownloadPageAsync(slovardkUrl, Encoding.GetEncoding(1251));
            _slovardkPageParser.LoadHtml(slovardkPageHtml);

            var translations = _slovardkPageParser.ParseWord();
            wordModel.Translations = translations;

            return wordModel;
        }
    }
}
