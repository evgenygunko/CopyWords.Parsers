﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CopyWords.Parsers
{
    public interface IDDOPageParser : IPageParser
    {
        int GetWordsCount();

        string ParseWord();

        string ParseEndings();

        string ParsePronunciation();

        string ParseSound();

        string ParseDefinitions();

        List<string> ParseExamples();
    }

    public class DDOPageParser : PageParserBase, IDDOPageParser
    {
        /// <summary>
        /// Gets the words count for given search.
        /// </summary>
        /// <returns>The words count.</returns>
        public int GetWordsCount()
        {
            var div = FindElementById("opslagsordBox_collapsed");

            var wordsCountDiv = div.SelectSingleNode("./div/div[contains(@class, 'diskret')]");
            if (wordsCountDiv == null)
            {
                throw new PageParserException("Cannot find a span element with CSS class 'diskret'");
            }

            string wordsCountText = DecodeText(wordsCountDiv.InnerText);

            return ParseWordsCountText(wordsCountText);
        }

        /// <summary>
        /// Gets a string which contains found Danish word.
        /// </summary>
        public string ParseWord()
        {
            var div = FindElementByClassName("div", "definitionBoxTop");

            var wordSpan = div.SelectSingleNode("//*[contains(@class, 'match')]");
            if (wordSpan == null)
            {
                throw new PageParserException("Cannot find a span element with CSS class 'match'");
            }

            return DecodeText(wordSpan.InnerText);
        }

        /// <summary>
        /// Gets endings for found word.
        /// </summary>
        public string ParseEndings()
        {
            string endings = string.Empty;

            var div = FindElementById("id-boj");

            if (div != null)
            {
                var spanEndings = div.SelectSingleNode("./span[contains(@class, 'tekstmedium allow-glossing')]");
                if (spanEndings != null)
                {
                    endings = spanEndings.InnerText;
                }
            }

            return DecodeText(endings);
        }

        /// <summary>
        /// Gets pronunciation for found word.
        /// </summary>
        public string ParsePronunciation()
        {
            string pronunciation = string.Empty;

            var div = FindElementById("id-udt");

            if (div != null)
            {
                var span = div.SelectSingleNode("./span/span[contains(@class, 'lydskrift')]");
                if (span != null)
                {
                    pronunciation = DecodeText(span.InnerText);
                }
            }

            return pronunciation;
        }

        /// <summary>
        /// Gets path to sound file for found word (which would be an URL).
        /// </summary>
        public string ParseSound()
        {
            string soundUrl = string.Empty;

            var div = FindElementById("id-udt");

            if (div != null)
            {
                var ahref = div.SelectSingleNode("./span/span/audio/div/a");
                if (ahref != null && ahref.Attributes["href"] != null)
                {
                    soundUrl = ahref.Attributes["href"].Value;

                    if (!soundUrl.EndsWith(".mp3"))
                    {
                        throw new PageParserException(
                            string.Format("Sound URL must have '.mp3' at the end. Parsed value = '{0}'", soundUrl));
                    }
                }
            }

            return soundUrl;
        }

        /// <summary>
        /// Gets definitions for found word. It will concatenate different definitions into one string with line breaks.
        /// </summary>
        public string ParseDefinitions()
        {
            string definitions = string.Empty;

            var contentBetydningerDiv = FindElementById("content-betydninger");

            if (contentBetydningerDiv != null)
            {
                var definitionsDivs = contentBetydningerDiv.SelectNodes("./div/div/span/span[contains(@class, 'definition')]");

                if (definitionsDivs != null && definitionsDivs.Count > 0)
                {
                    for (int i = 0; i < definitionsDivs.Count; i++)
                    {
                        if (definitionsDivs.Count > 1)
                        {
                            definitions += string.Format("{0}{1}. {2}", Environment.NewLine, (i + 1).ToString(), DecodeText(definitionsDivs[i].InnerText));
                        }
                        else
                        {
                            definitions += string.Format("{0}", DecodeText(definitionsDivs[i].InnerText));
                        }
                    }
                }
            }

            return definitions.Trim();
        }

        /// <summary>
        /// Gets examples for found word. It will also add a full stop at the end of each example.
        /// </summary>
        public List<string> ParseExamples()
        {
            List<string> examples = new List<string>();

            var contentBetydningerDiv = FindElementById("content-betydninger");

            if (contentBetydningerDiv != null)
            {
                // can't run XPath with a search for any depth - we want to take examples only from "top level" meaning
                // var examplesDivs = contentBetydningerDiv.SelectNodes("descendant::span[@class='citat']");
                var examplesDivs = contentBetydningerDiv.SelectNodes("./div/div/div/span[@class='citat']");
                if (examplesDivs == null)
                {
                    examplesDivs = contentBetydningerDiv.SelectNodes("./div/div/div/div/span[@class='citat']");
                }

                if (examplesDivs != null)
                {
                    for (int i = 0; i < examplesDivs.Count; i++)
                    {
                        string example = DecodeText(examplesDivs[i].InnerText);
                        if ((example.EndsWith(".") || example.EndsWith("!") || example.EndsWith("?")) == false)
                        {
                            example += ".";
                        }

                        examples.Add(string.Format("{0}. {1}", (i + 1).ToString(), example));
                    }
                }
            }

            return examples;
        }

        internal int ParseWordsCountText(string wordsCountText)
        {
            string numericValue = new string(wordsCountText.Where(char.IsDigit).ToArray());
            if (int.TryParse(numericValue, out int result))
            {
                return result;
            }

            return 1;
        }
    }
}
