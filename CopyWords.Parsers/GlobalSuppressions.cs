// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Will be disposed by the ftamework", Scope = "type", Target = "~T:CopyWords.Parsers.Services.FileDownloader")]
[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.Services.IFileDownloader.DownloadPageAsync(System.String,System.Text.Encoding)~System.Threading.Tasks.Task{System.String}")]
[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.Services.FileDownloader.DownloadPageAsync(System.String,System.Text.Encoding)~System.Threading.Tasks.Task{System.String}")]
[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.LookUpWord.GetWordVariationAsync(System.String,System.Boolean)~System.Threading.Tasks.Task{CopyWords.Parsers.Models.WordModel}")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.Services.FileDownloader.DownloadPageAsync(System.String,System.Text.Encoding)~System.Threading.Tasks.Task{System.String}")]

[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.DDOPageParser.ParseDefinitions~System.String")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.DDOPageParser.ParseExamples~System.Collections.Generic.List{System.String}")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.DDOPageParser.ParseExamples~System.Collections.Generic.List{System.String}")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.DDOPageParser.ParseSound~System.String")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.DDOPageParser.ParseSound~System.String")]
[assembly: SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.LookupInDRDictCommand.GetFileName(System.String)~System.String")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.LookupInDRDictCommand.GetFileName(System.String)~System.String")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.PageParserBase.FindElementById(System.String)~HtmlAgilityPack.HtmlNode")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.PageParserBase.FindElementsByClassName(System.String,System.String)~HtmlAgilityPack.HtmlNodeCollection")]
[assembly: SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.LookUpWord.GetSlovardkUri(System.String)~System.String")]

[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Used by clients", Scope = "member", Target = "~M:CopyWords.Parsers.LookUpWord.CheckThatWordIsValid(System.String)~System.ValueTuple{System.Boolean,System.String}")]

[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Reviewed>", Scope = "member", Target = "~P:CopyWords.Parsers.Models.WordModel.Examples")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Reviewed>", Scope = "member", Target = "~P:CopyWords.Parsers.Models.WordModel.Translations")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Reviewed>", Scope = "member", Target = "~P:CopyWords.Parsers.Models.WordModel.VariationUrls")]
[assembly: SuppressMessage("Naming", "VSSpell001:Spell Check", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.LookUpWord.#ctor(CopyWords.Parsers.IDDOPageParser,CopyWords.Parsers.ISlovardkPageParser,CopyWords.Parsers.Services.IFileDownloader)")]
[assembly: SuppressMessage("Naming", "VSSpell001:Spell Check", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.LookUpWord.GetWordVariationAsync(System.String,System.Boolean)~System.Threading.Tasks.Task{CopyWords.Parsers.Models.WordModel}")]
[assembly: SuppressMessage("Naming", "VSSpell001:Spell Check", Justification = "<Reviewed>", Scope = "member", Target = "~M:CopyWords.Parsers.LookUpWord.LookUpWordAsync(System.String,System.Boolean)~System.Threading.Tasks.Task{CopyWords.Parsers.Models.WordModel}")]
[assembly: SuppressMessage("Naming", "VSSpell001:Spell Check", Justification = "<Reviewed>", Scope = "type", Target = "~T:CopyWords.Parsers.Services.IFileDownloader")]
