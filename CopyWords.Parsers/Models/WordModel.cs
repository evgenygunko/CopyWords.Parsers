using System.Collections.Generic;

namespace CopyWords.Parsers.Models
{
    public class WordModel
    {
        public string Word { get; set; }
        public string Endings { get; set; }
        public string Pronunciation { get; set; }
        public string Definitions { get; set; }
        public string Sound { get; set; }
        public List<string> Examples { get; set; }
        public List<RussianTranslation> Translations { get; set; }
    }
}
