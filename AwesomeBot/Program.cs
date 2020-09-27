using AwesomeBot.CommandHandlers;
using AwesomeBot.CommandHandlers.Documents;
using AwesomeBot.DocumentsHelpers;
using AwesomeBot.RecognitionServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot;

namespace AwesomeBot
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            ConfigureServices();

            var client = _serviceProvider.GetService<AwesomeBotClient>();
            client.Run();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void ConfigureServices()
        {
            var collection = new ServiceCollection();

            var accessToken = "1386136666:AAH8hTSApqHZ6DuHUiHjQ-zKdPZW9W38yUQ";
            collection.AddSingleton(x => new TelegramBotClient(accessToken));

            collection.AddSingleton<AwesomeBotClient>();

            collection.AddSingleton<ImagesRecognitionService>();
            collection.AddSingleton<VoiceRecognitionService>();

            collection.AddSingleton<DocxDocumentGenerator>();
            collection.AddSingleton<PdfDocumentGenerator>();

            collection.AddSingleton<ImagesCommandHandler>();
            collection.AddSingleton<VoiceCommandHandler>();
            collection.AddSingleton<PdfCommandHandler>();
            collection.AddSingleton<DocxCommandHandler>();

            _serviceProvider = collection.BuildServiceProvider();
        }
    }
}
