using AwesomeBot.DocumentsHelpers.Interfaces;
using AwesomeBot.RecognitionServices;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace AwesomeBot.CommandHandlers.Documents
{
    public class DocumentCommandHandler : BaseCommandHandler
    {
        private readonly IDocumentGenerator _documentGenerator;
        private readonly VoiceRecognitionService _voiceRecognitionService;
        private readonly ImagesRecognitionService _imagesRecognitionService;

        protected DocumentCommandHandler(TelegramBotClient botCLient, ImagesRecognitionService imagesRecognitionService,
            VoiceRecognitionService voiceRecognitionService, IDocumentGenerator generator)
            : base(botCLient)
        {
            _documentGenerator = generator;
            _voiceRecognitionService = voiceRecognitionService;
            _imagesRecognitionService = imagesRecognitionService;
        }

        protected override bool ValidateMessage(Message message)
        {
            if (message.Text == null && message.Photo == null && message.Voice == null)
                return false;

            return true;
        }

        protected override async Task CustomHandle(Message message)
        {
            var textForDocument = string.Empty;

            if (message.Text != null)
                textForDocument = message.Text;
            else if (message.Photo != null)
            {
                foreach (var photo in message.Photo)
                {
                    MemoryStream photoStream = new MemoryStream();
                    var file = await BotClient.GetInfoAndDownloadFileAsync(photo.FileId, photoStream);
                    textForDocument += await _imagesRecognitionService.RecognizeFromFile(photoStream.ToArray());
                }
            } else if (message.Voice != null)
            {
                MemoryStream voiceStream = new MemoryStream();
                var file = await BotClient.GetInfoAndDownloadFileAsync(message.Voice.FileId, voiceStream);
                textForDocument = await _voiceRecognitionService.RecognizeFromFile(voiceStream.ToArray());
            }

            var documentStream = _documentGenerator.GenerateFromText(textForDocument);

            var bytes = documentStream.ToArray();
            var document = new InputOnlineFile(new MemoryStream(bytes), "Your document" + _documentGenerator.Extension);

            await BotClient.SendTextMessageAsync(message.Chat.Id, "Your file:");
            await BotClient.SendDocumentAsync(message.Chat.Id, document);
        }
    }
}
