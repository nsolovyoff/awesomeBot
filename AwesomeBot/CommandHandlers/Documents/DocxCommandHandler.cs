using AwesomeBot.DocumentsHelpers;
using AwesomeBot.RecognitionServices;
using Telegram.Bot;

namespace AwesomeBot.CommandHandlers.Documents
{
    public class DocxCommandHandler : DocumentCommandHandler
    {
        public DocxCommandHandler(TelegramBotClient botCLient, ImagesRecognitionService imagesRecognitionService,
            VoiceRecognitionService voiceRecognitionService, DocxDocumentGenerator generator)
            : base(botCLient, imagesRecognitionService, voiceRecognitionService, generator) 
        { }
    }
}
