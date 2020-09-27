using AwesomeBot.DocumentsHelpers.Interfaces;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using System.Text;

namespace AwesomeBot.DocumentsHelpers
{
    public class PdfDocumentGenerator : IDocumentGenerator
    {
        public string Extension => ".pdf";
        public PdfDocumentGenerator()
        {
            var provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);
        }

        public MemoryStream GenerateFromText(string text)
        {
            PdfDocument pdf = new PdfDocument();
            PdfPage pdfPage = pdf.AddPage();

            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Verdana", 20, XFontStyle.Regular);

            graph.DrawString(text, font, XBrushes.Black, new XRect(0, 0, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

            var stream = new MemoryStream();

            pdf.Save(stream);

            return stream;
        }
    }
}
