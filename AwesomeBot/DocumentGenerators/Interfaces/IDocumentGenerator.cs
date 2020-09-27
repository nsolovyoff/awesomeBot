using System.IO;

namespace AwesomeBot.DocumentsHelpers.Interfaces
{
    public interface IDocumentGenerator
    {
        public string Extension { get; }
        public MemoryStream GenerateFromText(string text);
    }
}
