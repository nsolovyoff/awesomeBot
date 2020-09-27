using AwesomeBot.DocumentsHelpers.Interfaces;
using System.IO;
using Xceed.Words.NET;

namespace AwesomeBot.DocumentsHelpers
{
    public class DocxDocumentGenerator : IDocumentGenerator
    {
        public string Extension => ".docx";
        public MemoryStream GenerateFromText(string text)
        {
            var stream = new MemoryStream();

            var doc = DocX.Create(stream);
            doc.InsertParagraph(text);
            doc.SaveAs(stream);

            return stream;
        }
    }
}
