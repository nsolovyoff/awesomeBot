using AwesomeBot.RecognitionServices;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AwesomeBot.CommandHandlers
{
    public class VoiceCommandHandler : BaseCommandHandler
    {
        private readonly VoiceRecognitionService _voiceRecognitionService;
        public VoiceCommandHandler(TelegramBotClient botCLient, VoiceRecognitionService recognitionService) : base(botCLient)
        {
            _voiceRecognitionService = recognitionService;
        }

        protected override async Task CustomHandle(Message message)
        {
            var recognizedText = string.Empty;

            MemoryStream stream = new MemoryStream();
            var file = await BotClient.GetInfoAndDownloadFileAsync(message.Voice.FileId, stream);
            recognizedText += await _voiceRecognitionService.RecognizeFromFile(stream.ToArray());

            if (!string.IsNullOrEmpty(recognizedText))
                await BotClient.SendTextMessageAsync(message.Chat.Id, "Recognized text: \n" + recognizedText);
            else
                await BotClient.SendTextMessageAsync(message.Chat.Id, "Speech on a voice message was not recognized");
        }

        protected override bool ValidateMessage(Message message)
        {
            if (message.Voice == null)
                return false;

            return true;
        }
    }
}
