using AwesomeBot.RecognitionServices.Interfaces;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Tesseract;

namespace AwesomeBot.RecognitionServices
{
    public class ImagesRecognitionService : ITextRecognition
    {
        public ImagesRecognitionService()
        {
        }

        public async Task<string> RecognizeFromFile(byte[] img)
        {
            var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
            var jpgStream = new MemoryStream(img);
            var image = Image.FromStream(jpgStream);

            var tiffStream = new MemoryStream();
            image.Save(tiffStream, System.Drawing.Imaging.ImageFormat.Tiff);

            var pix = Pix.LoadTiffFromMemory(tiffStream.ToArray());

            var page = engine.Process(pix);

            var text = page.GetText();

            page.Dispose();

            return text;
        }
    }
}
