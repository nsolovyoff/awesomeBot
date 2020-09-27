using AwesomeBot.RecognitionServices.Interfaces;
using Google.Cloud.Speech.V1;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Google.Cloud.Speech.V1.RecognitionConfig.Types;

namespace AwesomeBot.RecognitionServices
{
    public class VoiceRecognitionService : ITextRecognition
    {
        private readonly bool _disabled;
        private readonly SpeechClient _speechClient = SpeechClient.Create();
        private readonly RecognitionConfig _config = new RecognitionConfig
        {
            Encoding = AudioEncoding.OggOpus,
            SampleRateHertz = 16000,
            LanguageCode = LanguageCodes.Ukrainian.Ukraine
        };

        public VoiceRecognitionService(bool disabled = false)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\AwesomeBot-852a66ec21be.json");
            _disabled = disabled;
        }

        public async Task<string> RecognizeFromFile(byte[] audio)
        {
           if (_disabled)
                return "Speech recognition is currently disabled";

            var recognitionAudio = RecognitionAudio.FromBytes(audio);

            RecognizeResponse response = await _speechClient.RecognizeAsync(_config, recognitionAudio);

            var recognized = response.Results
                .SelectMany(result => result.Alternatives.Select(alternative => alternative.Transcript))
                .Aggregate((x, y) => x + " " +  y);

            return recognized;
        }
    }
}
