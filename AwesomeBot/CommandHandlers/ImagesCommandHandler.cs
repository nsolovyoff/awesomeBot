using System.IO;
using AwesomeBot.RecognitionServices;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AwesomeBot.CommandHandlers
{
    public class ImagesCommandHandler : BaseCommandHandler
    {
        private readonly ImagesRecognitionService _imagesRecognitionService;
        public ImagesCommandHandler(TelegramBotClient botClient, ImagesRecognitionService recognitionService) : base(botClient)
        {
            _imagesRecognitionService = recognitionService;
        }

        protected override bool ValidateMessage(Message message)
        {
            if (message.Photo == null)
                return false;

            return true;
        }

        protected override async Task CustomHandle(Message message)
        {
            var recognizedText = string.Empty;

            foreach(var photo in message.Photo)
            {
                MemoryStream stream = new MemoryStream();
                var file = await BotClient.GetInfoAndDownloadFileAsync(photo.FileId, stream);
                recognizedText += await _imagesRecognitionService.RecognizeFromFile(stream.ToArray());
            }

            if (!string.IsNullOrEmpty(recognizedText))
                await BotClient.SendTextMessageAsync(message.Chat.Id, "Recognized text: \n" + recognizedText);
            else
                await BotClient.SendTextMessageAsync(message.Chat.Id, "Text on an image was not recognized");
        }
    }
}
