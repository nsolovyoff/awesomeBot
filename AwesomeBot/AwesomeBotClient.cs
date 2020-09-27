using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Collections.Generic;
using System.Linq;
using AwesomeBot.CommandHandlers;
using AwesomeBot.CommandHandlers.Documents;
using System.Threading.Tasks;

namespace AwesomeBot
{
    public class AwesomeBotClient : IDisposable
    {
        private readonly TelegramBotClient _botClient;

        private List<UserCommand> _selectedCommands = new List<UserCommand>();

        private readonly ImagesCommandHandler _imagesCommandHandler;
        private readonly VoiceCommandHandler _voiceCommandHandler;
        private readonly DocxCommandHandler _docxCommandHandler;
        private readonly PdfCommandHandler _pdfCommandHandler;

        public AwesomeBotClient(ImagesCommandHandler imagesCommandHandler, VoiceCommandHandler voiceCommandHandler,
            DocxCommandHandler docxCommandHandler, PdfCommandHandler pdfCommandHandler, TelegramBotClient botCLient)
        {
            _botClient = botCLient;

            _imagesCommandHandler = imagesCommandHandler;
            _voiceCommandHandler = voiceCommandHandler;
            _docxCommandHandler = docxCommandHandler;
            _pdfCommandHandler = pdfCommandHandler;
        }

        public void Dispose()
        {
            _botClient.StopReceiving();
        }

        public async Task Run()
        {
            var me = await _botClient.GetMeAsync();

            _botClient.OnMessage += OnMessage;
            _botClient.StartReceiving();
        }

        private async void OnMessage(object sender, MessageEventArgs e)
        {
            var selectedCommand = _selectedCommands?.FirstOrDefault(x => x.ChatId == e.Message.Chat.Id);

            if (selectedCommand != null)
            {
                BaseCommandHandler commandHandler = selectedCommand.Command switch
                {
                    ECommandType.Images => _imagesCommandHandler,
                    ECommandType.Voice => _voiceCommandHandler,
                    ECommandType.Pdf => _pdfCommandHandler,
                    ECommandType.Docx => _docxCommandHandler
                };

                await commandHandler.Handle(e.Message);

                _selectedCommands.Remove(selectedCommand);
            }
            else if (e.Message.Text != null && TryParseCommand(e.Message.Text, out var command))
            {
                if (command == ECommandType.Start)
                {
                    await _botClient.SendTextMessageAsync(e.Message.Chat.Id, "Welcome! I am waiting for your command..");
                    return;
                }

                _selectedCommands.Add(new UserCommand { ChatId = e.Message.Chat.Id, Command = command });

                var response = "Ok. Now send " + command switch
                {
                    ECommandType.Images => "image or multiple images",
                    ECommandType.Voice => "voice message",
                    ECommandType.Pdf => "text or images or voice message",
                    ECommandType.Docx => "text or images or voice message",
                };

                await _botClient.SendTextMessageAsync(e.Message.Chat.Id, response);
            }
            else
                await _botClient.SendTextMessageAsync(e.Message.Chat.Id, "Send a valid command");
        }

        private bool TryParseCommand(string str, out ECommandType command)
        {
            command = default;

            if (!str.StartsWith("/"))
                return false;

            return Enum.TryParse(str.Substring(1), ignoreCase: true, out command);
        }

        private class UserCommand
        {
            public long ChatId { get; set; }
            public ECommandType Command { get; set; }
        }

        public enum ECommandType
        {
            Images,
            Voice,
            Pdf,
            Docx,
            Start
        }
    }
}
