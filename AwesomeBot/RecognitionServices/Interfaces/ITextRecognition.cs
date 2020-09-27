using System.Threading.Tasks;

namespace AwesomeBot.RecognitionServices.Interfaces
{
    interface ITextRecognition
    {
        public Task<string> RecognizeFromFile(byte[] source);
    }
}
